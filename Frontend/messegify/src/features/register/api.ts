import {API_URL} from "../../config";

export async function register(name: string, password: string, email: string) {
    const response = await fetch(`${API_URL}/account`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*'
        },
        body: JSON.stringify({
            name: name,
            password: password,
            email: email
        })
    });

    const responseText = await response.text();

    if (response.status !== 200) {
        throw new Error(responseText);
    }

    return responseText;
}