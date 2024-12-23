
        
let currentPage = 1;
const pageSize = 20;


$(document).ready(function() {
    loadSpecialties();
    loadDoctors();
    loadSpecialtiess();


    $('#categoryModal').on('hidden.bs.modal', function () {
        resetForm();
    });

    $('#addDoctorButton').click(function() {
        $('#modalTitle').html('<i class="fas fa-user-md me-2"></i>Thêm bác sĩ');
        $('#categoryModal').modal('show');
    });
    
    $('#btnDelete').click(function(){
        var id=$('#Id_worksdoctor').val();
        var id_doctor=$('#btnDelete').data('id');   
        deleteSchedule(id,id_doctor);
    });
    $('#btnDelete').click(function () {
        var id = $('#Id_worksdoctor').val();
        var id_doctor = $('#btnDelete').data('id');
        deleteSchedule(id, id_doctor);
    });
    

});

function loadSpecialties() {
    $.ajax({
        url: '/api/chuyen-khoa-catogery',
        type: 'GET',
        success: function(response) {
       
            let options = '<option value="">Chọn chuyên khoa</option>';
            response.data.forEach(function(specialty) {
                options += `<option value="${specialty.id_Specialty}">${specialty.title}</option>`;
            });
            $('#Id_specialty').html(options);
        }
    });
}

function loadDoctors() {
    $.ajax({
        url: '/api/lay-danh-sach-bac-si',
        type: 'POST',
        data: {
            page: currentPage,
            pageSize: pageSize,
            search: $('#searchInput').val(),
            specialtyId: $('#specialtyFilter').val()
        },
        success: function(response) {
            if(response.success) {
                renderDoctors(response.data);
                renderPagination(response.totalPages, currentPage);
            }
        }
    });
}

function renderDoctors(doctors) {
    let html = '';
    doctors.forEach(function(doctor, index) {
        html += `
            <tr class="align-middle">
                <td class="ps-4 fw-bold text-primary">${index + 1}</td>
                <td>
                    <img src="/Resources/${doctor.thumnail}" 
                         class="rounded-3 shadow-sm" 
                         style="width: 80px; height: 80px; object-fit: cover">
                </td>
                <td>
                    <h6 class="mb-1 fw-bold">${doctor.name}</h6>
                    <small class="text-muted">BS. ${doctor.nameCategory}</small>
                </td>
                <td>
                    <span class="badge bg-light text-dark px-3 py-2 rounded-pill">
                        ${doctor.nameCategory}
                    </span>
                </td>
                <td>
                    <span class="text-muted small">
                        ${doctor.alias_url}
                    </span>
                </td>
                <td class="text-center">
                    <div class="btn-group" role="group" aria-label="Doctor actions">
                        <button type="button" class="btn btn-outline-primary btn-sm rounded-circle me-2" 
                                onclick="editDoctor(${doctor.id_doctor})"
                                aria-label="Chỉnh sửa bác sĩ">
                            <i class="fas fa-edit" aria-hidden="true"></i>
                        </button>
                        <button type="button" class="btn btn-outline-danger btn-sm rounded-circle me-2" 
                                onclick="deleteDoctor(${doctor.id_doctor})"
                                aria-label="Xóa bác sĩ">
                            <i class="fas fa-trash" aria-hidden="true"></i>
                        </button>
                        <button type="button" class="btn btn-outline-success btn-sm rounded-circle"
                                onclick="addWorkSchedule(${doctor.id_doctor})" 
                                aria-label="Thêm thời khoá biểu" id="btnsvadd">
                            <i class="fas fa-calendar-plus" aria-hidden="true"></i>
                        </button>
                    </div>
                </td>
            </tr>
        `;
    });
    $('#categoryTableBody').html(html);
    
    // Initialize tooltips
    $('[data-bs-toggle="tooltip"]').tooltip();
}

function renderPagination(totalPages, currentPage) {
    let html = '';
    
    // Previous button
    html += `
        <button class="btn btn-outline-primary btn-sm me-1" 
                onclick="changePage(${currentPage - 1})"
                ${currentPage === 1 ? 'disabled' : ''}>
            &laquo;
        </button>
    `;

    // Calculate range of pages to show
    let startPage = Math.max(1, currentPage - 2);
    let endPage = Math.min(totalPages, startPage + 4);
    
    // Adjust start if end is maxed out
    if (endPage - startPage < 4) {
        startPage = Math.max(1, endPage - 4);
    }

    // First page + ellipsis
    if (startPage > 1) {
        html += `
            <button class="btn btn-outline-primary btn-sm me-1" onclick="changePage(1)">1</button>
            ${startPage > 2 ? '<span class="me-1">...</span>' : ''}
        `;
    }

    // Page numbers
    for (let i = startPage; i <= endPage; i++) {
        html += `
            <button class="btn btn-${currentPage === i ? 'primary' : 'outline-primary'} btn-sm me-1" 
                    onclick="changePage(${i})">
                ${i}
            </button>
        `;
    }

    // Last page + ellipsis
    if (endPage < totalPages) {
        html += `
            ${endPage < totalPages - 1 ? '<span class="me-1">...</span>' : ''}
            <button class="btn btn-outline-primary btn-sm me-1" onclick="changePage(${totalPages})">${totalPages}</button>
        `;
    }

    // Next button
    html += `
        <button class="btn btn-outline-primary btn-sm me-1"
                onclick="changePage(${currentPage + 1})"
                ${currentPage === totalPages ? 'disabled' : ''}>
            &raquo;
        </button>
    `;

    $('#pagination').html(html);
}

function changePage(page) {
    currentPage = page;
    loadDoctors();
}

function editDoctor(id) {
  
    $.ajax({
        url: '/api/lay-bac-si-theo-chuyen-khoa',
        type: 'POST',
        data: { id: id },
        success: function(doctor) {
           
            $('#modalTitle').html('<i class="fas fa-user-md me-2"></i>Cập nhật bác sĩ');
            $('#id_doctor').val(doctor.data[0].id_doctor);
            $('#name').val(doctor.data[0].name);
            $('#Alias_url').val(doctor.data[0].alias_url);
            $('#Id_specialty').val(doctor.data[0].id_specialty);
            $('#introduction').summernote('code', doctor.data[0].introduction);
            $('#organization').summernote('code', doctor.data[0].organization);
            $('#award').summernote('code', doctor.data[0].award);
            $('#research').summernote('code', doctor.data[0].research);
            $('#training').summernote('code', doctor.data[0].training);
            $('#experiencework').summernote('code', doctor.data[0].experiencework);
            $('#categoryModal').modal('show');
        }
    });
}

function deleteDoctor(id) {
    Swal.fire({
        title: 'Bạn có chắc chắn?',
        text: "Bạn sẽ không thể hoàn tác hành động này!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6', 
        cancelButtonColor: '#d33',
        confirmButtonText: 'Đồng ý',
        cancelButtonText: 'Hủy'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/api/xoa-bac-si',
                type: 'POST',
                data: { id: id },
                success: function(response) {
                    if(response.success) {
                        Swal.fire(
                            'Đã xóa!',
                            'Bác sĩ đã được xóa thành công.',
                            'success'
                        )
                        loadDoctors();
                    } else {
                        Swal.fire(
                            'Lỗi!',
                            response.message,
                            'error'
                        )
                    }
                }
            });
        }
    })
}

function searchDoctors() {
    let searchTerm = $('#searchInput').val();
    let specialtyId = $('#specialtyFilter').val();
    $.ajax({
        url: '/api/lay-danh-sach-bac-si',
        type: 'GET',
        data: {
            searchTerm: searchTerm,
            page: currentPage,
            pageSize: pageSize
        },
        success: function(response) {
            renderDoctors(response.ds);
            renderPagination(response.totalPages);
        }
    });
}

function resetForm() {
    $('#categoryForm')[0].reset();
    $('#id_doctor').val('');
    $('#name').val('');
    $('#id_specialty').val('');
    $('#introduction').summernote('code', '');
    $('#organization').summernote('code', '');
    $('#award').summernote('code', '');
    $('#research').summernote('code', '');
    $('#training').summernote('code', '');
    $('#experiencework').summernote('code', '');
    $('#formFile').val('');
    $('#previewImage').attr('src', '').addClass('d-none');
}

function saveDoctor() {
    var form = $('#categoryForm')[0];
    const isNew = !$('#id_doctor').val();
    const formData = new FormData(form);

    $.ajax({
        url: '/api/them-bac-si',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        beforeSend: function() {    
            Swal.fire({
                title: 'Chờ một xíu....',
                html: `
                    <div class="text-center">
                        <img src="https://i.pinimg.com/originals/fe/a5/33/fea5336ece9573d235870d51b8d28e7a.gif" width="100" height="100" alt="Loading..." />
                        <div class="progress mt-3" style="height: 20px;">
                            <div class="progress-bar progress-bar-striped progress-bar-animated bg-primary" 
                                 role="progressbar" 
                                 style="width: 0%"
                                 aria-valuenow="0" 
                                 aria-valuemin="0" 
                                 aria-valuemax="100">
                            </div>
                        </div>
                    </div>
                `,
                showConfirmButton: false,
                allowOutsideClick: false,
                background: '#ffffff',
                didOpen: () => {
                    let progress = 0;
                    const bar = document.querySelector('.progress-bar');
                    const interval = setInterval(() => {
                        progress += 2;
                        bar.style.width = progress + '%';
                        bar.setAttribute('aria-valuenow', progress);
                        if (progress >= 100) {
                            clearInterval(interval);
                        }
                    }, 50);
                }
            });
        },
        success: function(response) {
            if (response.success) {
                Swal.fire({
                    title: 'Thành công!',
                    text: isNew ? "Thêm bác sĩ thành công" : "Cập nhật bác sĩ thành công",
                    icon: 'success'
                }).then(() => {
                    resetForm();
                    $('#categoryModal').modal('hide');
                    loadDoctors(currentPage,pageSize);
                });
            } else {
                Swal.fire({
                    title: 'Lỗi!',
                    text: response.message || "Có lỗi xảy ra",
                    icon: 'error'
                });
            }
        },
        error: function(xhr, status, error) {
            Swal.fire({
                title: 'Lỗi!',
                text: "Có lỗi xảy ra khi xử lý yêu cầu",
                icon: 'error'
            });
        }
    });
}

function generateAlias() {
    let name = $('#name').val();
    // Chuyển về chữ thường
    let str = name.toLowerCase();

    // Xóa dấu
    str = str.normalize('NFD').replace(/[\u0300-\u036f]/g, '');

    // Thay thế ký tự đặc biệt bằng dấu gạch ngang
    str = str.replace(/[^a-z0-9 -]/g, '')
            .replace(/\s+/g, '-')     // Thay khoảng trắng bằng dấu gạch ngang
            .replace(/-+/g, '-');      // Xóa nhiều dấu gạch ngang liên tiếp

    // Xóa dấu gạch ngang ở đầu và cuối
    str = str.replace(/^-+|-+$/g, '');
    $('#Alias_url').val('/bac-si/' + str);
    return '/bac-si/' + str;
}

function generateScheduleTable() {
    const startDate = new Date($('#startDate').val());
    const endDate = new Date($('#endDate').val());

    if (!startDate || !endDate || startDate > endDate) {
        $('#scheduleCheckboxes').html('');
        return;
    }
    if(!startDate || !endDate) {
        Swal.fire({
            title: 'Lỗi!', 
            text: "Vui lòng chọn ngày bắt đầu và ngày kết thúc",
            icon: 'error'
        });
        return;
    }
    
    if(startDate > endDate) {
        Swal.fire({
            title: 'Lỗi!',
            text: "Ngày bắt đầu không được lớn hơn ngày kết thúc",
            icon: 'error'
        });
        $('#startDate').val('');
        $('#endDate').val('');
        return;
    }

    let tableHtml = `
        <table class="table table-bordered">
            <thead>
                <tr>
                    <th>Ngày</th>
                    <th class="text-center">Buổi sáng</th>
                    <th class="text-center">Buổi chiều</th>
                    <th class="text-center">Buổi tối</th>
                </tr>
            </thead>
            <tbody>
    `;

    for (let date = new Date(startDate); date <= endDate; date.setDate(date.getDate() + 1)) {
        const formattedDate = date.toLocaleDateString('vi-VN', {
            weekday: 'long',
            year: 'numeric',
            month: '2-digit',
            day: '2-digit'
        });
        const dateValue = date.toISOString().split('T')[0];

        tableHtml += `
            <tr>
                <td>${formattedDate}</td>
                <td class="text-center">
                    <input type="checkbox" class="form-check-input" name="schedule[${dateValue}][morning]" value="true">
                </td>
                <td class="text-center">
                    <input type="checkbox" class="form-check-input" name="schedule[${dateValue}][afternoon]" value="true">
                </td>
                <td class="text-center">
                    <input type="checkbox" class="form-check-input" name="schedule[${dateValue}][evening]" value="true">
                </td>
            </tr>
        `;
    }

    tableHtml += `
            </tbody>
        </table>
    `;

    $('#scheduleCheckboxes').html(tableHtml);
}

function addWorkSchedule(id) {
    $('#scheduleModal').modal('show');
    $('#btnsave').data('id',id);
    $('#btnDelete').data('id',id);
    getSchedule(id);
}

function saveWorkSchedule() {
    const startDate = new Date($('#startDate').val());
    const endDate = new Date($('#endDate').val());
    const doctorId = $('#btnsave').data('id');
    const note = $('#scheduleNote').val();

    const workDoctorData = {
        Id_workdoctor: $('#Id_worksdoctor').val(),
        Id_doctor: parseInt(doctorId),
        CreateAt: startDate.toISOString(),
        EndDate: endDate.toISOString(),
        Note: note,
        Workschedules: []
    };

    // Lặp qua từng ngày để lấy dữ liệu từ các checkbox
    for (let date = new Date(startDate); date <= endDate; date.setDate(date.getDate() + 1)) {
        const dateValue = date.toISOString().split('T')[0];
        
        const schedule = {
            Date: dateValue,
            Morning: $(`input[name="schedule[${dateValue}][morning]"]`).is(':checked') ? "true" : "false",
            Afternoon: $(`input[name="schedule[${dateValue}][afternoon]"]`).is(':checked') ? "true" : "false", 
            Evening: $(`input[name="schedule[${dateValue}][evening]"]`).is(':checked') ? "true" : "false"
        };

        workDoctorData.Workschedules.push(schedule);
    }

    $.ajax({
        url: '/api/them-lich-lam-viec',
        type: 'POST',
        data: workDoctorData,
        success: function(response) {
            if (response.status) {
                Swal.fire({
                    title: 'Thành công!',
                    text: "Thêm lịch làm việc thành công",
                    icon: 'success'
                }).then(() => {
                    $('#scheduleModal').modal('hide');
                    loadDoctors(currentPage, pageSize);
                });
            } else {
                Swal.fire({
                    title: 'Lỗi!',
                    text: response.message || "Có lỗi xảy ra",
                    icon: 'error'
                });
            }
        },
        error: function() {
            Swal.fire({
                title: 'Lỗi!',
                text: "Có lỗi xảy ra khi xử lý yêu cầu",
                icon: 'error'
            });
        }
    });
}

function getSchedule(id_doctor) {
    // Reset form
    $('#scheduleForm')[0].reset();
    $('#scheduleCheckboxes').empty();
    $.ajax({
        url: '/api/lich-lam-viec',
        type: 'POST',
        data: { id_doctor: id_doctor },
        success: function(response) {
            if (response.status && response.data) {
                const workSchedule = response.data;
            
                // Điền thông tin cơ bản
                $('#Id_worksdoctor').val(workSchedule.id_workdoctor);
           
                
                $('#startDate').val(workSchedule.createAt.split('T')[0]);
                $('#endDate').val(workSchedule.endDate.split('T')[0]);
                $('#scheduleNote').val(workSchedule.note);

                // Tạo bảng lịch làm việc
                generateScheduleTable();

                // Đánh dấu các ca làm việc
                workSchedule.workschedules.forEach(schedule => {
                    const date = schedule.date.split('T')[0];
                    if(schedule.morning === "true") {
                        $(`input[name="schedule[${date}][morning]"]`).prop('checked', true);
                    }
                    if(schedule.afternoon === "true") {
                        $(`input[name="schedule[${date}][afternoon]"]`).prop('checked', true);
                    }
                    if(schedule.evening === "true") {
                        $(`input[name="schedule[${date}][evening]"]`).prop('checked', true);
                    }
                });

                // Hiển thị modal
                $('#scheduleModal').modal('show');
            } else {
                Swal.fire({
                    title: 'Thông báo',
                    text: response.message || "Không tìm thấy lịch làm việc",
                    icon: 'info'
                });
            }
        },
        error: function() {
            Swal.fire({
                title: 'Lỗi!',
                text: "Có lỗi xảy ra khi tải lịch làm việc",
                icon: 'error'
            });
        }
    });
}

function deleteSchedule(id,id_doctor) {
    $.ajax({
        url: '/api/xoa-lich-lam-viec',
        type: 'POST',
        data: { id: id },
        success: function(response) {
            if (response.status) {
                Swal.fire({
                    title: 'Thành công!',
                    text: "Xóa lịch làm việc thành công",
                    icon: 'success'
                });
                getSchedule(id_doctor);
            }
        },
        error: function() {
            Swal.fire({
                title: 'Lỗi!',
                text: "Có lỗi xảy ra khi xử lý yêu cầu",
                icon: 'error'
            });
        }
    });
}

function loadSpecialtiess() {
    $.ajax({
        url: '/api/chuyen-khoa-catogery',
        type: 'GET',
        success: function(response) {
            let options = '<option value="">Tất cả chuyên khoa</option>';
            response.data.forEach(function(specialty) {
                options += `<option value="${specialty.id_Specialty}">${specialty.title}</option>`;
            });
            $('#specialtyFilter').html(options);
        }
    });
}

  function openElfinder() {
    var fm = $('<div/>').dialogelfinder({
        url: '@Url.Action("Connector", "Filemanager")',
        lang: 'vi',
        width: 850,
        height: 450,
        destroyOnClose: true,
        getFileCallback: function(files, fm) {
            const fileUrl = files.url;
            fetch(fileUrl)
                .then(response => response.blob())
                .then(blob => {
                    const file = new File([blob], files.name, { type: blob.type });
                    
                    const dataTransfer = new DataTransfer();
                    dataTransfer.items.add(file);
                    
                    document.getElementById('ImageFile').files = dataTransfer.files;
                    
                    // Đóng dialog sau khi chọn file
                    fm.hide();
                    
                    // Hiển thị tên file đã chọn
                    $('#ImageFile').next('.custom-file-label').html(files.name);
                });
        },
        commandsOptions: {
            getfile: {
                oncomplete: 'close'
            }
        }
    }).dialogelfinder('instance');
}
