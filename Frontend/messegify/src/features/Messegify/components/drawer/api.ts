import Cookies from "js-cookie";
import ky from "ky";
import {API_URL} from "../../../../config";
import {removeJWTToken} from "../../../../pages/layout/Header";

export async function updateAccountRequest(name: string | null, password: string | null, email: string | null) {
    const requestBody: { name?: string; password?: string; email?: string } = {};

    requestBody.name = name ?? undefined;
    requestBody.password = password ?? undefined;
    requestBody.email = email ?? undefined;

    const token = Cookies.get('auth_token');
    const authorizedKy = ky.extend({
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*',
            authorization: `Bearer ${token}`
        },
        body: JSON.stringify(requestBody),
    });

    const response = authorizedKy.put(`${API_URL}/account/me`);

    return response;
}

export async function deleteAccountRequest() {
    const token = Cookies.get('auth_token');
    const authorizedKy = ky.extend({
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*',
            authorization: `Bearer ${token}`
        },
    });

    const response = authorizedKy.delete(`${API_URL}/account/me`);

    removeJWTToken();

    return response;
}

export async function deleteContactRequest(contactId: string) {
    const token = Cookies.get('auth_token');
    const authorizedKy = ky.extend({
        headers: {
            authorization: `Bearer ${token}`,
        },
    });

    await authorizedKy.delete(`${API_URL}/contact/${contactId}`);
}

export async function verifyAccountRequest(name: string, password: string) {
    const kyInstance = ky.extend({
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*'
        },
        body: JSON.stringify({
            UsernameOrEmail: name,
            Password: password,
        })
    });

    const response = kyInstance.post(`${API_URL}/account/authenticate`);

    return response;
}