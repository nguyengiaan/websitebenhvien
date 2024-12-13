
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/friendHub")
.build();
connection.start().then(function () {
console.log("SignalR connected.");
}).catch(function (err) {
return console.error(err.toString());
});
connection.on("ReceiveNotification", function () {
    GetNotification();
});

$(document).ready(function(){
    GetNotification();
});
function GetNotification(){
    $.ajax({
        url:"/api/thongbao", 
        type:"GET",
        success:function(data){
            if(data.status){
                var html="";
                var count=0;
                data.data.forEach(function(item)
                {
                    if(item.status==false){
                        count++;
                    }
                    if(count < 5 || item.status == false) {
                        html+=` <a class="dropdown-item d-flex align-items-center" href="${item.url}">
                                    <div class="mr-3">
                                        <div class="icon-circle ${item.status ? 'bg-success' : 'bg-danger'}">
                                            <i class="fas fa-file-alt text-white"></i>
                                        </div>
                                    </div>
                                    <div>
                                        <div class="small text-gray-500">${new Date(item.createat).toLocaleString('vi-VN')}</div>
                                        <span class="font-weight-bold">${item.title}</span>
                                        <div class="small text-gray-500">${item.description}</div>
                                    </div>
                            </a>
                          `
                    }
                });
                if(count == 0){
                    $("#count-notification").text("");
                }else{
                    $("#count-notification").text(count);
                }
                $("#notification").html(html);
            }
        }
    });
}
function ViewNotification(){
    $.ajax({
        url:"/api/xemthongbao",
        type:"GET",
        success:function(data){
            if(data.status){
                GetNotification();
            }
        }
    });
}
