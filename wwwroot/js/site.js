$(document).ready(function() {
    // Ẩn select language nếu tồn tại
    Header();
    Menu();
    GetSlide();
    GetSchema();
    ListProduct();
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
    console.log("ok");  
    $.ajax({
        url: "/Pagemain/GetTitleheader",
        type: "GET",
        success: function (res) {

            if ( res.data)
            {
                var html=` <div class="d-none d-md-flex align-items-center mb-2">
                
                            <img src="/image/Header-logo.png" alt="Emergency" class="me-2" style="height: 35px;">
                            <p class="m-0 text-danger fw-bold mt-1">
                                <i class="fas fa-ambulance fa-lg me-2"></i>Cấp cứu 24/7:${res.data[0].telephone}
                            </p>
                        </div>
                        
                        <!-- Navigation Links -->
                        <div class="d-none d-md-flex flex-wrap justify-content-center justify-content-md-end gap-3">
                            ${res.data[0].titleheader.map((item, index) => `
                            <a href="${item.link}" class="text-decoration-none text-dark" style="font-family: 'Montserrat', sans-serif; font-weight: bold;">
                                <img src="${index === 0 ? 'https://cdn-icons-png.flaticon.com/512/1077/1077114.png' : 
                                         index === 1 ? 'https://cdn-icons-png.flaticon.com/512/3406/3406886.png' : 
                                         'https://cdn-icons-png.flaticon.com/512/3557/3557635.png'}" 
                                     alt="Emergency" class="me-2" style="height: 25px;"> ${item.title}
                            </a>
                            `).join('')}
                        </div>`
                $('#header-contact').html(html);
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

                res.data.forEach(menu => {
                    if (menu.status) {
                        // Kiểm tra xem menu có submenu không
                        html += `<li class="nav-item px-2 ${menu.menu && menu.menu.length > 0 ? 'dropdown' : ''}" 
                                   style="transition: all 0.3s ease;">`;

                        if (menu.menu && menu.menu.length > 0) {
                            // Menu có submenu
                            html += `
                                <a class="nav-link text-white" 
                                   href="${menu.link_menu}" 
                                   id="menu${menu.id_menu}" 
                                   role="button"
                                   aria-expanded="false" 
                                   style="font-size: 16px; 
                                          font-family: 'Montserrat', sans-serif; 
                                          font-weight: bold; 
                                          white-space: nowrap;
                                          transition: all 0.3s ease;
                                          position: relative;
                                          padding: 8px 15px;">
                                    ${menu.title_menu || 'Menu'}
                                    <span style="position: absolute;
                                                bottom: 0;
                                                left: 50%;
                                                width: 0;
                                                height: 2px;
                                                background-color: #fff;
                                                transition: all 0.3s ease;
                                                transform: translateX(-50%);"></span>
                                </a>
                                <ul class="dropdown-menu" 
                                    aria-labelledby="menu${menu.id_menu}"
                                    style="animation: fadeIn 0.3s ease;
                                           border-radius: 8px;
                                           box-shadow: 0 4px 6px rgba(0,0,0,0.1);">`;

                            // Thêm các submenu
                            menu.menu.forEach(submenu => {
                                if (submenu.status) {
                                    html += `
                                        <li>
                                            <a class="dropdown-item" 
                                               href="${submenu.link_MenuChild}" 
                                               style="font-size: 16px;
                                                      transition: all 0.3s ease;
                                                      padding: 10px 20px;">
                                                ${submenu.title_MenuChild || 'Submenu'}
                                            </a>
                                        </li>`;
                                }
                            });

                            html += `</ul>`;
                        } else {
                            // Menu không có submenu
                            html += `
                                <a class="nav-link text-white" 
                                   href="${menu.link_menu}" 
                                   style="font-size: 16px;
                                          font-family: 'Montserrat', sans-serif;
                                          font-weight: bold;
                                          white-space: nowrap;
                                          transition: all 0.3s ease;
                                          position: relative;
                                          padding: 8px 15px;">
                                    ${menu.title_menu || 'Menu'}
                                    <span style="position: absolute;
                                                bottom: 0;
                                                left: 50%;
                                                width: 0;
                                                height: 2px;
                                                background-color: #fff;
                                                transition: all 0.3s ease;
                                                transform: translateX(-50%);"></span>
                                </a>`;
                        }

                        html += `</li>`;
                    }
                });

                // Thêm lựa chọn ngôn ngữ cuối menu
                html += `
                    <li class="nav-item px-2">
                        <div class="d-flex align-items-center">
                            <div class="language-selector d-flex gap-2">
                                <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/2/21/Flag_of_Vietnam.svg/2000px-Flag_of_Vietnam.svg.png" 
                                     alt="Vietnamese" 
                                     class="language-flag" 
                                     style="width: 30px; height: 20px; cursor: pointer; transition: transform 0.3s ease;"
                                     onmouseover="this.style.transform='scale(1.1)'"
                                     onmouseout="this.style.transform='scale(1)'"
                                     onclick="changeLanguage('vi')" />
                                
                                <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/8/83/Flag_of_the_United_Kingdom_%283-5%29.svg/1280px-Flag_of_the_United_Kingdom_%283-5%29.svg.png"
                                     alt="English"
                                     class="language-flag"
                                     style="width: 30px; height: 20px; cursor: pointer; transition: transform 0.3s ease;"
                                     onmouseover="this.style.transform='scale(1.1)'"
                                     onmouseout="this.style.transform='scale(1)'"
                                     onclick="changeLanguage('en')" />
                                     
                                <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/f/fa/Flag_of_the_People%27s_Republic_of_China.svg/2000px-Flag_of_the_People%27s_Republic_of_China.svg.png"
                                     alt="Chinese"
                                     class="language-flag"
                                     style="width: 30px; height: 20px; cursor: pointer; transition: transform 0.3s ease;"
                                     onmouseover="this.style.transform='scale(1.1)'"
                                     onmouseout="this.style.transform='scale(1)'"
                                     onclick="changeLanguage('zh-CN')" />
                            </div>
                 
                        </div>
                    </li>`;

                // Thêm CSS cho hiệu ứng hover
                const style = document.createElement('style');
                style.textContent = `
                    .nav-link:hover span {
                        width: 100% !important;
                    }
                    .dropdown-item:hover {
                        background-color: #f8f9fa;
                        transform: translateX(5px);
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
                `;
                document.head.appendChild(style);

                // Gắn HTML đã tạo vào phần tử menu
                $('#menu').html(html);
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
                // Xử lý danh sách slides
                let indicatorsHtml = '';
                let slidesHtml = '';

                res.data.sort((a, b) => a.sort - b.sort); // Sắp xếp theo `sort`

                res.data.forEach((slide, index) => {
                    if (slide.status) {
                        // Tạo các nút chỉ mục
                        indicatorsHtml += `
                            <button type="button" data-bs-target="#carouselExampleIndicators" 
                                    data-bs-slide-to="${index}" 
                                    class="${index === 0 ? 'active' : ''}" 
                                    aria-current="${index === 0 ? 'true' : 'false'}" 
                                    aria-label="Slide ${index + 1}">
                            </button>`;

                        // Tạo các slide
                        slidesHtml += `
                            <div class="carousel-item ${index === 0 ? 'active' : ''}">
                                <a href="${slide.link}" target="_blank">
                                    <img src="/Resources/${slide.slide}" 
                                         class="d-block w-100 img-fluid" 
                                         alt="${slide.title}" 
                                         style="width: 100%; height: auto; min-height: 300px; max-height: 80vh; object-fit: cover;">
                                </a>
                                <div class="carousel-caption d-none d-md-block">
                                    <h5>${slide.title}</h5>
                                </div>
                            </div>`;
                    }
                });

                // Gắn HTML vào carousel
                const html = `
                    <div id="carouselExampleIndicators" class="carousel slide" data-bs-ride="carousel">
                        <div class="carousel-indicators">
                            ${indicatorsHtml}
                        </div>
                        <div class="carousel-inner">
                            ${slidesHtml}
                        </div>
                        <button class="carousel-control-prev" type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide="prev">
                            <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                            <span class="visually-hidden">Previous</span>
                        </button>
                        <button class="carousel-control-next" type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide="next">
                            <span class="carousel-control-next-icon" aria-hidden="true"></span>
                            <span class="visually-hidden">Next</span>
                        </button>
                    </div>`;

                // Gắn HTML vào phần tử chứa slider
                $('#carouselExampleIndicators').html(html);
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
            console.log(res);
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
                console.log(res.data);
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
        <button class="carousel-control-prev" type="button" data-bs-target="#productCarousel" data-bs-slide="prev">
            <span class="carousel-control-prev-icon" aria-hidden="true"></span>
            <span class="visually-hidden">Previous</span>
        </button>
        <button class="carousel-control-next" type="button" data-bs-target="#productCarousel" data-bs-slide="next">
            <span class="carousel-control-next-icon" aria-hidden="true"></span>
            <span class="visually-hidden">Next</span>
        </button>
    `;

    const productContainer = document.querySelector('#productContainer');
    
    // Điều chỉnh số sản phẩm trên mỗi slide theo kích thước màn hình
    let productsPerSlide = 4;
    if (window.innerWidth < 768) { // Mobile
        productsPerSlide = 1;
    } else if (window.innerWidth < 992) { // Tablet
        productsPerSlide = 2;
    }

    // Group products into chunks based on screen size
    let productChunks = [];
    for (let i = 0; i < productData.length; i += productsPerSlide) {
        productChunks.push(productData.slice(i, i + productsPerSlide));
    }

    // Create carousel slides
    productChunks.forEach((chunk, index) => {
        const slide = document.createElement('div');
        slide.className = index === 0 ? 'carousel-item active' : 'carousel-item';

        const row = document.createElement('div');
        row.className = 'row g-2 justify-content-center';

        chunk.forEach(product => {
            if (product.status) {
                const col = document.createElement('div');
                // Điều chỉnh class của col theo kích thước màn hình
                if (window.innerWidth < 768) {
                    col.className = 'col-12 mb-3'; // Mobile: 1 sản phẩm/slide
                } else if (window.innerWidth < 992) {
                    col.className = 'col-6 mb-3'; // Tablet: 2 sản phẩm/slide
                } else {
                    col.className = 'col-md-3 mb-3'; // Desktop: 4 sản phẩm/slide
                }

                const equipmentItem = document.createElement('div');
                equipmentItem.className = 'equipment-item';
                equipmentItem.style.cursor = 'pointer';
                equipmentItem.onclick = () => {
                    window.location.href =product.alias_url ;
                };

                const img = document.createElement('img');
                img.src = "/Resources/" + product.url || 'https://via.placeholder.com/200';
                img.alt = product.title;
                img.style = 'height: 200px; width: 100%; object-fit: cover;';

                const overlay = document.createElement('div');
                overlay.className = 'overlay';

                const title = document.createElement('h4');
                title.textContent = product.title;

                const description = document.createElement('p');
                description.innerHTML = product.description || 'Mô tả sản phẩm đang được cập nhật...';

                overlay.appendChild(title);
                overlay.appendChild(description);
                equipmentItem.appendChild(img);
                equipmentItem.appendChild(overlay);
                col.appendChild(equipmentItem);
                row.appendChild(col);
            }
        });

        if (row.children.length > 0) {
            slide.appendChild(row);
            productContainer.appendChild(slide);
        }
    });

    // Initialize Bootstrap carousel
    if (productContainer.children.length > 0) {
        new bootstrap.Carousel(carousel, {
            interval: 3000,
            wrap: true
        });
    }

    // Xử lý sự kiện resize window
    window.addEventListener('resize', () => {
        // Gọi lại hàm để cập nhật layout
        displayProductCarousel(productData);
    });
}
