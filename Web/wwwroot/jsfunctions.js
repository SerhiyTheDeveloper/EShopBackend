async function loginUser(request) {
    try {
        const response = await fetch('/api/auth/login', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(request),
            credentials: 'include'
        });
        var result = await response.text()
        return result.replace(/"/g, '')
    } catch (error) {
        return "Помилка з'єднання. Спробуйте ще раз";
    }
}

async function registerUser(request) {
    try {
        const response = await fetch('/api/auth/register', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(request),
        });
        var result = await response.text()
        return result.replace(/"/g, '')
    } catch (error) {
        return "Помилка з'єднання. Спробуйте ще раз";
    }
}

async function resentCode(request) {
    try {
        const response = await fetch('/api/auth/resent', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(request),
        });
        var result = await response.text()
        return result.replace(/"/g, '')
    } catch (error) {
        return "Помилка з'єднання. Спробуйте ще раз";
    }
}

async function verifyUser(request) {
    try {
        const response = await fetch('/api/auth/verify', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(request),
        });
        var result = await response.text()
        return result.replace(/"/g, '')
    } catch (error) {
        return "Помилка з'єднання. Спробуйте ще раз";
    }
}

async function logoutUser() {
    try {
        const response = await fetch('/api/auth/logout', {
            method: 'POST',
            credentials: 'include'
        });
        return response.ok;
    } catch (error) {
        return false;
    }
}

async function callUserApi(method, request) {
    try {
        const response = await fetch('/api/user/me', {
            method: method,
            headers: { 'Content-Type': 'application/json' },
            body: request ? JSON.stringify(request) : undefined,
            credentials: 'include'
        });
        return await response.json();
    } catch (error) {
        return { success: false, message: "Помилка з'єднання. Спробуйте ще раз" };
    }
}

function getCurrentUser() {
    return callUserApi('GET');
}

function updateCurrentUser(request) {
    return callUserApi('PUT', request);
}

async function callApi(method, url, request) {
    try {
        const options = { method: method, credentials: 'include' };
        if (request) {
            options.headers = { 'Content-Type': 'application/json' };
            options.body = JSON.stringify(request);
        }
        const response = await fetch(url, options);
        return await response.json();
    } catch (error) {
        return { success: false, message: "Помилка з'єднання. Спробуйте ще раз" };
    }
}

function getAdminUsers() {
    return callApi('GET', '/api/admin/users');
}

function changeUserRole(userId, promote) {
    return callApi('POST', '/api/admin/users/' + userId + (promote ? '/promote' : '/demote'));
}

function deleteUser(userId) {
    return callApi('DELETE', '/api/admin/users/' + userId);
}

function callManagerApi(method, path, request) {
    return callApi(method, '/api/manager/' + path, request);
}

function callShopApi(method, path, request) {
    return callApi(method, '/api/shop/' + path, request);
}

function callWishListApi(method, path) {
    return callApi(method, '/api/wishlist' + (path ? '/' + path : ''));
}

function cartLoad() {
    try {
        return JSON.parse(localStorage.getItem('shopik.cart') || '[]');
    } catch (error) {
        return [];
    }
}

function cartSave(items) {
    localStorage.setItem('shopik.cart', JSON.stringify(items));
}

// Клік на посилання поточної сторінки (іконка кошика, пункт меню, «Головна») не має додавати в історію
// браузера ще один запис із тією самою адресою. Обробник у фазі перехоплення скасовує лише перехід за таким
// посиланням, а решта обробників (наприклад, закриття меню по @onclick) спрацьовує як зазвичай.
(function () {
    if (window.__sameUrlGuard) return;
    window.__sameUrlGuard = true;

    document.addEventListener('click', function (e) {
        if (e.button !== 0 || e.ctrlKey || e.metaKey || e.shiftKey || e.altKey) return;

        const link = e.target.closest && e.target.closest('a[href]');
        if (!link || (link.target && link.target !== '_self') || link.hasAttribute('download')) return;

        const url = new URL(link.href, location.href);
        const sameUrl = url.origin === location.origin
            && url.pathname === location.pathname
            && url.search === location.search
            && (!url.hash || url.hash === location.hash);

        if (sameUrl) e.preventDefault();
    }, true);
})();
