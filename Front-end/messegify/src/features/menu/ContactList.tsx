import React, {FC, useEffect, useState} from 'react';
import {Contact} from "../../types/contact";
import {listContacts} from "./api";
import {Link} from "react-router-dom";
import {API_URL} from "../../config";
import ky from "ky";
import {Account} from "../../types/account";

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
    const [friendsName, setFriendsName] = useState<string | undefined>(undefined);

    useEffect(() => {
        listContacts().then((data) => setContacts(data));


    }, [])

    async function getUserId() {
        const response = await ky.get(`${API_URL}/account`).json<AccountData>();
        const userId = response['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid'];
        console.log("Retrieved user's id")
        console.log(userId)
        return parseInt(userId, 10);
    }

    async function getFriendsName(contact: Contact) {
        const userId = await getUserId();
        if (userId === parseInt(contact.firstAccountId, 10)) {
            const response = await ky.get(`${API_URL}/account/{secondAccountId}`).json<Account>();

            return response.name;
        } else {
            const response = await ky.get(`${API_URL}/account/{firstAccountId}`).json<Account>();

            return response.name;
        }
    }

    return (
        <ul>
            {contacts.map((contact) => {
                let friendsName;

                getFriendsName(contact).then((name) => {
                    friendsName = name;
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