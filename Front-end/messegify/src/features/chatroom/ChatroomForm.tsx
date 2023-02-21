import React, {FC, useState, useEffect} from 'react';
import {useGetMessages} from './api';
import {Message} from '../../types/message';
import {MantineProvider, Paper} from '@mantine/core';

export const ChatroomForm: FC = () => {
    const [messages, setMessages] = useState<Message[]>([]);
    const getMessages = useGetMessages();

    useEffect(() => {
        async function fetchData() {
            const receivedMessages = await getMessages;
            setMessages(receivedMessages);
        }

        fetchData();
    }, [getMessages]);

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
