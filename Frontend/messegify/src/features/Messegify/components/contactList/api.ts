import Cookies from "js-cookie";
import ky from "ky";
import {API_URL} from "../../../../config";
import {Contact} from "../../../../types/contact";
import {AccountClaims} from "../../../../types/accountClaims";

export async function listContactsRequest() {
    const token = Cookies.get('auth_token');
    const authorizedKy = ky.extend({
        headers: {
            authorization: `Bearer ${token}`
        }
    });

    return authorizedKy.get(`${API_URL}/contact`).json<Contact[]>();
}

export async function getUserId() {
    const token = Cookies.get('auth_token');
    const authorizedKy = ky.extend({
        headers: {
            authorization: `Bearer ${token}`,
        },
    });

    const response = await authorizedKy.get(`${API_URL}/account/me`).json<AccountClaims>();
    const userId = response['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid'];

    return userId;
}