import {API_URL} from "../../config";

export const register = async (username: string, password: string, email: string) => {

    const response = await fetch(`${API_URL}/account`, {
        method: 'POST',
        headers: {
            ContentType: 'application/json',
            // Authorization: 'Basic' + window.btoa(Username + ':' + Password + ':' + Email),
            'Access-Control-Allow-Origin': '*'
        },
        credentials: 'include',
        body: JSON.stringify({
            email: email,
            password: password,
            username: username
        })
    });

    if (response.status !== 200) throw new Error('Registration failed');
    return await response.text();
}