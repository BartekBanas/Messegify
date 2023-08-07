import Cookies from "js-cookie";
import ky from "ky";
import {API_URL} from "../../../../config";
import {removeJWTToken} from "../../../../pages/layout/Header";
import {useCookies} from "react-cookie";

export async function updateAccountRequest(username: string | null, password: string | null, email: string | null) {
    const requestBody: { username?: string; password?: string; email?: string } = {};

    requestBody.username = username ?? undefined;
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

export async function verifyAccountRequest(username: string, password: string) {
    const kyInstance = ky.extend({
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*'
        },
        body: JSON.stringify({
            UsernameOrEmail: username,
            Password: password,
        })
    });

    const response = kyInstance.post(`${API_URL}/account/authenticate`);

    return response;
}