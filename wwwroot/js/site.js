﻿
const connection = new signalR.HubConnectionBuilder()
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
    Header();   
    Menu();
    GetSlide();
    loadNews();
    Getallvideo();
    GetSchema();
    ListProduct();
    ListService();
    ListNews();
    ListShareCustomer();
    loadSpecialties1();
    loadSpecialties();
    GetFooter();
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
function Header() {
    $.ajax({
        url: "/Pagemain/GetTitleheader",
        type: "GET",
        success: function (res) {
            if (res.data) {
                const headerHtml = `
                             <div class="nav-links d-flex flex-wrap justify-content-center justify-content-md-end gap-3 mb-2">
                            ${res.data[0].titleheader.map((item, index) => `
                                <a href="${item.link}" 
                                   class="nav-link text-decoration-none text-dark d-flex align-items-center"
                                   role="button"
                                   aria-label="${item.title}"
                                   style="font-family: 'Montserrat', sans-serif;
                                          font-weight: bold;
                                          padding: 8px 15px;
                                          border-radius: 5px;
                                          transition: all 0.3s ease;
                                          background-color: #f8f9fa;
                                          box-shadow: 0 2px 4px rgba(0,0,0,0.1);">
                                    <img src="${index === 0 ? 'https://cdn-icons-png.flaticon.com/512/1077/1077114.png' : 
                                             index === 1 ? 'https://cdn-icons-png.flaticon.com/512/3406/3406886.png' : 
                                             'https://cdn-icons-png.flaticon.com/512/3557/3557635.png'}"
                                         alt="${item.title} icon" 
                                         class="me-2" 
                                         width="25" 
                                         height="25"
                                         loading="lazy"> 
                                  <span style="color:#000000">${item.title}</span>
                                </a>
                            `).join('')}

                            <div class="language-selector d-flex gap-3" role="navigation" aria-label="Language Selection">
                                <button onclick="changeLanguage('vi')" 
                                        class="language-btn d-block" 
                                        aria-label="Switch to Vietnamese"
                                        style="border: none; background: none; padding: 0;">
                                    <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/2/21/Flag_of_Vietnam.svg/2000px-Flag_of_Vietnam.svg.png"
                                         alt="Vietnamese Flag"
                                         class="language-flag"
                                         width="30"
                                         height="20"
                                         loading="lazy"
                                         style="cursor: pointer; transition: transform 0.2s ease; border-radius: 4px; box-shadow: 0 1px 3px rgba(0,0,0,0.1); object-fit: contain;">
                                </button>

                                <button onclick="changeLanguage('en')"
                                        class="language-btn d-block"
                                        aria-label="Switch to English"
                                        style="border: none; background: none; padding: 0;">
                                    <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/8/83/Flag_of_the_United_Kingdom_%283-5%29.svg/1280px-Flag_of_the_United_Kingdom_%283-5%29.svg.png"
                                         alt="English Flag"
                                         class="language-flag"
                                         width="30"
                                         height="20"
                                         loading="lazy"
                                         style="cursor: pointer; transition: transform 0.2s ease; border-radius: 4px; box-shadow: 0 1px 3px rgba(0,0,0,0.1); object-fit: contain;">
                                </button>

                                <button onclick="changeLanguage('zh-CN')"
                                        class="language-btn d-block"
                                        aria-label="Switch to Chinese"
                                        style="border: none; background: none; padding: 0;">
                                    <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/f/fa/Flag_of_the_People%27s_Republic_of_China.svg/2000px-Flag_of_the_People%27s_Republic_of_China.svg.png"
                                         alt="Chinese Flag"
                                         class="language-flag"
                                         width="30"
                                         height="20"
                                         loading="lazy"
                                         style="cursor: pointer; transition: transform 0.2s ease; border-radius: 4px; box-shadow: 0 1px 3px rgba(0,0,0,0.1); object-fit: contain;">
                                </button>

                                <button onclick="changeLanguage('ko')"
                                        class="language-btn d-block"
                                        aria-label="Switch to Korean"
                                        style="border: none; background: none; padding: 0;">
                                    <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/0/09/Flag_of_South_Korea.svg/2000px-Flag_of_South_Korea.svg.png"
                                         alt="Korean Flag"
                                         class="language-flag"
                                         width="30"
                                         height="20"
                                         loading="lazy"
                                         style="cursor: pointer; transition: transform 0.2s ease; border-radius: 4px; box-shadow: 0 1px 3px rgba(0,0,0,0.1); object-fit: contain;">
                                </button>

                                <button onclick="changeLanguage('ja')"
                                        class="language-btn d-block"
                                        aria-label="Switch to Japanese"
                                        style="border: none; background: none; padding: 0;">
                                    <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/9/9e/Flag_of_Japan.svg/2000px-Flag_of_Japan.svg.png"
                                         alt="Japanese Flag"
                                         class="language-flag"
                                         width="30"
                                         height="20"
                                         loading="lazy"
                                         style="cursor: pointer; transition: transform 0.2s ease; border-radius: 4px; box-shadow: 0 1px 3px rgba(0,0,0,0.1); object-fit: contain;">
                                </button>

                            </div>
                        </div>
                  

                    <nav class="header-navigation d-flex flex-column align-items-end" role="navigation">
                        <div class="header-top d-none d-md-flex align-items-center mb-3" aria-label="Emergency Contact Information">
                            <div class="emergency-contacts px-4" role="contentinfo">
                                <p class="m-0 text-danger fw-bold mt-2" style="font-size: 18px; letter-spacing: 0.5px; text-shadow: 1px 1px 3px rgba(0,0,0,0.1);">
                                    <span class="emergency-number me-4" aria-label="Emergency Number">
                                        <i class="fas fa-ambulance fa-lg me-3 ambulance-icon" aria-hidden="true" style="color: #ff0000; transform-style: preserve-3d; animation: moveAmbulance 2s infinite;"></i>
                                        <span class="contact-badge" style="border: 2px solid #ff0000; padding: 6px 15px; border-radius: 8px; margin-right: 20px; box-shadow: 0 3px 6px rgba(0,0,0,0.15); background: rgba(255,255,255,0.9);">
                                            <span style="color: #ff0000;">Cấp cứu 24/24: <a href="tel:${res.data[0].telephone}" style="color: #ff0000 !important; text-decoration: none; font-weight: 700;">${res.data[0].telephone}</a></span>
                                        </span>
                                    </span>
                                    <span class="social-icons ms-4" aria-label="Social Media Links">
                                        <a href="https://www.youtube.com/@benhvienmyphuoc3250" target="_blank" class="social-link me-4" style="text-decoration: none;">
                                            <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/0/09/YouTube_full-color_icon_%282017%29.svg/2560px-YouTube_full-color_icon_%282017%29.svg.png" 
                                                 alt="YouTube"
                                                 width="32"
                                                 height="32"
                                                 style="border-radius: 50%; transition: transform 0.3s ease; box-shadow: 0 2px 4px rgba(0,0,0,0.1);">
                                        </a>
                                        <a href="https://www.facebook.com/benhvienmyphuoc?locale=vi_VN" target="_blank" class="social-link me-4" style="text-decoration: none;">
                                            <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/0/05/Facebook_Logo_%282019%29.png/1024px-Facebook_Logo_%282019%29.png"
                                                 alt="Facebook"
                                                 width="32" 
                                                 height="32"
                                                 style="border-radius: 50%; transition: transform 0.3s ease; box-shadow: 0 2px 4px rgba(0,0,0,0.1);">
                                        </a>
                                        <a href="https://zalo.me/3218616946017588505" target="_blank" class="social-link" style="text-decoration: none;">
                                            <img src="/Images/zalo_sharelogo.png"
                                                 alt="Facebook"
                                                 width="32" 
                                                 height="32"
                                                 style="border-radius: 50%; transition: transform 0.3s ease; box-shadow: 0 2px 4px rgba(0,0,0,0.1);">
                                        </a>
                                    </span>
                                </p>
                                <style>
                                    @keyframes moveAmbulance {
                                        0% { transform: translateX(0) rotate(0deg); }
                                        25% { transform: translateX(5px) rotate(5deg); }
                                        75% { transform: translateX(-5px) rotate(-5deg); }
                                        100% { transform: translateX(0) rotate(0deg); }
                                    }
                                    .contact-badge {
                                        transition: all 0.3s ease;
                                    }
                                    .contact-badge:hover {
                                        transform: translateY(-2px);
                                        box-shadow: 0 5px 8px rgba(0,0,0,0.2);
                                    }
                                    .social-link img:hover {
                                        transform: scale(1.15);
                                        box-shadow: 0 4px 8px rgba(0,0,0,0.2);
                                    }
                                </style>
                            </div>
                        </div>
                    </nav>
                    </nav>`;

                $('#header-contact').html(headerHtml);

                // Add hover effects using JavaScript
                const languageFlags = document.querySelectorAll('.language-flag');
                languageFlags.forEach(flag => {
                    flag.addEventListener('mouseover', () => flag.style.transform = 'scale(1.1)');
                    flag.addEventListener('mouseout', () => flag.style.transform = 'scale(1)');
                });

                const navLinks = document.querySelectorAll('.nav-link');
                navLinks.forEach(link => {
                    link.addEventListener('mouseover', () => link.style.backgroundColor = '#e9ecef');
                    link.addEventListener('mouseout', () => link.style.backgroundColor = '#f8f9fa');
                });
            }
        }
    });
}
function Menu() {
    $.ajax({
        url: "/Pagemain/ListMenu",
        type: "GET", 
        success: function (res) {
            if (res.success) {
                let html = '';

                // Sắp xếp menu theo `order_menu`
                res.data.sort((a, b) => a.order_menu - b.order_menu);

                // Thêm nút tìm kiếm
         

                res.data.forEach(menu => {
                    if (menu.status) {
                        html += `<li class="nav-item px-2 ${menu.menu && menu.menu.length > 0 ? 'dropdown' : ''}" 
                                   style="transition: all 0.3s ease; border-right: 1px solid #ccc;">`;

                        if (menu.menu && menu.menu.length > 0) {
                            html += `
                                <a class="nav-link" 
                                   href="${menu.link_menu}" 
                                   id="menu${menu.id_menu}" 
                                   role="button"
                                   aria-expanded="false" 
                                   style="font-size: 14px; 
                                          font-family: 'Roboto', sans-serif; 
                                          font-weight: 600; 
                                          white-space: nowrap;
                                          transition: all 0.3s ease;
                                          position: relative;
                                          padding: 8px 15px;
                                          color: #0095d9;">
                                    ${menu.title_menu || 'Menu'}
                                    <span style="position: absolute;
                                                bottom: 0;
                                                left: 50%;
                                                width: 0;
                                                height: 2px;
                                                background-color: #0095d9;
                                                transition: all 0.3s ease;
                                                transform: translateX(-50%);"></span>
                                </a>
                                <ul class="dropdown-menu" 
                                    aria-labelledby="menu${menu.id_menu}"
                                    style="animation: fadeIn 0.3s ease;
                                           border-radius: 3%;
                                           box-shadow: 0 8px 16px rgba(0,0,0,0.15);
                                           background: #ffffff;
                                           width: 100%;
                                           padding: 10px 0;
                                           min-width: 220px;
                                           left: 0;">`;
                             
                            // Sort submenu items by order_MenuChild from high to low
                            menu.menu.sort((a, b) => a.order_menu-b.order_menu );
                            
                    
                            menu.menu.forEach(submenu => {
                                if (submenu.status) {
                                    let submenuTitle = submenu.title_MenuChild || 'Submenu';
                                    // Cắt ngắn tiêu đề nếu dài hơn 25 ký tự
                                    if (submenuTitle.length > 25) {
                                        submenuTitle = submenuTitle.substring(0, 25) + '...';
                                    }
                                    
                                    html += `
                                        <li>
                                            <a class="dropdown-item" 
                                               href="${submenu.link_MenuChild}" 
                                               title="${submenu.title_MenuChild}"
                                               style="font-size: 13px;
                                                      transition: all 0.3s ease;
                                                      padding: 12px 25px;
                                                      color: #0095d9;
                                                      font-weight: 500;
                                                      border-left: 3px solid transparent;
                                                      margin: 2px 0;
                                                      font-family: 'Roboto', sans-serif;
                                                      overflow: hidden;
                                                      text-overflow: ellipsis;
                                                      white-space: nowrap;">
                                                ${submenuTitle}
                                            </a>
                                        </li>`;
                                }
                            });

                            html += `</ul>`;
                        } else {
                            html += `
                                <a class="nav-link" 
                                   href="${menu.link_menu}" 
                                   style="font-size: 14px;
                                          font-family: 'Roboto', sans-serif;
                                          font-weight: 600;
                                          white-space: nowrap;
                                          transition: all 0.3s ease;
                                          position: relative;
                                          padding: 8px 15px;
                                          color: #0095d9;">
                                    ${menu.title_menu || 'Menu'}
                                    <span style="position: absolute;
                                                bottom: 0;
                                                left: 50%;
                                                width: 0;
                                                height: 2px;
                                                background-color: #0095d9;
                                                transition: all 0.3s ease;
                                                transform: translateX(-50%);"></span>
                                </a>`;
                        }

                        html += `</li>`;
                    }
                });
                html += `
                <li class="nav-item px-2">
                    <a class="nav-link" href="#" data-bs-toggle="modal" data-bs-target="#searchModal"
                       style="font-size: 14px;
                              font-family: 'Roboto', sans-serif;
                              font-weight: 600;
                              color: #0095d9;">
                        <i class="fas fa-search"></i>
                    </a>
                </li>`;

                // Thêm modal tìm kiếm
                const searchModal = `
                    <div class="modal fade" id="searchModal" tabindex="-1" aria-labelledby="searchModalLabel" aria-hidden="true">
                        <div class="modal-dialog modal-dialog-centered">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title" id="searchModalLabel">Tìm kiếm</h5>
                                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                </div>
                                <div class="modal-body">
                                    <div class="input-group">
                                        <input type="text" class="form-control" placeholder="Nhập từ khóa tìm kiếm..." aria-label="Search">
                                        <button class="btn btn-primary" type="button">
                                            <i class="fas fa-search"></i>
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                `;

                // Thêm CSS cho hiệu ứng hover
                const style = document.createElement('style');
                style.textContent = `
                    .nav-link:hover span {
                        width: 100% !important;
                    }
                    .dropdown-item:hover {
                        background: rgba(255, 0, 0, 0.1) !important;
                        transform: translateX(5px);
                        border-left: 4px solid red !important;
                        color: red !important;
                        font-weight: 600;
                    }
                    @keyframes fadeIn {
                        from {
                            opacity: 0;
                            transform: translateY(-10px);
                        }
                        to {
                            opacity: 1;
                            transform: translateY(0);
                        }
                    }
                    .dropdown:hover .dropdown-menu {
                        display: block;
                    }
                    .nav-item:last-child {
                        border-right: none;
                    }
                    .dropdown-menu {
                        margin-top: 0;
                        left: 0 !important;
                        background: #ffffff;
                        width: 100%;
                    }
                `;
                document.head.appendChild(style);

                // Gắn HTML đã tạo vào phần tử menu
                $('#menu').html(html);
                $('body').append(searchModal);
            }
        },
        error: function (err) {
            console.error("Lỗi khi lấy dữ liệu menu:", err);
        }
    });
}
function GetSlide() {
    $.ajax({
        url: "/Pagemain/GetSlides", 
        type: "GET",
        success: function (res) {
            if (res.success) {
                let slidesHtml = '';
                
                res.data.sort((a, b) => a.sort - b.sort);

                // Generate one slide per image
                res.data.forEach((slide, index) => {
                    if (slide.status) {
                        const isActive = index === 0 ? 'active' : '';
                        const slideHeight = window.innerWidth < 768 ? '375px' : '428px';
                        
                        slidesHtml += `
                            <div class="carousel-item ${isActive}">
                                <a href="${slide.link || '#'}" class="text-decoration-none">
                                    <div class="slide-container rounded-4 overflow-hidden position-relative" 
                                         style="height: ${slideHeight}; width: 100%;">
                                        <div class="slide-item position-relative" 
                                             style="width: 100%; height: 100%; transition: all 0.3s ease; cursor: pointer;">
                                            <img src="/Resources/${slide.slide}" 
                                                 class="w-100 h-100" 
                                                 alt="${slide.title}"
                                                 loading="eager"
                                                 decoding="sync" 
                                                 style="object-fit: cover; cursor: pointer; border-radius: 6px; 
                                                        image-rendering: -webkit-optimize-contrast;
                                                        image-rendering: crisp-edges;
                                                        -webkit-backface-visibility: hidden;
                                                        backface-visibility: hidden;
                                                        transform: translateZ(0);
                                                        -webkit-font-smoothing: subpixel-antialiased;">
                                        </div>
                                    </div>
                                </a>
                            </div>`;
                    }
                });

                $('#carouselContainer').html(slidesHtml);

                // Initialize carousel with higher quality transitions
                new bootstrap.Carousel(document.querySelector('#mainCarousel'), {
                    interval: 1500,
                    ride: 'carousel',
                    wrap: true,
                    touch: true
                });

                // Handle resize events with debouncing
                let resizeTimeout;
                window.addEventListener('resize', function() {
                    clearTimeout(resizeTimeout);
                    resizeTimeout = setTimeout(() => {
                        GetSlide();
                    }, 250);
                });

            } else {
                console.error("Không có dữ liệu slide hợp lệ.");
            }
        },
        error: function (err) {
            console.error("Lỗi khi lấy dữ liệu slide:", err);
        }
    });
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
function ListProduct() {
    $.ajax({
        url: "/Allinone/ListProduct", // API lấy dữ liệu
        type: "GET",
        success: function (res) {
            if (res.status) {
     
                // Gọi hàm hiển thị carousel với dữ liệu trả về
                displayProductCarousel(res.data);
            } else {
                console.error("Không có dữ liệu để hiển thị.");
            }
        },
        error: function (err) {
            console.error("Lỗi khi lấy dữ liệu:", err);
        }
    });
}
function displayProductCarousel(productData) {
    const carousel = document.querySelector('#productCarousel');
    carousel.innerHTML = `
        <div class="carousel-inner" id="productContainer"></div>
        <button class="carousel-control-prev" type="button" data-bs-target="#productCarousel" data-bs-slide="prev" 
                style="width: 45px; height: 45px; background: #14a1ff; border: none; border-radius: 50%; position: absolute; left: 10px; top: 50%; transform: translateY(-50%); opacity: 0.9; transition: all 0.3s ease;">
            <span class="carousel-control-prev-icon" aria-hidden="true"></span>
            <span class="visually-hidden">Previous</span>
        </button>
        <button class="carousel-control-next" type="button" data-bs-target="#productCarousel" data-bs-slide="next"
                style="width: 45px; height: 45px; background: #14a1ff; border: none; border-radius: 50%; position: absolute; right: 10px; top: 50%; transform: translateY(-50%); opacity: 0.9; transition: all 0.3s ease;">
            <span class="carousel-control-next-icon" aria-hidden="true"></span>
            <span class="visually-hidden">Next</span>
        </button>
    `;

    const productContainer = document.querySelector('#productContainer');
    
    // Số sản phẩm trên mỗi slide
    let productsPerSlide = 3; // Mặc định hiển thị 3 sản phẩm một hàng
    if (window.innerWidth < 768) {
        productsPerSlide = 1;
    }

    // Chia sản phẩm thành các nhóm
    let productChunks = [];
    for (let i = 0; i < productData.length; i += productsPerSlide) {
        productChunks.push(productData.slice(i, i + productsPerSlide));
    }

    // Tạo các slide
    productChunks.forEach((chunk, index) => {
        const slide = document.createElement('div');
        slide.className = `carousel-item ${index === 0 ? 'active' : ''}`;

        const row = document.createElement('div');
        row.className = 'row g-4 justify-content-center';

        chunk.forEach(product => {
            if (product.status) {
                const col = document.createElement('div');
                col.className = window.innerWidth < 768 ? 'col-12' : 'col-md-4';

                col.innerHTML = `
                    <div class="card h-100 shadow-sm" style="border: none; transition: transform 0.3s;">
                        <div class="position-relative overflow-hidden" style="height: 250px;">
                            <img src="/Resources/${product.url}" 
                                 class="card-img-top" 
                                 alt="${product.title}"
                                 style="height: 100%; width: 100%; object-fit: cover; transition: transform 0.3s;">
                            <div class="position-absolute top-0 start-0 w-100 h-100 d-flex align-items-center justify-content-center"
                                 style="background: rgba(0,0,0,0.5); opacity: 0; transition: opacity 0.3s;">
                                <h5 class="text-white text-center px-3">${product.title}</h5>
                            </div>
                        </div>
                        <div class="card-body text-center">
                            <h5 class="card-title text-truncate mb-0">${product.title}</h5>
                            <a href="${product.alias_url}" class="btn btn-outline-primary mt-3">Xem chi tiết</a>
                        </div>
                    </div>
                `;

                // Thêm hiệu ứng hover
                const card = col.querySelector('.card');
                card.addEventListener('mouseenter', () => {
                    card.style.transform = 'translateY(-5px)';
                    card.querySelector('.position-absolute').style.opacity = '1';
                });
                
                card.addEventListener('mouseleave', () => {
                    card.style.transform = 'translateY(0)';
                    card.querySelector('.position-absolute').style.opacity = '0';
                });

                row.appendChild(col);
            }
        });

        slide.appendChild(row);
        productContainer.appendChild(slide);
    });

    // Khởi tạo carousel
    new bootstrap.Carousel(carousel, {
        interval: 5000,
        wrap: true
    });

    // Xử lý responsive
    window.addEventListener('resize', () => {
        displayProductCarousel(productData);
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
function ListNews() {
    $.ajax({
        url: "/Allinone/NewsList",
        type: "GET",
        success: function(response) {
            if (response.status) {
             
                displayNewsCarousel(response.data);
            }
        },
        error: function(error) {
            console.log("Error:", error);
        }
    });
}
function displayNewsCarousel(productData) {
    const carousel = document.getElementById('newsCarousel');
    if (!carousel) return;

    const newsContainer = document.createElement('div');
    newsContainer.className = 'carousel-inner';

    // Cố định số lượng item trên mỗi slide là 4
    const itemsPerSlide = 4;
    const slides = [];
    let currentSlide = [];

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

    slides.forEach((slideItems, slideIndex) => {
        const slide = document.createElement('div');
        slide.className = `carousel-item ${slideIndex === 0 ? 'active' : ''}`;

        const row = document.createElement('div');
        row.className = 'row g-4 justify-content-center';

        slideItems.forEach(item => {
            if (item.status) {
                const col = document.createElement('div');
                col.className = 'col-lg-3 col-md-6 col-12';

                const newsItem = document.createElement('div');
                newsItem.className = 'card h-100 border-0 shadow-sm rounded-lg overflow-hidden';
                newsItem.style.cssText = `
                    transition: all 0.3s ease;
                    background: #ffffff;
                `;

                // Hover effects
                newsItem.addEventListener('mouseenter', () => {
                    newsItem.style.transform = 'translateY(-5px)';
                    newsItem.style.boxShadow = '0 8px 16px rgba(0,0,0,0.1)';
                });
                newsItem.addEventListener('mouseleave', () => {
                    newsItem.style.transform = 'translateY(0)';
                    newsItem.style.boxShadow = '0 4px 8px rgba(0,0,0,0.05)';
                });

                // Image container
                const imgContainer = document.createElement('div');
                imgContainer.className = 'position-relative overflow-hidden';
                imgContainer.style.height = '220px';

                const img = document.createElement('img');
                img.src = `/Resources/${item.url}`;
                img.className = 'w-100 h-100';
                img.alt = item.title;
                img.style.cssText = `
                    object-fit: cover;
                    transition: transform 0.3s ease;
                `;

                // News label
                const newsLabel = document.createElement('div');
                newsLabel.className = 'position-absolute badge bg-primary';
                newsLabel.style.cssText = `
                    top: 15px;
                    right: 15px;
                    padding: 8px 12px;
                    font-size: 0.85rem;
                    font-weight: 500;
                    border-radius: 20px;
                    z-index: 2;
                `;
                newsLabel.textContent = 'Tin tức & Sự kiện';

                // Check if post is from today and add NEW label if true
                const today = new Date();
                const postDate = new Date(item.createat);
                if (today.toDateString() === postDate.toDateString()) {
                    const newLabel = document.createElement('div');
                    newLabel.className = 'position-absolute badge bg-danger';
                    newLabel.style.cssText = `
                        top: 15px;
                        left: 15px;
                        padding: 8px 12px;
                        font-size: 0.85rem;
                        font-weight: 500;
                        border-radius: 20px;
                        z-index: 2;
                    `;
                    newLabel.textContent = 'NEW';
                    imgContainer.appendChild(newLabel);
                }

                imgContainer.appendChild(img);
                imgContainer.appendChild(newsLabel);

                const cardBody = document.createElement('div');
                cardBody.className = 'card-body p-4';

                // Date
                const date = document.createElement('p');
                date.className = 'text-primary mb-2';
                date.style.fontSize = '0.9rem';
                date.innerHTML = `<i class="far fa-calendar-alt me-2"></i>${new Date(item.createat).toLocaleDateString('vi-VN')}`;

                // Title
                const title = document.createElement('h5');
                title.className = 'card-title fw-bold mb-3';
                title.style.cssText = `
                    font-size: 1.1rem;
                    line-height: 1.5;
                    display: -webkit-box;
                    -webkit-line-clamp: 2;
                    -webkit-box-orient: vertical;
                    overflow: hidden;
                `;
                title.textContent = item.title;

                // Description
                const description = document.createElement('p');
                description.className = 'card-text text-muted mb-4';
                description.style.cssText = `
                    font-size: 0.95rem;
                    display: -webkit-box;
                    -webkit-line-clamp: 3;
                    -webkit-box-orient: vertical;
                    overflow: hidden;
                `;
                description.textContent = item.description;

                // Read more button
                const link = document.createElement('a');
                link.href = item.alias_url;
                link.className = 'btn btn-outline-primary rounded-pill px-4';
                link.innerHTML = '<i class="fas fa-arrow-right me-2"></i>Xem thêm';
                link.style.cssText = `
                    transition: all 0.3s ease;
                    font-size: 0.9rem;
                `;

                cardBody.appendChild(date);
                cardBody.appendChild(title);
                cardBody.appendChild(description);
                cardBody.appendChild(link);

                newsItem.appendChild(imgContainer);
                newsItem.appendChild(cardBody);
                col.appendChild(newsItem);
                row.appendChild(col);
            }
        });

        slide.appendChild(row);
        newsContainer.appendChild(slide);
    });

    carousel.innerHTML = '';
    carousel.appendChild(newsContainer);

    // Carousel controls
    carousel.innerHTML += `
        <button class="carousel-control-prev" type="button" data-bs-target="#newsCarousel" data-bs-slide="prev" 
                style="width: 40px; height: 40px; background: rgba(20, 161, 255, 0.8); border-radius: 50%; 
                position: absolute; top: 50%; transform: translateY(-50%); left: -20px;
                border: 2px solid #fff; box-shadow: 0 2px 5px rgba(0,0,0,0.1); transition: all 0.3s ease;">
            <span class="carousel-control-prev-icon" aria-hidden="true" style="width: 20px; height: 20px;"></span>
            <span class="visually-hidden">Previous</span>
        </button>
        <button class="carousel-control-next" type="button" data-bs-target="#newsCarousel" data-bs-slide="next"
                style="width: 40px; height: 40px; background: rgba(20, 161, 255, 0.8); border-radius: 50%;
                position: absolute; top: 50%; transform: translateY(-50%); right: -20px;
                border: 2px solid #fff; box-shadow: 0 2px 5px rgba(0,0,0,0.1); transition: all 0.3s ease;">
            <span class="carousel-control-next-icon" aria-hidden="true" style="width: 20px; height: 20px;"></span>
            <span class="visually-hidden">Next</span>
        </button>
    `;

    new bootstrap.Carousel(carousel, {
        interval: 5000,
        wrap: true
    });
}
function ListShareCustomer() {
    $.ajax({
        url: "/Allinone/ListShareCustomer",
        type: "GET",
        success: function(response) {
            if (response.status) {
          
                displayShareCustomerCarousel(response.data);
            }
        },
        error: function(error) {
            console.log("Error:", error);
        }
    })
}
function displayShareCustomerCarousel(productData) {
    const carousel = document.getElementById('shareCustomerCarousel');
    if (!carousel) return;

    const shareCustomerContainer = document.createElement('div');
    shareCustomerContainer.className = 'carousel-inner';

    // Số lượng items trên mỗi slide
    const itemsPerSlide = window.innerWidth < 768 ? 2 : 
                         window.innerWidth < 992 ? 3 : 4;

    // Chia items thành các slides
    const slides = [];
    for (let i = 0; i < productData.length; i += itemsPerSlide) {
        slides.push(productData.slice(i, i + itemsPerSlide));
    }

    // Tạo slides
    slides.forEach((slideItems, slideIndex) => {
        const slide = document.createElement('div');
        slide.className = `carousel-item ${slideIndex === 0 ? 'active' : ''}`;

        const row = document.createElement('div');
        row.className = 'row g-4 justify-content-center align-items-center';

        slideItems.forEach(item => {
            const col = document.createElement('div');
            col.className = `col-${12/itemsPerSlide}`;

            const imgContainer = document.createElement('div');
            imgContainer.className = 'partner-logo-container';
            imgContainer.style.cssText = `
                padding: 20px;
                background: #ffffff;
                border-radius: 12px;
                box-shadow: 0 3px 12px rgba(0,0,0,0.08);
                transition: all 0.3s ease;
                text-align: center;
                display: flex;
                align-items: center;
                justify-content: center;
                height: 150px;
                margin: 10px;
                border: 1px solid #f0f0f0;
            `;

            const img = document.createElement('img');
            img.src = `/Resources/${item.customerName}`;
            img.alt = item.customerName;
            img.className = 'img-fluid';
            img.style.cssText = `
                max-height: 80px;
                max-width: 80%;
                width: auto;
                object-fit: contain;
                opacity: 0.7;
                transition: all 0.3s ease;
                background-color: #ffffff;
            `;

            // Hover effects
            imgContainer.addEventListener('mouseenter', () => {
                imgContainer.style.transform = 'translateY(-5px)';
                imgContainer.style.boxShadow = '0 8px 20px rgba(0,0,0,0.12)';
                img.style.filter = 'grayscale(0%)';
                img.style.opacity = '1';
            });

            imgContainer.addEventListener('mouseleave', () => {
                imgContainer.style.transform = 'translateY(0)';
                imgContainer.style.boxShadow = '0 3px 12px rgba(0,0,0,0.08)';
                img.style.filter = 'grayscale(100%)';
                img.style.opacity = '0.7';
            });

            imgContainer.appendChild(img);
            col.appendChild(imgContainer);
            row.appendChild(col);
        });

        slide.appendChild(row);
        shareCustomerContainer.appendChild(slide);
    });

    // Thêm controls
    carousel.innerHTML = '';
    carousel.appendChild(shareCustomerContainer);
    
    const controls = `
        <button class="carousel-control-prev" type="button" data-bs-target="#shareCustomerCarousel" data-bs-slide="prev">
            <span class="carousel-control-prev-icon" aria-hidden="true"></span>
        </button>
        <button class="carousel-control-next" type="button" data-bs-target="#shareCustomerCarousel" data-bs-slide="next">
            <span class="carousel-control-next-icon" aria-hidden="true"></span>
        </button>
    `;
    
    carousel.innerHTML += controls;

    // Khởi tạo carousel
    new bootstrap.Carousel(carousel, {
        interval: 3000,
        wrap: true,
        touch: true
    });

    // Xử lý resize
    window.addEventListener('resize', () => {
        displayShareCustomerCarousel(productData);
    });
}
// lấy footer
function GetFooter() {
    $.ajax({
        url: "/PageMain/GetFooter", 
        type: "GET",
        success: function(response) {
            if (response.data) {
                const footer = `
                    <div class="container-fluid px-4">
                        <div class="row mx-0">
                            <!-- Logo và thông tin liên hệ -->
                            <div class="col-lg-4 mb-4">
                                <img src="/Images/Logo-rm.png" alt="Logo bệnh viện" class="mb-3" style="height: 60px;">
                                <ul class="list-unstyled text-muted">
                                    <li class="mb-2">
                                        <i class="fas fa-map-marker-alt me-2 text-primary"></i>
                                        ${response.data.address}
                                    </li>
                                    <li class="mb-2">
                                        <i class="fas fa-phone me-2 text-primary"></i>
                                        ${response.data.telephone}
                                    </li>
                                    <li>
                                        <i class="fas fa-envelope me-2 text-primary"></i>
                                        ${response.data.email}
                                    </li>
                                </ul>
                            </div>

                            <!-- Về chúng tôi -->
                            <div class="col-lg-2 mb-4">
                                <h5 class="fw-bold text-uppercase mb-3">Thông tin chung</h5>
                                <ul class="list-unstyled">
                                    <li class="mb-2">
                                        <a href="${response.data.linkgt}" class="text-muted text-decoration-none hover-primary">
                                            <i class="fas fa-angle-right me-2"></i>Giới thiệu
                                        </a>
                                    </li>
                                    <li class="mb-2">
                                        <a href="${response.data.linktn}" class="text-muted text-decoration-none hover-primary">
                                            <i class="fas fa-angle-right me-2"></i>Tầm nhìn & Sứ mệnh
                                        </a>
                                    </li>
                                    <li class="mb-2">
                                        <a href="${response.data.dnbs}" class="text-muted text-decoration-none hover-primary">
                                            <i class="fas fa-angle-right me-2"></i>Đội ngũ bác sĩ
                                        </a>
                                    </li>
                                    <li class="mb-2">
                                        <a href="${response.data.linktd}" class="text-muted text-decoration-none hover-primary">
                                            <i class="fas fa-angle-right me-2"></i>Tuyển dụng
                                        </a>
                                    </li>
                                </ul>
                            </div>

                            <!-- Dịch vụ -->
                            <div class="col-lg-2 mb-4">
                                <h5 class="fw-bold text-uppercase mb-3">Dịch vụ</h5>
                                <ul class="list-unstyled">
                                    <li class="mb-2">
                                        <a href="${response.data.linkkb}" class="text-muted text-decoration-none hover-primary">
                                            <i class="fas fa-angle-right me-2"></i>Khám bệnh
                                        </a>
                                    </li>
                                    <li class="mb-2">
                                        <a href="${response.data.linkxn}" class="text-muted text-decoration-none hover-primary">
                                            <i class="fas fa-angle-right me-2"></i>Xét nghiệm
                                        </a>
                                    </li>
                                    <li class="mb-2">
                                        <a href="${response.data.linkcdha}" class="text-muted text-decoration-none hover-primary">
                                            <i class="fas fa-angle-right me-2"></i>Chẩn đoán hình ảnh
                                        </a>
                                    </li>
                                    <li class="mb-2">
                                        <a href="${response.data.linknt}" class="text-muted text-decoration-none hover-primary">
                                            <i class="fas fa-angle-right me-2"></i>Nội trú
                                        </a>
                                    </li>
                                    <li class="mb-2">
                                        <a href="${response.data.linkdn}" class="text-muted text-decoration-none hover-primary">
                                            <i class="fas fa-angle-right me-2"></i>Khám doanh nghiệp
                                        </a>
                                    </li>
                                    <li class="mt-3">
                                        <a class="btn btn-outline-primary btn-sm">
                                            <i class="fas fa-calendar-alt me-2"></i>Lịch khám bệnh
                                        </a>
                                    </li>
                                </ul>
                            </div>

                            <!-- Đăng ký nhận tin -->
                            <div class="col-lg-4 mb-4">
                                <h5 class="fw-bold text-uppercase mb-3">Đăng ký nhận tin</h5>
                                <p class="text-muted">Nhận thông tin mới nhất về sức khỏe và các chương trình ưu đãi.</p>
                                <div class="input-group mb-3">
                                    <input type="email" class="form-control" placeholder="Nhập email của bạn">
                                    <button class="btn btn-primary" type="button">Đăng ký</button>
                                </div>
                                <div class="social-links mt-3">
                                    <a href="${response.data.linkface}" class="text-primary me-3"><i class="fab fa-facebook-f fa-lg"></i></a>
                                    <a href="${response.data.linktwiter}" class="text-info me-3"><i class="fab fa-twitter fa-lg"></i></a>
                                    <a href="${response.data.linkyoutube}" class="text-danger"><i class="fab fa-youtube fa-lg"></i></a>
                                </div>
                            </div>
                        </div>
                        <!-- Bản quyền -->
                        <div class="text-center text-muted mt-4">
                            <small>&copy; 2024 Bệnh viện Đa khoa Mỹ Phước. All rights reserved.</small>
                        </div>
                    </div>`;
                $('footer').html(footer);
            }
        }
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

// Các câu hỏi mẫu




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
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(
            (position) => {
            
               
            },
            (error) => {
                console.error("Error obtaining location:", error);
            }
        );
    } else {
        console.log("Geolocation is not supported by this browser.");
    }
  
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
function loadNews() 
{
    $.ajax({
        url: "/Allinone/NewsList",
        type: "GET",
        success: function(response) {
            if (response.status) {
                    console.log(response.data);
               var html = '';
               response.data.forEach(function(news) {
                if (news.status) {
                    // Check if news is within last week
                    const oneWeekAgo = new Date();
                    oneWeekAgo.setDate(oneWeekAgo.getDate() - 7);
                    const newsDate = new Date(news.created_at);
                    const isNew = newsDate > oneWeekAgo;

                    html += `<a href="${news.alias_url}" class="list-group-item list-group-item-action p-3 border-bottom hover-effect rounded-4 position-relative" style="transition: all 0.3s ease; border-left: 4px solid #0095d9;">
                        <div class="news-item d-flex">
                            <div class="news-image me-3" style="width: 100px; min-width: 100px;">
                                <img src="/Resources/${news.url}" alt="${news.title}" class="img-fluid rounded" style="width: 100px; height: 80px; object-fit: cover;">
                            </div>
                            <div class="news-content">
                                <h6 class="mb-2 fw-bold" style="color: #0095d9">${news.title}</h6>
                                ${isNew ? '<span class="badge position-absolute top-0 end-0 m-2 fw-bold" style="background-color: #0095d9">MỚI</span>' : ''}
                                <div class="d-flex align-items-center mb-2" style="color: #000000">
                                    <i class="far fa-clock me-2"></i>
                                    <small>${news.createat.split('T')[0]}</small>
                                </div>
                                <p class="mb-0" style="color: #000000; font-size: 0.95rem; line-height: 1.5;">
                                    ${news.title}...
                                </p>
                            </div>
                        </div>
                    </a>`;
                }
               });
               $('.list-group').html(html);

               // Add hover effect styles
               $('<style>')
                    .text(`
                        .list-group-item:hover {
                            transform: translateY(-3px);
                            box-shadow: 0 5px 15px rgba(0, 149, 217, 0.3);
                            border-color: #0095d9 !important;
                        }
                    `)
                    .appendTo('head');
            }
        },
        error: function(error) {
            console.log("Error:", error);
        }
    });
    
}
function loadSpecialties1() {
    $.ajax({
        url: '/api/chuyen-khoa-catogery',
        type: 'GET',
        success: function(response) {
            if(response.data) {
                const gradients = [
                    'linear-gradient(135deg, #0095d9 0%, #006699 100%)',
                    'linear-gradient(135deg, #00b3ff 0%, #0095d9 100%)',
                    'linear-gradient(135deg, #33ccff 0%, #0095d9 100%)',
                    'linear-gradient(135deg, #66e0ff 0%, #0095d9 100%)', 
                    'linear-gradient(135deg, #99ebff 0%, #0095d9 100%)',
                    'linear-gradient(135deg, #4158D0 0%, #C850C0 46%, #FFCC70 100%)',
                    'linear-gradient(135deg, #0093E9 0%, #80D0C7 100%)',
                    'linear-gradient(135deg, #8EC5FC 0%, #E0C3FC 100%)',
                    'linear-gradient(135deg, #D9AFD9 0%, #97D9E1 100%)',
                    'linear-gradient(135deg, #00B4DB 0%, #0083B0 100%)',
                    'linear-gradient(135deg, #1CB5E0 0%, #000851 100%)',
                    'linear-gradient(135deg, #2193b0 0%, #6dd5ed 100%)',
                    'linear-gradient(135deg, #396afc 0%, #2948ff 100%)',
                    'linear-gradient(135deg, #11998e 0%, #38ef7d 100%)',
                    'linear-gradient(135deg, #FC466B 0%, #3F5EFB 100%)'
                ];

                let html = `
                <div id="specialtyCarousel" class="carousel slide" data-bs-ride="carousel">
                    <div class="carousel-inner">`;

                // Desktop: 4 items per slide, Mobile: 1 item per slide
                const itemsPerSlide = window.innerWidth < 768 ? 1 : 4;
                
                for(let i = 0; i < response.data.length; i += itemsPerSlide) {
                    const isActive = i === 0 ? 'active' : '';
                    html += `<div class="carousel-item ${isActive}">
                            <div class="row row-cols-1 row-cols-md-4">`;
                    
                    // Add items for this slide
                    for(let j = i; j < Math.min(i + itemsPerSlide, response.data.length); j++) {
                        const specialty = response.data[j];
                        const gradientIndex = j % gradients.length;
                        const gradient = gradients[gradientIndex];
                        const textColorClass = gradientIndex === 1 ? 'text-success' : 
                                             gradientIndex === 2 ? 'text-warning' :
                                             gradientIndex === 3 ? 'text-info' :
                                             gradientIndex === 4 ? 'text-danger' : 'text-primary';

                        html += `
                            <div class="col ${itemsPerSlide === 1 ? 'col-12' : 'mb-4 mb-md-0'}">
                                <div class="card border-0 text-center h-100 text-white rounded-4" 
                                     style="background: ${gradient}; animation: float 3s ease-in-out infinite ${j * 0.5}s;">
                                    <div class="card-body" style="width: 100%; height: 300px;">
                                        <div class="specialty-image-container mb-3">
                                            <img src="/Resources/${specialty.thumnail}" 
                                                 alt="${specialty.title}" 
                                                 class="rounded-circle shadow" 
                                                 style="width: 120px; height: 120px; object-fit: cover;">
                                        </div>
                                        <h5 class="card-title fw-bold">${specialty.title}</h5>
                                     
                                        <a href="${specialty.alias_url}" class="btn btn-light  fw-bold shadow" 
                                           style="border-radius: 25px; box-shadow: 0 4px 6px rgba(0, 0, 0, 0.2);">
                                            Xem chi tiết
                                        </a>
                                    </div>
                                </div>
                            </div>`;
                    }
                    html += `</div></div>`;
                }

                html += `
                    </div>
                    <button class="carousel-control-prev" type="button" data-bs-target="#specialtyCarousel" data-bs-slide="prev" 
                            style="width: 30px; height: 30px; background: rgba(0,0,0,0.5); border-radius: 50%; margin: auto 10px;">
                        <span class="carousel-control-prev-icon" aria-hidden="true" style="width: 15px; height: 15px;"></span>
                        <span class="visually-hidden">Previous</span>
                    </button>
                    <button class="carousel-control-next" type="button" data-bs-target="#specialtyCarousel" data-bs-slide="next"
                            style="width: 30px; height: 30px; background: rgba(0,0,0,0.5); border-radius: 50%; margin: auto 10px;">
                        <span class="carousel-control-next-icon" aria-hidden="true" style="width: 15px; height: 15px;"></span>
                        <span class="visually-hidden">Next</span>
                    </button>
                </div>`;

                $('#specialtyCarousel').html(html);
            }
        },
        error: function(error) {
            console.error('Error loading specialties:', error);
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
function Getallvideo() {
    $.ajax({
        url: '/api-lay-tat-ca-video',
        type: 'GET',
        success: function(response) {
            if (response.status) {
                let html = '';
                response.data.forEach((item, index) => {
                    html += `
                        <div class="carousel-item ${index === 0 ? 'active' : ''}">
                            <div class="video-wrapper position-relative">
                                <div class="ratio ratio-16x9 rounded-bottom-4 overflow-hidden shadow-lg hover-scale"
                                     style="transition: transform 0.3s ease; height: 334px;">
                                <video class="w-100 h-100" controls>
                                    <source src="/Resources/${item.link_video}" type="video/mp4">
                                    Your browser does not support the video tag.
                                </video>
                                </div>
                            </div>
                        </div>
                    `;
                });

                $('#videoCarousel').html(`
                    <div class="carousel-inner">
                        ${html}
                    </div>
                    <button class="carousel-control-prev" type="button" data-bs-target="#videoCarousel" data-bs-slide="prev" 
                            style="width: 30px; height: 30px; background: rgba(0,0,0,0.5); border-radius: 50%; top: 50%; transform: translateY(-50%); left: 10px;">
                        <span class="carousel-control-prev-icon" aria-hidden="true" style="width: 15px; height: 15px;"></span>
                        <span class="visually-hidden">Previous</span>
                    </button>
                    <button class="carousel-control-next" type="button" data-bs-target="#videoCarousel" data-bs-slide="next"
                            style="width: 30px; height: 30px; background: rgba(0,0,0,0.5); border-radius: 50%; top: 50%; transform: translateY(-50%); right: 10px;">
                        <span class="carousel-control-next-icon" aria-hidden="true" style="width: 15px; height: 15px;"></span>
                        <span class="visually-hidden">Next</span>
                    </button>
                `);
            }
        },
        error: function(error) {
            console.log('Error:', error);
        }
    });
}