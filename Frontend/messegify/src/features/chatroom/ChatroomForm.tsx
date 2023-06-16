import React, {FC, useEffect, useState} from 'react';
import {handleSubmit, useGetMessages, useMessageWebSocket} from './api';
import {Message} from '../../types/message';
import {Button, Group, MantineProvider, Paper, Text, TextInput} from '@mantine/core';
import {API_URL} from "../../config";
import {ContactList} from "../menu/ContactList";
import Cookies from "js-cookie";
import ky from "ky";
import {AccountClaims} from "../../types/accountClaims";
import './ChatroomForm.css'
import {io} from "socket.io-client";
import {useForm} from "@mantine/form";
import {Link} from "react-router-dom";

export const ChatroomForm: FC = () => {
    const currentUrl = window.location.href;
    const roomId = currentUrl.split('/').pop() ?? '';
    const [messages, setMessages] = useState<Message[]>([]);
    const [userId, setUserId] = useState("");
    const getMessages = useGetMessages();

    const form = useForm<Message>({
        initialValues: {
            id: '',
            textContent: '',
            accountId: '',
            SentDate: ''
        },
    })

    const lastMessage = useMessageWebSocket();

    async function getUserId() {
        const token = Cookies.get('auth_token');
        const authorizedKy = ky.extend({
            headers: {
                authorization: `Bearer ${token}`
            }
        });

        const response = await authorizedKy.get(`${API_URL}/account`).json<AccountClaims>();

        setUserId(response['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid']);
    }

    async function fetchData() {
        const receivedMessages = await getMessages;
        setMessages(receivedMessages);

        console.log(receivedMessages);
    }

    useEffect(() => {
        getUserId();
        fetchData();
    }, [])

    // useEffect(() => {
    //     fetchData().then()
    // }, [lastMessage])

    useEffect(() => {
        const socket = io("ws://localhost:5000"); // adres hosta socket.io

        socket.on('newMessage', (message: Message) => {
            setMessages([...messages, message]);
            console.log(message);
        });

        return () => {
            socket.disconnect();
        };
    }, [messages]);

    const handleLogout = () => {
        Cookies.remove('auth_token');
    };

    return (
        <MantineProvider theme={{colorScheme: 'dark'}}>
            <Group style={{width: '100%', display: 'flex', height: '100%'}}>
                <Paper shadow="sm" radius="md" p="lg" withBorder style={{flex: 1, height: '100%'}}>
                    <ContactList/>
                </Paper>

                <div style={{flex: 4, height: '100%', maxHeight: "87vh",}}>
                    <Paper shadow="sm" radius="md" p="lg" withBorder style={{flex: 4, height: '90%', overflow: 'auto'}}>
                        {messages.map((message) => {

                            const messageClass = message.accountId === userId ? "my-message" : "not-my-message";
                            const classes = `${messageClass} message-entry`;

                            return (
                                <Paper
                                    key={message.id} shadow="sm" radius="md" p="lg" withBorder className={classes}>
                                    <div>{message.SentDate}</div>
                                    <div>{message.textContent}</div>
                                </Paper>
                            )
                        })}
                    </Paper>
                    <Paper shadow="sm" radius="md" p="lg" withBorder style={{height: '10%'}}>
                        <div style={{marginBottom: "10px"}}>
                            <Text color={'#D5D7E0'} sx={{
                                fontSize: 20,
                                lineHeight: 1.4,
                                fontWeight: 'bold',
                                fontFamily: '"Open Sans", sans-serif'
                            }}>
                                <form onSubmit={form.onSubmit(values => handleSubmit(values, roomId))}>
                                    <Group>
                                        <TextInput required type="message"
                                                   {...form.getInputProps('textContent')}
                                                   style={{width: '300px'}}/>

                                        <Button type="submit"> Send </Button>

                                        <div style={{marginLeft: 'auto'}}>
                                            <Link to="/menu">
                                                <Button variant="gradient"
                                                        gradient={{from: 'teal', to: 'lime', deg: 105}}>
                                                    Menu
                                                </Button>
                                            </Link>
                                        </div>
                                    </Group>
                                </form>
                            </Text>
                        </div>
                    </Paper>
                </div>
            </Group>
        </MantineProvider>
    );
};
