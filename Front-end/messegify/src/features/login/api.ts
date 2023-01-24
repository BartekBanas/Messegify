import {API_URL} from "../../config";

export const login = async (username: string, password: string) => {
    const response = await fetch(`${API_URL}/account/authenticate`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*'
        },
        body: JSON.stringify({
            UsernameOrEmail: username,
            Password: password,
        })
    });

    if (response.status !== 200) throw new Error('Login failed');
    return await response.text();
}