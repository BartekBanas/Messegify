import ky from "ky";
import {API_URL} from "../../config";
import {Contact} from "../../types/contact";
import Cookies from "js-cookie";

export const useMenuApi = () => {

}

export const listContacts = () => {
    const token = Cookies.get('auth_token');

    const authorizedKy = ky.extend({
        headers: {
            authorization: `Bearer ${token}`
        }
    });

    console.log("And the token is:")
    console.log(token)

    return authorizedKy.get(`${API_URL}/contact/list`).json<Contact[]>();
}