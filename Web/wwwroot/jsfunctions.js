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
