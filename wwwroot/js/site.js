
$(document).ready(function() {
    // Ẩn select language nếu tồn tại
    Header();

    $(window).scroll(function() {
        if ($(this).scrollTop() > 200) {
            $('#backToTop').fadeIn();
        } else {
            $('#backToTop').fadeOut();
        }
    });

    $('#backToTop').click(function() {
        $('html, body').animate({scrollTop: 0}, 'slow');
        return false;
    });
});
function changeLanguage(languageCode) {
var select = document.querySelector('.goog-te-combo');
if (select) {
    select.value = languageCode;
    select.dispatchEvent(new Event('change'));
}
}
function Header() {
    console.log("ok");  
    $.ajax({
        url: "/Pagemain/GetTitleheader",
        type: "GET",
        success: function (res) {
            if (res.success) {
                console.log(res.data);
                // Update logo
                if (res.data.logo) {
                    $('img[alt="Logo"]').attr('src', '/Resources/' + res.data.logo);
                }
                
                // Update emergency phone number
                if (res.data.telephone) {
                    $('.text-danger.fw-bold').html('<i class="fas fa-ambulance fa-lg me-2"></i>Cấp cứu 24/7: ' + res.data.telephone);
                }
                
                // Update navigation links
                var navLinks = '';
                if (res.data.titleheader && Array.isArray(res.data.titleheader)) {
                    res.data.titleheader.forEach(function(item) {
                        if (!item || !item.id_titleheader) return;
                        
                        var icon = '';
                        switch(item.id_titleheader) {
                            case 'tl1':
                                icon = 'icon-user.png';
                                break;
                            case 'tl2': 
                                icon = 'icon-hoidap.png';
                                break;
                            case 'tl3':
                                icon = 'icon-cacu.png';
                                break;
                            default:
                                return;
                        }
                        
                        navLinks += `<a href="${item.link || '#'}" class="text-decoration-none text-dark" style="font-family: 'Montserrat', sans-serif; font-weight: bold;">
                            <img src="/image/${icon}" alt="Icon" class="me-2" style="height: 25px;"> ${item.title || ''}
                        </a>`;
                    });
                
                    $('.d-none.d-md-flex.flex-wrap').html(navLinks);
                }
            } else {
                console.error("Dữ liệu không hợp lệ:", res.message);
            }
        },
        error: function (xhr, status, error) {
            console.error("Có lỗi xảy ra:", error);
        }
    });
}
