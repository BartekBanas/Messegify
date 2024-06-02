import React, {FC, useEffect, useState} from 'react';
import {ChatroomItem} from "./ChatroomItem";
import {Loader, MantineProvider, Paper} from "@mantine/core";
import {getChatroomsRequest} from "./api";
import '../../styles/CustomScrollbar.css';
import {Chatroom} from "../../types/chatroom";

export const ChatroomList: FC = () => {
    const [chatrooms, setChatrooms] = useState<Chatroom[]>([]);

    useEffect(() => {
        fetchContacts();

        const interval = setInterval(() => {
            fetchContacts();
        }, 1000);

        return () => clearInterval(interval);
    }, []);

    async function fetchContacts() {
        try {
            const data = await getChatroomsRequest();
            setChatrooms(data);
        } catch (error) {
            console.error('Error fetching chatrooms:', error);
        }
    }

    return (
        <MantineProvider theme={{colorScheme: 'dark'}}>
            <Paper
                shadow="sm"
                radius="md"
                p="lg"
                withBorder
                style={{maxHeight: '87vh', height: '100%'}}
                className="custom-scrollbar"
            >
                <ul>
                    <React.Suspense fallback={<Loader color="indigo" variant="dots"/>}>
                        {chatrooms.map((contact) => (
                            <ChatroomItem key={contact.id} chatroom={contact}/>
                        ))}
                    </React.Suspense>
                </ul>
            </Paper>
        </MantineProvider>
    );
};