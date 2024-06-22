import Cookies from "js-cookie";
import ky from "ky";
import {API_URL} from "../../config";
import {Contact} from "../../types/contact";
import {AccountClaims} from "../../types/accountClaims";
import {Chatroom} from "../../types/chatroom";
import {Account} from "../../types/account";

export async function getContactsRequest() {
    const token = Cookies.get('auth_token');
    const authorizedKy = ky.extend({
        headers: {
            authorization: `Bearer ${token}`
        }
    });

    return authorizedKy.get(`${API_URL}/contact`).json<Contact[]>();
}

export async function getActiveContactsRequest() {
    const token = Cookies.get('auth_token');
    const authorizedKy = ky.extend({
        headers: {
            authorization: `Bearer ${token}`
        }
    });

    return authorizedKy.get(`${API_URL}/contact/active`).json<Contact[]>();
}

export async function getAccountRequest(otherMembersId: string) : Promise<Account> {
    const token = Cookies.get('auth_token');
    const authorizedKy = ky.extend({
        headers: {
            authorization: `Bearer ${token}`,
        },
    });

    return await authorizedKy.get(`${API_URL}/account/${otherMembersId}`).json<Account>();
}

export async function getChatroomsRequest() {
    const token = Cookies.get('auth_token');
    const authorizedKy = ky.extend({
        headers: {
            authorization: `Bearer ${token}`
        }
    });

    return authorizedKy.get(`${API_URL}/chatroom`).json<[Chatroom]>();
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