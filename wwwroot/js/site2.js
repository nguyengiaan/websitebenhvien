$(document).ready(function() {
    // Ẩn select language nếu tồn tại
    Header();
    Menu();

    GetFooter();

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
    console.log(languageCode);  
var select = document.querySelector('.goog-te-combo');
console.log(select);

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
                    <div class="header-top d-none d-md-flex align-items-center mb-2" aria-label="Emergency Contact Information">
                        <div class="emergency-contacts" role="contentinfo">
                            <p class="m-0 text-danger fw-bold mt-1" style="font-size: 18px; letter-spacing: 0.5px; text-shadow: 2px 2px 4px rgba(0,0,0,0.2);">
                                <span class="emergency-number" aria-label="Emergency Number">
                                    <i class="fas fa-ambulance fa-lg me-2 ambulance-icon" aria-hidden="true" style="color: #ff0000; transform-style: preserve-3d; animation: moveAmbulance 2s infinite;"></i>
                                    <span class="contact-badge" style="border: 2px solid #ff0000; padding: 4px 8px; border-radius: 5px; margin-right: 10px; box-shadow: 0 4px 8px rgba(0,0,0,0.1); transform: perspective(1000px) rotateX(5deg);">
                                        <span style="color: #ff0000;">Cấp cứu 24/24: <a href="tel:${res.data[0].telephone}" style="color: #ff0000 !important; text-decoration: none;">${res.data[0].telephone}</a></span>
                                    </span>
                                </span>
                                <span class="hotline-number" aria-label="Hotline Number">
                                    <i class="fas fa-phone-volume fa-lg me-2 phone-icon" aria-hidden="true" style="color: #ff0000; transform-style: preserve-3d; animation: ringPhone 1s infinite;"></i>
                                    <span class="contact-badge" style="border: 2px solid #ff0000; padding: 4px 8px; border-radius: 5px; box-shadow: 0 4px 8px rgba(0,0,0,0.1); transform: perspective(1000px) rotateX(5deg);">
                                        <span style="color: #ff0000;">Hotline 24/24: <a href="tel:02743535777" style="color: #ff0000 !important; text-decoration: none;">0274 3535 777</a></span>
                                    </span>
                                </span>
                            </p>
                            <style>
                                @keyframes moveAmbulance {
                                    0% { transform: translateX(0) rotate(0deg); }
                                    25% { transform: translateX(5px) rotate(5deg); }
                                    75% { transform: translateX(-5px) rotate(-5deg); }
                                    100% { transform: translateX(0) rotate(0deg); }
                                }
                                @keyframes ringPhone {
                                    0% { transform: rotate(-15deg); }
                                    50% { transform: rotate(15deg); }
                                    100% { transform: rotate(-15deg); }
                                }
                                .contact-badge {
                                    transition: transform 0.3s ease;
                                }
                                .contact-badge:hover {
                                    transform: perspective(1000px) rotateX(5deg) translateY(-3px) !important;
                                }
                            </style>
                        </div>
                    </div>

                    <nav class="header-navigation d-flex flex-column align-items-end" role="navigation">
                        <div class="nav-links d-none d-md-flex flex-wrap justify-content-center justify-content-md-end gap-3 mb-2">
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
                                    ${item.title}
                                </a>
                            `).join('')}

                            <div class="language-selector d-flex gap-3" role="navigation" aria-label="Language Selection">
                                <button onclick="changeLanguage('vi')" 
                                        class="language-btn" 
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
                                        class="language-btn"
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
                                        class="language-btn"
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
                            </div>
                        </div>
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
                                          color: #ffffff;">
                                    ${menu.title_menu || 'Menu'}
                                    <span style="position: absolute;
                                                bottom: 0;
                                                left: 50%;
                                                width: 0;
                                                height: 2px;
                                                background-color: #ffffff;
                                                transition: all 0.3s ease;
                                                transform: translateX(-50%);"></span>
                                </a>
                                <ul class="dropdown-menu" 
                                    aria-labelledby="menu${menu.id_menu}"
                                    style="animation: fadeIn 0.3s ease;
                                           border-radius: 3%;
                                           box-shadow: 0 8px 16px rgba(0,0,0,0.15);
                                           background: #0063EC;
                                           padding: 10px 0;
                                           min-width: 220px;
                                           border: none;
                                           left: 0;">`;

                            menu.menu.forEach(submenu => {
                                if (submenu.status) {
                                    html += `
                                        <li>
                                            <a class="dropdown-item" 
                                               href="${submenu.link_MenuChild}" 
                                               style="font-size: 13px;
                                                      transition: all 0.3s ease;
                                                      padding: 12px 25px;
                                                      color: #ffffff;
                                                      font-weight: 500;
                                                      border-left: 3px solid transparent;
                                                      margin: 2px 0;
                                                      font-family: 'Roboto', sans-serif;">
                                                ${submenu.title_MenuChild || 'Submenu'}
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
                                          color: #ffffff;">
                                    ${menu.title_menu || 'Menu'}
                                    <span style="position: absolute;
                                                bottom: 0;
                                                left: 50%;
                                                width: 0;
                                                height: 2px;
                                                background-color: #0063EC;
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
                              color: #ffffff;">
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
                        background: rgba(255, 255, 255, 0.3) !important;
                        transform: translateX(5px);
                        border-left: 4px solid #ffffff !important;
                        color: #ffffff !important;
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
                                <img src="/Resources/${response.data.logo}" alt="Logo bệnh viện" class="mb-3" style="height: 60px;">
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