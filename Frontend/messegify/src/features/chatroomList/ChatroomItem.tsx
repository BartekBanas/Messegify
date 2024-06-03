import React, {FC, useEffect} from 'react';
import {Box, Loader, MantineProvider} from '@mantine/core';
import {Link} from 'react-router-dom';
import {API_URL} from '../../config';
import ky from 'ky';
import Cookies from 'js-cookie';
import {Account} from '../../types/account';
import {getUserId} from "./api";
import {Chatroom, ChatroomType} from "../../types/chatroom";

export const ChatroomItem: FC<{ chatroom: Chatroom }> = ({chatroom}) => {
    const [name, setName] = React.useState<string | undefined>(undefined);
    const [loading, setLoading] = React.useState(true);

    useEffect(() => {
        async function getChatroomName(chatroom: Chatroom): Promise<string | undefined> {
            let chatroomName;

            if (chatroom.chatRoomType === ChatroomType.regular) {
                chatroomName = chatroom.name;
            }

            if (chatroom.chatRoomType === ChatroomType.direct) {
                let contactName = await getFriendsName(chatroom);
                chatroomName = "Chatroom with " + contactName;
            }

            if (chatroomName !== undefined) {
                setLoading(false);
                return chatroomName;
            }
        }

        async function getFriendsName(chatroom: Chatroom): Promise<string> {
            const userId = await getUserId();
            const otherMembersId = chatroom.members.find((member) => member !== userId);

            const token = Cookies.get('auth_token');
            const authorizedKy = ky.extend({
                headers: {
                    authorization: `Bearer ${token}`,
                },
            });

            const response = await authorizedKy.get(`${API_URL}/account/${otherMembersId}`).json<Account>();
            return response.name;
        }

        async function fetchData() {
            const chatroomName = await getChatroomName(chatroom);
            setName(chatroomName);
        }

        fetchData();
    }, [chatroom]);

    return (
        <MantineProvider theme={{colorScheme: 'dark'}}>
            <Box
                component={Link} to={`/chatroom/${chatroom.id}`}
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
                {loading ? <Loader color="indigo" variant="dots"/> : name}
            </Box>
        </MantineProvider>
    );
};