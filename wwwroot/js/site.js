﻿const connection = new signalR.HubConnectionBuilder()
    .withUrl("/friendHub")
    .build();

connection.start()
    .then(function () {
        console.log("SignalR connected.");
    })
    .catch(function (err) {
        console.error(err.toString());
    });
const sampleQuestions = [];

const sampleAnswers = {};
connection.on("ReceiveNotification", function () {
    ViewChat();
});
$(document).ready(function() {
    // Ẩn select language nếu tồn tại
  
    GetSchema();
    GetAllEquipment();
    ListService();
 
    loadSpecialties();
    laydscauhoi();
    $(window).scroll(function() {
        if ($(this).scrollTop() > 200) {
            $('#backToTop').fadeIn();
        } else {
            $('#backToTop').fadeOut();
        }
    });
    $('#btnRegister').click(function(){
        registerAppointment();
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
function GetSchema() {
    $.ajax({
        url: "/Pagemain/TimeWork",
        type: "GET",
        success: function (res) {
            if (res.success) {
                $('#timework').html(res.data.content);

                // Kiểm tra chiều cao nội dung
                const content = $('#timework');
                const readMoreBtn = $('#btnReadMore');

                if (content[0].scrollHeight > 300) {
                    readMoreBtn.show(); // Hiện nút "Xem thêm"
                }

                readMoreBtn.on('click', function () {
                    if (content.css('max-height') === '300px') {
                        content.css('max-height', 'none'); // Mở rộng toàn bộ nội dung
                        $(this).text('Thu gọn'); // Thay đổi text nút
                    } else {
                        content.css('max-height', '300px'); // Thu nhỏ nội dung lại
                        $(this).text('Xem thêm');
                    }
                });
            }
        }
    });
}
function ListService() {
    $.ajax({
        url: "/Allinone/ListService",
        type: "GET",
        success: function(response) {
            if (response.status) {
                displayServiceCarousel(response.data);
            } else {
                alert("Không thể tải danh sách dịch vụ");
            }
        },
        error: function(error) {
            console.log(error);
            alert("Đã xảy ra lỗi khi tải danh sách dịch vụ");
        }
    });
}
function displayServiceCarousel(productData) {
    const carousel = document.querySelector('#serviceCarousel');
    carousel.innerHTML = `
        <div class="carousel-inner" id="serviceContainer"></div>
        <button class="carousel-control-prev custom-carousel-control" type="button" data-bs-target="#serviceCarousel" data-bs-slide="prev">
            <i class="fas fa-chevron-left"></i>
        </button>
        <button class="carousel-control-next custom-carousel-control" type="button" data-bs-target="#serviceCarousel" data-bs-slide="next">
            <i class="fas fa-chevron-right"></i>
        </button>
    `;

    const productContainer = document.querySelector('#serviceContainer');
    
    // Group items into slides based on screen size
    const slides = [];
    let currentSlide = [];
    const itemsPerSlide = window.innerWidth < 768 ? 1 : 3;

    productData.forEach((item, index) => {
        if (currentSlide.length === itemsPerSlide) {
            slides.push(currentSlide);
            currentSlide = [];
        }
        currentSlide.push(item);
    });

    if (currentSlide.length > 0) {
        slides.push(currentSlide);
    }

    // Create carousel slides
    slides.forEach((slideItems, slideIndex) => {
        const slide = document.createElement('div');
        slide.className = `carousel-item ${slideIndex === 0 ? 'active' : ''}`;

        const row = document.createElement('div');
        row.className = 'row g-4 justify-content-center';

        slideItems.forEach(item => {
            if (item.status) {
                const col = document.createElement('div');
                col.className = window.innerWidth < 768 ? 'col-12' : 'col-md-4';

                col.innerHTML = `
                    <div class="service-card">
                        <div class="service-card-inner">
                            <div class="service-card-front">
                                <img src="/Resources/${item.url}" alt="${item.title}" class="service-image">
                                <div class="service-overlay">
                                    <h4 class="service-title">${item.title}</h4>
                                </div>
                            </div>
                            <div class="service-card-back">
                                <div class="service-content">
                                    <h4>${item.title}</h4>
                                    <p class="service-description">${item.description || 'Xem chi tiết để biết thêm thông tin'}</p>
                                    <a href="${item.alias_url}" class="service-btn">
                                        Xem chi tiết
                                        <i class="fas fa-arrow-right ms-2"></i>
                                    </a>
                                </div>
                            </div>
                        </div>
                    </div>
                `;

                const serviceCard = col.querySelector('.service-card');
                
                // Add modern hover effects
                serviceCard.addEventListener('mouseenter', () => {
                    serviceCard.style.transform = 'scale(1.05)';
                    serviceCard.style.boxShadow = '0 10px 20px rgba(0,0,0,0.2)';
                });
                
                serviceCard.addEventListener('mouseleave', () => {
                    serviceCard.style.transform = 'scale(1)';
                    serviceCard.style.boxShadow = '0 5px 15px rgba(0,0,0,0.1)';
                });

                row.appendChild(col);
            }
        });

        slide.appendChild(row);
        productContainer.appendChild(slide);
    });

    // Add custom styling
    const style = document.createElement('style');
    style.textContent = `
        .service-card {
            position: relative;
            height: 400px;
            border-radius: 15px;
            overflow: hidden;
            transition: all 0.3s ease;
            box-shadow: 0 5px 15px rgba(0,0,0,0.1);
        }
        
        .service-image {
            width: 100%;
            height: 100%;
            object-fit: cover;
            object-position: center;
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
        }
        
        .service-overlay {
            position: absolute;
            bottom: 0;
            left: 0;
            right: 0;
            background: linear-gradient(to top, rgba(0,0,0,0.8), transparent);
            padding: 20px;
            color: white;
        }
        
        .service-title {
            margin: 0;
            font-size: 1.5rem;
            font-weight: 600;
        }
        
        .service-card-inner {
            position: relative;
            width: 100%;
            height: 100%;
            transition: transform 0.8s;
            transform-style: preserve-3d;
        }
        
        .service-card:hover .service-card-inner {
            transform: rotateY(180deg);
        }
        
        .service-card-front, .service-card-back {
            position: absolute;
            width: 100%;
            height: 100%;
            backface-visibility: hidden;
        }
        
        .service-card-back {
            background: linear-gradient(135deg, #14A1FF, #0056b3);
            transform: rotateY(180deg);
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 20px;
            color: white;
            text-align: center;
        }
        
        .service-btn {
            display: inline-block;
            padding: 10px 25px;
            background: white;
            color: #14A1FF;
            text-decoration: none;
            border-radius: 25px;
            margin-top: 15px;
            font-weight: 600;
            transition: all 0.3s ease;
        }
        
        .service-btn:hover {
            background: #f8f9fa;
            transform: translateY(-2px);
        }
        
        .custom-carousel-control {
            width: 40px;
            height: 40px;
            background: #14A1FF;
            border-radius: 50%;
            opacity: 0.9;
            top: 50%;
            transform: translateY(-50%);
        }
        
        .custom-carousel-control:hover {
            opacity: 1;
            background: #0056b3;
        }
    `;
    document.head.appendChild(style);

    // Initialize carousel with custom settings
    new bootstrap.Carousel(carousel, {
        interval: 5000,
        wrap: true,
        touch: true
    });

    // Handle responsive layout
    window.addEventListener('resize', () => {
        displayServiceCarousel(productData);
    });
}
function getSessionId() 
{
    // Decode cookie string to handle special characters
    const decodedCookie = decodeURIComponent(document.cookie);
    const cookies = decodedCookie.split(';');
    for (let cookie of cookies) {
        // Trim whitespace and split on first =
        let [name, ...value] = cookie.trim().split('=');
        // Join value parts back together in case value contained =
        value = value.join('=');
        
        if (name === 'SessionId') {
            return value;
        }

    }
 
    return null;
}
// xem tin nhắn khách hàng 
function ViewChat() {
    const sessionId = getSessionId();
    if (!sessionId) {
        console.log("SessionId cookie not found");
        return;
    }

    $.ajax({
        url: "/api/lay-tin-nhan",
        type: "POST",
        data: { id: sessionId },
        success: function(response) {
            if(response.status)
            {
                renderChat(response.data);
            }
        }
    });
}
// render chat
function renderChat(data) {
    const chatMessages = document.getElementById('chatMessages');
    chatMessages.innerHTML = ''; // Xóa các tin nhắn hiện tại

    // Thêm tin nhắn chào mừng nếu không có tin nhắn nào
    if (!data || data.length === 0) {
        chatMessages.innerHTML = `
            <div class="mb-2 animate__animated animate__fadeInLeft">
                <div class="d-inline-block bg-light rounded p-2" style="position: relative; max-width: 80%;">
                    <p class="m-0">Xin chào! Chúng tôi có thể giúp gì cho bạn? 👋</p>
                </div>
            </div>`;
        renderSampleQuestions();
        return;
    }

    // Hiển thị từng tin nhắn
    data.forEach(message => {
        const isAdmin = message.id_Sender === 'admin';
        const messageHtml = `
            <div class="mb-2 animate__animated ${isAdmin ? 'animate__fadeInLeft' : 'animate__fadeInRight'}" 
                 style="display: flex; justify-content: ${isAdmin ? 'flex-start' : 'flex-end'}">
                <div class="d-inline-block ${isAdmin ? 'bg-light' : 'bg-primary text-white'} rounded p-2" 
                     style="position: relative; max-width: 80%; word-break: break-word;">
                    <p class="m-0">${message.message}</p>
                </div>
            </div>`;
        chatMessages.innerHTML += messageHtml;
    });

    // Cuộn xuống cuối cùng
    chatMessages.scrollTop = chatMessages.scrollHeight;
}
// Hiển thị các câu hỏi mẫu
function renderSampleQuestions() {
    const chatMessages = document.getElementById('chatMessages');
    chatMessages.innerHTML = ''; // Fix: Changed from .html to .innerHTML
    const questionsHtml = sampleQuestions.map(question => `
        <div class="mb-2 animate__animated animate__fadeInLeft">
            <button class="btn btn-outline-secondary w-100 text-start" onclick="sendSampleQuestion('${question}')">${question}</button>
        </div>
    `).join('');
    chatMessages.innerHTML = questionsHtml; // Fix: Changed from += to = to avoid duplicating
}
// Gửi câu hỏi mẫu
function sendSampleQuestion(question) {
    const messageInput = document.getElementById('messageInput');
    messageInput.value = question;
    sendMessage();
}
// Ghi đè hàm sendMessage để xử lý câu trả lời tĩnh
function sendMessage() {
    const messageInput = document.getElementById('messageInput');
    const message = messageInput.value.trim();
    if (message) {
        const chatMessages = document.getElementById('chatMessages');
        chatMessages.innerHTML += `<div class="mb-2 text-end">
            <div class="d-inline-block bg-primary text-white rounded p-2">
                ${message}
            </div>
        </div>`;

        // Kiểm tra nếu tin nhắn là câu hỏi mẫu
        if (sampleAnswers[message]) {
            setTimeout(() => {
                chatMessages.innerHTML += `<div class="mb-2">
                    <div class="d-inline-block bg-light rounded p-2">
                        ${sampleAnswers[message]}
                    </div>
                </div>`;
                chatMessages.scrollTop = chatMessages.scrollHeight;
            }, 500);
        }

        messageInput.value = '';
        chatMessages.scrollTop = chatMessages.scrollHeight;
    }
}
// Gửi câu hỏi mẫu
function sendSampleQuestion(question) {
    const messageInput = document.getElementById('messageInput');
    messageInput.value = question;
    sendMessage();
}
// Gọi hàm renderSampleQuestions khi cửa sổ chat được mở
document.getElementById('chatButton').addEventListener('click', renderSampleQuestions);
// Sample messages

document.addEventListener('DOMContentLoaded', function() {
  
  
    const chatButton = document.getElementById('chatButton');
    const chatWindow = document.getElementById('chatWindow');
    const closeChatBtn = document.getElementById('closeChatBtn');
    const messageInput = document.getElementById('messageInput');
    const sendMessageBtn = document.getElementById('sendMessageBtn');
    const chatMessages = document.getElementById('chatMessages');
    ViewChat();
    // Toggle chat window
    chatButton.addEventListener('click', function() {
        chatWindow.style.display = chatWindow.style.display === 'none' ? 'block' : 'none';
    });

    // Close chat window
    closeChatBtn.addEventListener('click', function() {
        chatWindow.style.display = 'none';
    });
   


    // Send message
    function sendMessage() {
        const message = messageInput.value.trim();
        if (message) {
            $.ajax({
                url: "/api/gui-cho-admin",
                type: "POST",
                data: { message: message },
                success: function(response) {
                    if(response.status)
                    {
                        messageInput.value = '';
                        chatMessages.innerHTML += `<div class="mb-2 text-end">
                            <div class="d-inline-block bg-primary text-white rounded p-2">
                                ${message}
                            </div>
                        </div>`;
                        ViewChat();
                    }
                    else
                    {
                        alert(response.message);
                    }
                }
            });
        }
    }

    sendMessageBtn.addEventListener('click', sendMessage);
    messageInput.addEventListener('keypress', function(e) {
        if (e.key === 'Enter') {
            sendMessage();
        }
    });
    // Get SessionId from cookie
  
});

function loadDoctor(id) {
    $.ajax({
        url: '/api/bac-si',
        type: 'POST',
        data: { id: id },
        success: function (response) {

            let options = '<option value="">-- Chọn bác sĩ --</option>'; 
            response.data.forEach(function (doctor) {
                options += `<option value="${doctor.id_doctor}">${doctor.name}</option>`;
            });
            $('#Id_doctor').html(options);

            $('#Id_doctor').off('change').on('change', function() {
                const selectedDoctorId = $(this).val();
                if (selectedDoctorId) {
                    loadDoctorSchedule(selectedDoctorId);
                } else {
                    $('#calendar').html('');
                }
            });
        },
        error: function () {
            Swal.fire({
                title: 'Lỗi!',
                text: 'Không thể tải dữ liệu bác sĩ',
                icon: 'error'
            });
        }
    });
}
function loadDoctorSchedule(doctorId) {
    if (!doctorId) {
        console.error('Doctor ID is required');
        return;
    }

    const calendarContainer = document.getElementById('calendar');
    if (!calendarContainer) {
        console.error('Calendar container not found');
        return;
    }

    const today = new Date();
    today.setHours(0, 0, 0, 0); // Set to start of the day
    
    calendarContainer.innerHTML = '<div class="text-center"><i class="fas fa-spinner fa-spin"></i> Đang tải lịch làm việc...</div>';

    $.ajax({
        url: '/api/lich-lam-viec',
        type: 'POST',
        data: {
            id_doctor: doctorId
        },
        success: function(response) {
            if (response.status && response.data) {
                const workSchedules = response.data.workschedules.filter(schedule => {
                    const scheduleDate = new Date(schedule.date);
                    scheduleDate.setHours(0, 0, 0, 0); // Set to start of the day
                    return scheduleDate >= today;
                });
          
                if (!workSchedules || workSchedules.length === 0) {
                    calendarContainer.innerHTML = '<div class="alert alert-info">Bác sĩ chưa có lịch làm việc</div>';
                    return;
                }

                let calendarHtml = `
                    <div class="calendar-wrapper p-3 bg-white rounded shadow-sm">
                        <div class="mb-4">
                            <label class="form-label fw-bold text-primary">
                                <i class="fas fa-calendar-alt me-2"></i>Chọn ngày khám
                            </label>
                            <select class="form-select form-select-lg border-0 bg-light" id="datePicker">
                                <option value="">-- Vui lòng chọn ngày --</option>
                `;

                workSchedules.forEach(schedule => {
                    const scheduleDate = new Date(schedule.date);
                    const formattedDate = scheduleDate.toLocaleDateString('vi-VN', {
                        weekday: 'long',
                        year: 'numeric',
                        month: 'long',
                        day: 'numeric'
                    });

                    calendarHtml += `
                        <option value="${schedule.date}" 
                                data-morning="${schedule.morning}" 
                                data-afternoon="${schedule.afternoon}" 
                                data-evening="${schedule.evening}" 
                                data-id-schedule="${schedule.id}">
                            ${formattedDate}
                        </option>
                    `;
                });

                calendarHtml += `
                            </select>
                        </div>
                        <div class="time-slots mb-3" style="display:none" id="timePickerWrapper">
                            <label class="form-label fw-bold text-primary">
                                <i class="fas fa-clock me-2"></i>Chọn giờ khám
                            </label>
                            <div class="row g-2" id="timePicker"></div>
                        </div>
                `;

                calendarContainer.innerHTML = calendarHtml;

                $('#datePicker').off('change').on('change', function() {
                    const selectedOption = $(this).find(':selected');
                    const selectedDate = $(this).val();
                    const morning = selectedOption.data('morning');
                    const afternoon = selectedOption.data('afternoon');
                    const evening = selectedOption.data('evening');
                    const scheduleId = selectedOption.data('id-schedule');
                    
                    const timePickerWrapper = $('#timePickerWrapper');
                    const timePicker = $('#timePicker');
                    
                    if(selectedDate) {
                        let timeSlots = '';
                        
                        if(morning) {
                            timeSlots += `
                                <div class="col-12 mb-2">
                                    <div class="time-period">
                                        <small class="text-muted"><i class="fas fa-sun me-1"></i>Buổi sáng (7h-11h)</small>
                                    </div>
                                </div>
                            `;
                            for(let hour = 7; hour <= 11; hour++) {
                                timeSlots += `
                                    <div class="col-6 col-md-3">
                                        <input type="radio" class="btn-check" name="timeSlot" id="time${hour}00" value="${String(hour).padStart(2, '0')}:00">
                                        <label class="btn btn-outline-primary w-100" for="time${hour}00">${hour}:00</label>
                                    </div>
                                    <div class="col-6 col-md-3">
                                        <input type="radio" class="btn-check" name="timeSlot" id="time${hour}30" value="${String(hour).padStart(2, '0')}:30">
                                        <label class="btn btn-outline-primary w-100" for="time${hour}30">${hour}:30</label>
                                    </div>
                                `;
                            }
                        }
                        
                        if(afternoon) {
                            timeSlots += `
                                <div class="col-12 mb-2 mt-3">
                                    <div class="time-period">
                                        <small class="text-muted"><i class="fas fa-sun me-1"></i>Buổi chiều (13h-16h)</small>
                                    </div>
                                </div>
                            `;
                            for(let hour = 13; hour <= 16; hour++) {
                                timeSlots += `
                                    <div class="col-6 col-md-3">
                                        <input type="radio" class="btn-check" name="timeSlot" id="time${hour}00" value="${String(hour).padStart(2, '0')}:00">
                                        <label class="btn btn-outline-primary w-100" for="time${hour}00">${hour}:00</label>
                                    </div>
                                    <div class="col-6 col-md-3">
                                        <input type="radio" class="btn-check" name="timeSlot" id="time${hour}30" value="${String(hour).padStart(2, '0')}:30">
                                        <label class="btn btn-outline-primary w-100" for="time${hour}30">${hour}:30</label>
                                    </div>
                                `;
                            }
                        }

                        if(evening) {
                            timeSlots += `
                                <div class="col-12 mb-2 mt-3">
                                    <div class="time-period">
                                        <small class="text-muted"><i class="fas fa-moon me-1"></i>Buổi tối (18h-20h)</small>
                                    </div>
                                </div>
                            `;
                            for(let hour = 18; hour <= 20; hour++) {
                                timeSlots += `
                                    <div class="col-6 col-md-3">
                                        <input type="radio" class="btn-check" name="timeSlot" id="time${hour}00" value="${String(hour).padStart(2, '0')}:00">
                                        <label class="btn btn-outline-primary w-100" for="time${hour}00">${hour}:00</label>
                                    </div>
                                    <div class="col-6 col-md-3">
                                        <input type="radio" class="btn-check" name="timeSlot" id="time${hour}30" value="${String(hour).padStart(2, '0')}:30">
                                        <label class="btn btn-outline-primary w-100" for="time${hour}30">${hour}:30</label>
                                    </div>
                                `;
                            }
                        }
                        
                        timePicker.html(timeSlots);
                        timePickerWrapper.slideDown();

                        $('input[name="timeSlot"]').off('change').on('change', function() {
                            const selectedTime = $(this).val();
                            const appointmentDateTime = `${selectedDate}T${selectedTime}`;
                            
                            if(!$('#appointmentTime').length) {
                                calendarContainer.insertAdjacentHTML('beforeend', `
                                    <input type="hidden" id="appointmentTime" value="${appointmentDateTime}">
                                    <input type="hidden" id="scheduleId" value="${scheduleId}">
                                `);
                            } else {
                                $('#appointmentTime').val(appointmentDateTime);
                                $('#scheduleId').val(scheduleId);
                            }
                        });
                    } else {
                        timePickerWrapper.slideUp();
                    }
                });
            } else {
                calendarContainer.innerHTML = '<div class="alert alert-warning">Hôm nay bác sĩ không có lịch làm việc</div>';
            }
        },
        error: function(xhr, status, error) {
            console.error('Error loading doctor schedule:', error);
            calendarContainer.innerHTML = '<div class="alert alert-danger">Đã xảy ra lỗi khi tải lịch làm việc</div>';
            Swal.fire({
                title: 'Lỗi',
                text: 'Không thể tải lịch khám của bác sĩ', 
                icon: 'error'
            });
        }
    });
}
function loadSpecialties() {
    $.ajax({
        url: '/api/chuyen-khoa-catogery',
        type: 'GET',
        success: function (response) {
            let options = '<option value="">-- Chọn chuyên khoa --</option>';
            response.data.forEach(function (specialty) {
                options += `<option value="${specialty.id_Specialty}">${specialty.title}</option>`;
            });
            $('#Id_specialty').html(options);

            $('#Id_specialty').off('change').on('change', function () {
                const selectedSpecialtyId = $(this).val();
                if (selectedSpecialtyId) {
                    loadDoctor(selectedSpecialtyId);
                } else {
                    $('#Id_doctor').html('<option value="">-- Chọn bác sĩ --</option>');
                    $('#calendar').html('');
                }
            });
        },
        error: function (err) {
            console.error('Error loading specialties:', err);
            Swal.fire({
                title: 'Lỗi',
                text: 'Không thể tải danh sách chuyên khoa',
                icon: 'error'
            });
        }
    });
}
function registerAppointment() {
    let appointmentTimeStr = $('#appointmentTime').val() || $('#appointmentTimeSK').val();
    if (appointmentTimeStr && !appointmentTimeStr.match(/^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}T\d{2}:\d{2}$/)) {
        const date = new Date(appointmentTimeStr);
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const day = String(date.getDate()).padStart(2, '0');
        const hours = String(date.getHours()).padStart(2, '0');
        const minutes = String(date.getMinutes()).padStart(2, '0');
        appointmentTimeStr = `${year}-${month}-${day}T00:00:00T${hours}:${minutes}`;
    }
    console.log(appointmentTimeStr);
    const parts = appointmentTimeStr.split('T');
    const datePart = parts[0]; // "2024-12-29"
    const timePart = parts[2] || parts[1].split(':').slice(1).join(':'); // "09:30"
    
    // Tạo chuỗi mới
    const result = `${datePart}T${timePart}:00`;

    const appointmentData = {
        Name_doctor: $('#Id_doctor option:selected').text() || 'Không có',
        Examinationtime: result,
        name: $('#patientName').val(),
        phone: $('#patientPhone').val(),
        note: $('#patientNote').val(),
        Id_Specialty: $('#Id_specialty').val() || '0'
    };

    $.ajax({
        url: '/api/dang-ky-kb',
        type: 'POST',
        data: appointmentData,
        beforeSend: function() {
            Swal.fire({
                title: 'Đang xử lý...',
                text: 'Vui lòng chờ trong giây lát',
                allowOutsideClick: false,
                didOpen: () => {
                    Swal.showLoading();
                }
            });
        },
        success: function(response) {
            if (response.status) {
                Swal.fire({
                    title: 'Thành công',
                    text: response.message,
                    icon: 'success'
                }).then(() => {
                    window.location.reload();
                });
            } else {
                Swal.fire({
                    title: 'Lỗi',
                    text: response.message,
                    icon: 'error'
                });
            }
        },
        error: function() {
            Swal.fire({
                title: 'Lỗi',
                text: 'Không thể đăng ký khám bệnh. Vui lòng thử lại sau.',
                icon: 'error'
            });
        }
    });
}
function laydscauhoi()
{
    $.ajax({
        url: '/api/lay-tat-ca-tin-nhan-mau',
        type: 'GET',
        success: function (response) 
        {
              

                if(response.status) {
                    const activeQuestions = response.data.filter(question => question.status === "Active");
                    sampleQuestions.push(...activeQuestions.map(question => question.question));
                    activeQuestions.forEach(function (question) {
                        let answerHtml = question.reply;
                        answerHtml += '<div class="mt-3">';
                        if (question.buttonSamples) {
                            question.buttonSamples.forEach(function (button) {
                                answerHtml += `<a href="${button.link}" class="btn btn-primary me-2">${button.title}</a>`;
                            });
                        }
                        answerHtml += '</div>';
                        sampleAnswers[question.question] = answerHtml;
                    });
                }
        },
        error: function (err) {
            console.error('Error loading question:', err);
            Swal.fire({
                title: 'Lỗi',
                text: 'Không thể tải danh sách câu hỏi',
                icon: 'error'
            });
        }
    });
}
function toggleSpecialtyFields() {
    const specialtyFields = document.querySelectorAll('#Id_specialty, #Id_doctor');
    const isCheckup = document.getElementById('checkup').checked;
    const appointmentTimeContainer = document.getElementById('appointmentTimeContainer');

    specialtyFields.forEach(field => {
        field.closest('.form-floating').style.display = isCheckup ? 'none' : 'block';
    });

    if (isCheckup) {
        if (!appointmentTimeContainer) {
            const container = document.createElement('div');
            container.id = 'appointmentTimeContainer';
            container.className = 'form-floating mb-3';

            // Tạo giá trị ngày bắt đầu và ngày kết thúc
            const today = new Date();
            today.setHours(0, 0, 0, 0); // Đặt giờ về 0
            const todayStr = today.toLocaleDateString('en-GB').split('/').reverse().join('-'); // Format: YYYY-MM-DD

            const nextYear = new Date(today);
            nextYear.setFullYear(nextYear.getFullYear() + 1);
            const maxDate = nextYear.toLocaleDateString('en-GB').split('/').reverse().join('-'); // Format: YYYY-MM-DD

            // Tạo container input
            container.innerHTML = `
                <input type="datetime-local" 
                       class="form-control" 
                       id="appointmentTimeSK" 
                       name="appointmentTimeSK"
                       min="${todayStr}T07:00"
                       max="${maxDate}T16:00"
                       step="1800"
                       required>
                <label for="appointmentTimeSK">Chọn ngày và giờ khám</label>
                <small class="text-muted text-danger" style="color:red!important">
                    Thời gian làm việc: 7h-11h và 13h-16h
                </small>
            `;

            const parentElement = specialtyFields[0].closest('.form-floating').parentNode;
            parentElement.insertBefore(container, specialtyFields[0].closest('.form-floating'));

            // Xử lý sự kiện khi chọn thời gian
            const appointmentTimeSK = container.querySelector('#appointmentTimeSK');
            appointmentTimeSK.addEventListener('change', function () {
                const selectedDateTime = new Date(this.value);
                
                // Kiểm tra giờ hợp lệ (7h-11h và 13h-16h)
                const hours = selectedDateTime.getHours();
                const isValidTime = (hours >= 7 && hours < 11) || (hours >= 13 && hours < 16);
                
                if (!isValidTime) {
                    alert('Vui lòng chọn thời gian trong khung giờ làm việc: 7h-11h và 13h-16h');
                    this.value = ''; // Reset giá trị
                    return;
                }

                // Chuyển đổi thành định dạng yyyy-MM-ddThh:mm
                const year = selectedDateTime.getFullYear();
                const month = String(selectedDateTime.getMonth() + 1).padStart(2, '0');
                const day = String(selectedDateTime.getDate()).padStart(2, '0');
                const minutes = String(selectedDateTime.getMinutes()).padStart(2, '0');

                // Format thành yyyy-MM-ddT00:00:00Thh:mm
                const formattedDate = `${year}-${month}-${day}T00:00:00T${hours}:${minutes}`;
                console.log(`Ngày giờ đã chọn: ${formattedDate}`);
            });
        }
    } else {
        if (appointmentTimeContainer) {
            appointmentTimeContainer.remove();
        }
    }
}
function GetAllEquipment() {
    $.ajax({
        url: '/api-lay-tat-ca-may',
        type: 'GET',
        success: function(response) {
            if (response.status) {
                let carouselItems = '';
                const itemsPerSlide = 6;
                const totalItems = response.data.length;
                
                for (let i = 0; i < totalItems; i += itemsPerSlide) {
                    const isActive = i === 0 ? 'active' : '';
                    let slideHtml = '<div class="carousel-item ' + isActive + '"><div class="row g-4">';
                    
                    for (let j = i; j < Math.min(i + itemsPerSlide, totalItems); j++) {
                        const item = response.data[j];
                        slideHtml += `
                            <div class="col-lg-4 col-md-6 mb-4">
                                <div class="equipment-card fade-in">
                                    <div class="equipment-image">
                                        <img src="/Resources/${item.image_machine}" alt="${item.name_machine}" class="img-fluid">
                                        <div class="equipment-overlay">
                                            <h4>${item.name_machine}</h4>
                                            <p>${item.description_machine}</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        `;
                    }
                    
                    slideHtml += '</div></div>';
                    carouselItems += slideHtml;
                }

                $('#carasoul').html(`
                    <div id="equipmentCarousel" class="carousel slide" data-bs-ride="carousel">
                        <div class="carousel-inner">
                            ${carouselItems}
                        </div>
                        <button class="carousel-control-prev" type="button" data-bs-target="#equipmentCarousel" data-bs-slide="prev">
                            <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                            <span class="visually-hidden">Previous</span>
                        </button>
                        <button class="carousel-control-next" type="button" data-bs-target="#equipmentCarousel" data-bs-slide="next">
                            <span class="carousel-control-next-icon" aria-hidden="true"></span>
                            <span class="visually-hidden">Next</span>
                        </button>
                    </div>
                `);

                // Auto slide every 5 seconds
                $('#equipmentCarousel').carousel({
                    interval: 5000
                });
            }
        },
        error: function(error) {
            console.log('Error:', error);
        }
    });
}
