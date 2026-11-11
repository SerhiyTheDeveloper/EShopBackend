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
