import React, {FC, useEffect} from 'react';
import {Contact} from "../../types/contact";
import {listContacts} from "./api";
import {Link} from "react-router-dom";
import {API_URL} from "../../config";
import ky from "ky";
import {Account} from "../../types/account";
import Cookies from "js-cookie";
import {AccountClaims} from "../../types/accountClaims";
import {MantineProvider, Paper, Text} from "@mantine/core";

export const ContactList: FC = () => {
    const [contacts, setContacts] = React.useState<Contact[]>([]);
    const [friendsNames, setFriendsNames] = React.useState<{ [id: string]: string }>({});

    useEffect(() => {
        listContacts().then((data) => setContacts(data));
    }, []);

    async function getUserId() {
        const token = Cookies.get('auth_token');
        const authorizedKy = ky.extend({
            headers: {
                authorization: `Bearer ${token}`
            }
        });

        const response = await authorizedKy.get(`${API_URL}/account`).json<AccountClaims>();
        const userId = response['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid'];

        return parseInt(userId, 10);
    }

    async function getFriendsName(contact: Contact): Promise<string> {
        const userId = await getUserId();

        const token = Cookies.get('auth_token');
        const authorizedKy = ky.extend({
            headers: {
                authorization: `Bearer ${token}`
            }
        });

        let accountId;
        if (userId === parseInt(contact.firstAccountId, 10)) {
            accountId = contact.secondAccountId;
        } else {
            accountId = contact.firstAccountId;
        }

        if (friendsNames[accountId]) {
            // jeśli imię znajduje się już w pamięci, zwróć je
            return friendsNames[accountId];
        }

        const response = await authorizedKy.get(`${API_URL}/account/${accountId}`).json<Account>();
        const name = response.name;

        // dodaj imię do pamięci i zwróć je
        setFriendsNames({...friendsNames, [accountId]: name});
        return name;
    }

    const ContactItem = ({contact}: { contact: Contact }) => {
        const [name, setName] = React.useState<string | null>(null);

        useEffect(() => {
            getFriendsName(contact).then(setName);
        }, [contact]);

        return (
            <MantineProvider theme={{colorScheme: 'dark'}}>
                <Paper shadow="sm" radius="md" p="lg" withBorder>
                    <div style={{marginBottom: "10px"}}>
                        <Text color={'#D5D7E0'} sx={{
                            fontSize: 20,
                            lineHeight: 1.4,
                            fontWeight: 'bold',
                            fontFamily: '"Open Sans", sans-serif'
                        }}>
                            <li key={contact.id}>
                                <Link to={`/chatroom/${contact.contactChatRoomId}`}>Chatroom
                                    with {name || 'loading...'}</Link>
                            </li>
                        </Text>
                    </div>
                </Paper>
            </MantineProvider>
        );
    };

    return (
        <ul>
            <React.Suspense fallback={<div>Loading...</div>}>
                {contacts.map((contact) => (
                    <ContactItem key={contact.id} contact={contact}/>
                ))}
            </React.Suspense>
        </ul>
    );
};