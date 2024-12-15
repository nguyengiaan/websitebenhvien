$(document).ready(function() {
    // Simulate some users
    const users = [
        { id: 1, name: "Nguyễn Văn A", avatar: "/img/default-avatar.png", unread: 3 },
        { id: 2, name: "Trần Thị B", avatar: "/img/default-avatar.png", unread: 0 },
        { id: 3, name: "Lê Văn C", avatar: "/img/default-avatar.png", unread: 1 }
    ];

    // Render users list
    function renderUsers() {
        const usersList = $('.users-list');
        usersList.empty();
        
        users.forEach(user => {
            const userHtml = `
                <div class="user-item" data-user-id="${user.id}">
                    <img src="${user.avatar}" class="user-avatar" alt="${user.name}">
                    <div class="user-info">
                        <div class="user-name">${user.name}</div>
                    </div>
                    <button class="delete-btn" onclick="deleteUser(${user.id})">
                        <i class="fas fa-trash"></i>
                    </button>
                    ${user.unread > 0 ? `
                        <span class="notification-badge ${user.unread > 0 ? 'animate-bell' : ''}">${user.unread}</span>
                        <i class="fas fa-bell notification-bell ${user.unread > 0 ? 'animate-bell' : ''}"></i>
                    ` : ''}
                </div>
            `;
            usersList.append(userHtml);
        });

        // Add CSS animation for bell
        if (!$('style#bell-animation').length) {
            $('head').append(`
                <style id="bell-animation">
                    .animate-bell {
                        animation: bellShake 1s infinite;
                    }
                    @keyframes bellShake {
                        0% { transform: rotate(0); }
                        15% { transform: rotate(5deg); }
                        30% { transform: rotate(-5deg); }
                        45% { transform: rotate(4deg); }
                        60% { transform: rotate(-4deg); }
                        75% { transform: rotate(2deg); }
                        85% { transform: rotate(-2deg); }
                        92% { transform: rotate(1deg); }
                        100% { transform: rotate(0); }
                    }
                </style>
            `);
        }
    }

    // Initialize
    renderUsers();

    // Handle user selection
    $('.users-list').on('click', '.user-item', function() {
        const userId = $(this).data('user-id');
        const user = users.find(u => u.id === userId);
        
        $('#selected-user-name').text(user.name);
        $('.online-status').removeClass('d-none');
        
        // Clear unread count
        user.unread = 0;
        renderUsers();
    });

    // Handle message sending
    $('#send-message').click(function() {
        const messageText = $('#message-input').val().trim();
        if (messageText) {
            const messageHtml = `
                <div class="message sent">
                    ${messageText}
                </div>
            `;
            $('#chat-messages').append(messageHtml);
            $('#message-input').val('');
            
            // Scroll to bottom
            const chatContainer = $('.chat-container');
            chatContainer.scrollTop(chatContainer[0].scrollHeight);
        }
    });

    // Handle enter key
    $('#message-input').keypress(function(e) {
        if (e.which == 13) {
            $('#send-message').click();
        }
    });
});