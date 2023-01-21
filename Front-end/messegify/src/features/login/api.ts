export const login = async (username: string, password: string) => {
    const response = await fetch('http://localhost:5000/auth/login', {
        method: 'POST',
        headers: {
            ContentType: 'application/json',
            Authorization: 'Basic' + window.btoa(username + ':' + password),
        },
        credentials: 'include'
    });

    if(response.status !== 200) throw new Error('Login failed');
    return await response.text();
}