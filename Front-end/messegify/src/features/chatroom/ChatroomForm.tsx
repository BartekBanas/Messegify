import React, {FC, useState, useEffect} from 'react';
import {useGetMessages, useMessageWebSocket} from './api';
import {Message} from '../../types/message';
import {Group, MantineProvider, Paper} from '@mantine/core';
import io from 'socket.io-client';
import {API_URL} from "../../config";
import {ContactList} from "../menu/ContactList";

export const ChatroomForm: FC = () => {
    const [messages, setMessages] = useState<Message[]>([]);
    const getMessages = useGetMessages();

    const lastMessage = useMessageWebSocket();

    async function fetchData() {
        const receivedMessages = await getMessages;
        setMessages(receivedMessages);

        console.log(receivedMessages);
    }

    useEffect(() => {
        fetchData();
    }, [])

    // useEffect(() => {
    //     fetchData().then()
    // }, [lastMessage])

    // useEffect(() => {
    //     const currentUrl = window.location.href;
    //     const roomId = currentUrl.split('/').pop();
    //     const socket = io(API_URL + '/chatRoom/' + {roomId} + '/message'); // adres hosta socket.io
    //
    //     socket.on('newMessage', (message: Message) => {
    //         setMessages([...messages, message]);
    //         console.log(message);
    //     });
    //
    //     return () => {
    //         socket.disconnect();
    //     };
    // }, [messages]);

    return (
        <MantineProvider theme={{colorScheme: 'dark'}}>

            <Group style={{width: '100%'}}>
                <ContactList/>

                <div>
                    {messages.map((message) => (
                        <Paper
                            key={message.id}
                            shadow="sm"
                            radius="md"
                            p="lg"
                            withBorder
                            style={{
                                maxWidth: "500px", marginBottom: '15px', float: 'right', clear: 'both'
                            }}
                        >
                            <div>{message.SentDate}</div>
                            <div>{message.textContent}</div>
                        </Paper>
                    ))}
                </div>
            </Group>
        </MantineProvider>
    );
};
