import React, {FC, useEffect} from 'react';
import {Contact} from '../../types/contact';
import {MantineProvider, Paper, Text} from '@mantine/core';
import {Link} from 'react-router-dom';
import {API_URL} from '../../config';
import ky from 'ky';
import Cookies from 'js-cookie';
import {Account} from '../../types/account';
import {AccountClaims} from '../../types/accountClaims';

export const ContactItem: FC<{ contact: Contact }> = ({contact}) => {
    const [name, setName] = React.useState<string | null>(null);
    const [friendsNames, setFriendsNames] = React.useState<{ [id: string]: string }>({});

    useEffect(() => {
        getFriendsName(contact).then(setName);
    }, [contact]);

    async function getUserId() {
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

    async function getFriendsName(contact: Contact): Promise<string> {
        const userId = await getUserId();

        const token = Cookies.get('auth_token');
        const authorizedKy = ky.extend({
            headers: {
                authorization: `Bearer ${token}`,
            },
        });

        let accountId;
        if (userId === contact.firstAccountId) {
            accountId = contact.secondAccountId;
        } else {
            accountId = contact.firstAccountId;
        }

        if (friendsNames[accountId]) {
            return friendsNames[accountId];
        }

        const response = await authorizedKy.get(`${API_URL}/account/${accountId}`).json<Account>();
        const name = response.name;

        setFriendsNames({...friendsNames, [accountId]: name});
        return name;
    }

    return (
        <MantineProvider theme={{colorScheme: 'dark'}}>
            <Paper shadow="sm" radius="md" p="lg" withBorder>
                <div style={{marginBottom: '10px'}}>
                    <Text
                        color={'#D5D7E0'}
                        sx={{
                            fontSize: 20,
                            lineHeight: 1.4,
                            fontWeight: 'bold',
                            fontFamily: '"Open Sans", sans-serif',
                        }}
                    >
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