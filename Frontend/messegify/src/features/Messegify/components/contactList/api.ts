import Cookies from "js-cookie";
import ky from "ky";
import {API_URL} from "../../../../config";
import {Contact} from "../../../../types/contact";

export async function listContactsRequest() {
    const token = Cookies.get('auth_token');
    const authorizedKy = ky.extend({
        headers: {
            authorization: `Bearer ${token}`
        }
    });

    return authorizedKy.get(`${API_URL}/contact`).json<Contact[]>();
}