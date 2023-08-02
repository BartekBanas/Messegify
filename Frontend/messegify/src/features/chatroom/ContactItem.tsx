import React, {FC, useEffect} from 'react';
import {Contact} from '../../types/contact';
import {MantineProvider, Box, Loader} from '@mantine/core';
import {Link} from 'react-router-dom';
import {API_URL} from '../../config';
import ky from 'ky';
import Cookies from 'js-cookie';
import {Account} from '../../types/account';
import {getUserId} from "./api";

export const ContactItem: FC<{ contact: Contact }> = ({contact}) => {
    const [name, setName] = React.useState<string | null>(null);
    const [friendsNames, setFriendsNames] = React.useState<{ [id: string]: string }>({});
    const [loading, setLoading] = React.useState(true);

    useEffect(() => {
        getFriendsName(contact).then(setName);
    }, [contact]);

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
        setLoading(false);
        return name;
    }

    return (
        <MantineProvider theme={{colorScheme: 'dark'}}>
            <Box
                component={Link} to={`/chatroom/${contact.contactChatRoomId}`}
                sx={(theme) => ({
                    display: 'block',
                    backgroundColor: theme.colorScheme === 'dark' ? theme.colors.dark[6] : theme.colors.gray[0],
                    color: theme.colorScheme === 'dark' ? theme.colors.blue[4] : theme.colors.blue[7],
                    textAlign: 'center',
                    padding: theme.spacing.xl,
                    borderRadius: theme.radius.md,
                    cursor: 'pointer',
                    marginBottom: theme.spacing.xs,

                    '&:hover': {
                        backgroundColor:
                            theme.colorScheme === 'dark' ? theme.colors.dark[5] : theme.colors.gray[1],
                    },
                })}
            >
                {loading ? <Loader color="indigo" variant="dots"/> : "Chatroom with " + name}
            </Box>
        </MantineProvider>
    );
};