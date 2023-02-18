import React, {FC, useEffect} from 'react';
import {Contact} from "../../types/contact";
import {listContacts} from "./api";
import {Link} from "react-router-dom";
import {API_URL} from "../../config";
import ky from "ky";
import {Account} from "../../types/account";
import Cookies from "js-cookie";

interface AccountData {
    "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid": string,
    "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid": string,
    "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name": string,
    "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress": string,
    nbf: string,
    exp: string,
    iat: string,
    iss: string,
    aud: string
}

export const ContactList: FC = () => {
    const [contacts, setContacts] = React.useState<Contact[]>([]);


    useEffect(() => {
        listContacts().then((data) => setContacts(data));


    }, [])

    async function getUserId() {
        const token = Cookies.get('auth_token');
        const authorizedKy = ky.extend({
            headers: {
                authorization: `Bearer ${token}`
            }
        });

        const response = await authorizedKy.get(`${API_URL}/account`).json<AccountData>();
        const userId = response['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid'];
        console.log("Retrieved user's id", userId)

        return parseInt(userId, 10);
    }

    async function getFriendsName(contact: Contact) {
        const userId = await getUserId();

        const token = Cookies.get('auth_token');
        const authorizedKy = ky.extend({
            headers: {
                authorization: `Bearer ${token}`
            }
        });
        if (userId === parseInt(contact.firstAccountId, 10)) {
            const response = await authorizedKy.get(`${API_URL}/account/${contact.secondAccountId}`).json<Account>();

            console.log(response)

            return response.name;
        } else {
            const response = await authorizedKy.get(`${API_URL}/account/${contact.firstAccountId}`).json<Account>();

            console.log(response)

            return response.name;
        }
    }

    return (
        <ul>
            {contacts.map((contact) => {
                let friendsName;

                getFriendsName(contact).then((name) => {
                    friendsName = "name";
                    console.log(friendsName);
                });

                return (
                    <li key={contact.id}>
                        <Link to={`/chatroom/`}>Chatroom with {friendsName}</Link>
                    </li>
                );
            })}
        </ul>
    );
};