import {API_URL} from "../../config";

export const register = async (username: string, password: string, email: string) => {

    const uri = `${API_URL}/account`;
    console.log(uri)

    const response = await fetch(`${API_URL}/account`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*'
        },
        body: JSON.stringify({
            email: email,
            password: password,
            username: username
        })
    });

    if (response.status !== 200) throw new Error('Registration failed');
    return await response.text();
}