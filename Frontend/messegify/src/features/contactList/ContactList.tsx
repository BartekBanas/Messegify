import React, {FC, useEffect, useState} from 'react';
import {Contact} from "../../types/contact";
import {ContactItem} from "./ContactItem";
import {Loader, MantineProvider, Paper} from "@mantine/core";
import {listContactsRequest} from "./api";
import '../../styles/CustomScrollbar.css';

export const ContactList: FC = () => {
    const [contacts, setContacts] = useState<Contact[]>([]);

    useEffect(() => {
        fetchContacts();

        const interval = setInterval(() => {
            fetchContacts();
        }, 1000);

        return () => clearInterval(interval);
    }, []);

    async function fetchContacts() {
        try {
            const data = await listContactsRequest();
            setContacts(data);
        } catch (error) {
            console.error('Error fetching contacts:', error);
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
                        {contacts.map((contact) => (
                            <ContactItem key={contact.id} contact={contact}/>
                        ))}
                    </React.Suspense>
                </ul>
            </Paper>
        </MantineProvider>
    );
};