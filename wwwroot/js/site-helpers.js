(function(){
    'use strict';

    // Delegated language switcher: support elements with data-language or data-lang or class `change-language`
    document.addEventListener('click', function (e) {
        var el = e.target.closest && e.target.closest('[data-language], [data-lang], .change-language');
        if (!el) return;
        e.preventDefault();
        var lang = el.getAttribute('data-language') || el.getAttribute('data-lang') || el.dataset && el.dataset.lang;
        if (!lang) return;
        // call the existing function if defined, else do a fallback POST to set cookie
        if (typeof changeLanguage === 'function') {
            try { changeLanguage(lang); } catch (ex) { console.error('changeLanguage error', ex); }
            return;
        }

        // fallback: post to an endpoint to set language (adjust URL if needed)
        fetch('/change-language', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ lang: lang })
        }).then(function (resp) { location.reload(); }).catch(function(){ location.reload(); });
    });

    // Delegated header search: form with class .header-search or input with data-action="header-search"
    document.addEventListener('submit', function (e) {
        var form = e.target;
        if (!form.classList || !form.classList.contains('header-search')) return;
        e.preventDefault();
        var input = form.querySelector('input[name=keyword]') || form.querySelector('input[type=search]');
        if (!input) return;
        var q = input.value || '';
        var url = '/tim-kiem?keyword=' + encodeURIComponent(q);
        // If site uses ajax suggestions, that can be triggered here instead
        window.location.href = url;
    });

    // Delegated sample question buttons in chat (class .btn-sample-question)
    document.addEventListener('click', function (e) {
        var btn = e.target.closest && e.target.closest('.btn-sample-question');
        if (!btn) return;
        e.preventDefault();
        var q = btn.getAttribute('data-question') || '';
        try { q = decodeURIComponent(q); } catch (ex) {}
        if (typeof sendSampleQuestion === 'function') {
            sendSampleQuestion(q);
        } else if (typeof sendMessage === 'function') {
            var input = document.getElementById('messageInput');
            if (input) { input.value = q; sendMessage(); }
        } else {
            // fallback: set input value
            var input = document.getElementById('messageInput');
            if (input) { input.value = q; }
        }
    });

    // Hover effects for closeChatBtn (replaces inline onmouseover/onmouseout)
    document.addEventListener('mouseover', function (e) {
        var el = e.target.closest && e.target.closest('#closeChatBtn');
        if (!el) return;
        el.style.transform = 'rotate(90deg)';
    });
    document.addEventListener('mouseout', function (e) {
        var el = e.target.closest && e.target.closest('#closeChatBtn');
        if (!el) return;
        el.style.transform = 'rotate(0deg)';
    });

})();
