import {API_URL} from "../../config";

export const register = async (username: string, password: string, email: string) => {
    const response = await fetch(`${API_URL}/account`, {
        method: 'POST',
        headers: {
            ContentType: 'application/json',
            Authorization: 'Basic' + window.btoa(username + ':' + password + ':' + email),
        },
        credentials: 'include'
    });

    if (response.status !== 200) throw new Error('Registration failed');
    return await response.text();
}