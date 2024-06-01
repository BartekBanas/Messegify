import React, {FC, useEffect, useRef, useState} from 'react';
import {useForm} from '@mantine/form';
import {Button, Group, MantineProvider, Paper, Text, TextInput} from '@mantine/core';
import {useLocation} from 'react-router-dom';
import {API_URL} from '../../config';
import {ChatroomList} from '../contactList/ChatroomList';
import Cookies from 'js-cookie';
import ky from 'ky';
import {AccountClaims} from '../../types/accountClaims';
import './ChatroomForm.css';
import {Message} from '../../types/message';
import {handleSubmit, useGetMessages} from './api';
import {MenuButton} from "./MenuButton";

type ChatMessageProps = {
    message: Message;
    userId: string;
};

const ChatMessage: FC<ChatMessageProps> = ({message, userId}) => {
    const messageClass = message.accountId === userId ? 'my-message' : 'not-my-message';
    const classes = `${messageClass} message-entry`;

    return (
        <Paper key={message.id} shadow="sm" radius="md" p="lg" withBorder className={classes}>
            <div>{message.SentDate}</div>
            <div>{message.textContent}</div>
        </Paper>
    );
};

type ChatroomFormProps = {};

export const ChatroomForm: FC<ChatroomFormProps> = () => {
    const currentUrl = window.location.href;
    const roomId = currentUrl.split('/').pop() ?? '';
    const [messages, setMessages] = useState<Message[]>([]);
    const [userId, setUserId] = useState('');
    const getMessages = useGetMessages();
    const messageContainerRef = useRef<HTMLDivElement>(null);
    const [lastMessage, setLastMessage] = useState<Message | null>(null);

    const location = useLocation();

    const form = useForm<Message>({
        initialValues: {
            id: '',
            textContent: '',
            accountId: '',
            SentDate: '',
        },
    });

    async function updateUserId() {
        const token = Cookies.get('auth_token');
        const authorizedKy = ky.extend({
            headers: {
                authorization: `Bearer ${token}`,
            },
        });

        const response = await authorizedKy.get(`${API_URL}/account/me`).json<AccountClaims>();

        setUserId(response['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid']);
    }

    async function fetchData() {
        const receivedMessages = await getMessages;
        setMessages(receivedMessages);
    }

    useEffect(() => {
        updateUserId();
        fetchData();
    }, [roomId]);

    useEffect(() => {
        fetchData();
    }, [location]);

    useEffect(() => {
        const fetchLastMessage = async () => {
            try {
                const token = Cookies.get('auth_token');
                const authorizedKy = ky.extend({
                    headers: {
                        'Content-Type': 'application/json',
                        'Access-Control-Allow-Origin': '*',
                        authorization: `Bearer ${token}`
                    }
                });

                const response = await authorizedKy.get(`${API_URL}/chatRoom/${roomId}/message`).json<Message[]>();
                if (response.length > 0) {
                    const latestMessage = response[response.length - 1];
                    if (latestMessage.id !== lastMessage?.id) {
                        setLastMessage(latestMessage);
                    }
                }
            } catch (error) {
                console.error('Error fetching last message:', error);
            }
        };

        const interval = setInterval(fetchLastMessage, 1000);

        return () => {
            clearInterval(interval);
        };
    }, [roomId, lastMessage]);

    useEffect(() => {
        const fetchDataWithDelay = async () => {
            try {
                await fetchData();
                setTimeout(() => {
                    scrollToBottom();
                }, 100);
            } catch (error) {
                console.error('Error fetching data:', error);
            }
        };

        fetchDataWithDelay();
    }, [lastMessage]);

    const scrollToBottom = () => {
        if (messageContainerRef.current) {
            messageContainerRef.current.scrollTo({
                top: messageContainerRef.current.scrollHeight,
                behavior: 'smooth',
            });
        }
    };

    return (
        <MantineProvider theme={{colorScheme: 'dark'}}>
            <Group style={{width: '100%', display: 'flex', height: '100%'}}>
                <ChatroomList/>

                <div style={{flex: 4, height: '100%', maxHeight: '87vh'}}>
                    <Paper shadow="sm" radius="md" p="lg" withBorder className="custom-scrollbar"
                           style={{flex: 4, height: '90%', overflow: 'auto'}}
                           ref={messageContainerRef}>
                        {messages.map((message) => (
                            <ChatMessage key={message.id} message={message} userId={userId}/>
                        ))}
                    </Paper>
                    <Paper shadow="sm" radius="md" p="lg" withBorder style={{height: '10%'}}>
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
                                <form onSubmit={form.onSubmit((values) => {
                                    handleSubmit(values, roomId);
                                    form.reset();
                                })}>
                                    <Group>
                                        <TextInput required type="message" {...form.getInputProps('textContent')}
                                                   style={{width: '300px'}}/>

                                        <Button type="submit"> Send </Button>

                                        <div style={{marginLeft: 'auto'}}>
                                            <MenuButton/>
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