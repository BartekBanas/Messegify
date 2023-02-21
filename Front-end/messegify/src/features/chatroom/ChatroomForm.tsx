import React, {FC, useState, useEffect} from 'react';
import {useGetMessages} from './api';
import {Message} from '../../types/message';
import {MantineProvider, Paper} from '@mantine/core';
import io from 'socket.io-client';

export const ChatroomForm: FC = () => {
    const [messages, setMessages] = useState<Message[]>([]);
    const getMessages = useGetMessages();

    useEffect(() => {
        async function fetchData() {
            const receivedMessages = await getMessages;
            setMessages(receivedMessages);

            console.log(receivedMessages);
        }

        fetchData();
    }, []);

    useEffect(() => {
        const socket = io("https://localhost:5000/api"); // adres hosta socket.io

        socket.on('newMessage', (message: Message) => {
            setMessages([...messages, message]);
        });

        return () => {
            socket.disconnect();
        };
    }, [messages]);

    return (
        <MantineProvider theme={{colorScheme: 'dark'}}>
            {messages.map((message) => (
                <Paper key={message.id} shadow="sm" radius="md" p="lg" withBorder>
                    <div>{message.SentDate}</div>
                    <div>{message.textContent}</div>
                </Paper>
            ))}
        </MantineProvider>
    );
};
