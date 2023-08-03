import React, {FC, useEffect, useState} from 'react';
import {Contact} from "../../../../types/contact";
import {listContacts} from "../../../menu/api";
import {ContactItem} from "./ContactItem";
import {Loader, MantineProvider, Paper, ScrollArea} from "@mantine/core";

export const ContactList: FC = React.memo(() => {
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
            const data = await listContacts();
            setContacts(data);
        } catch (error) {
            console.error('Error fetching contacts:', error);
        }
    }

    return (
        <MantineProvider theme={{colorScheme: 'dark'}}>
            <ScrollArea h={700} type="scroll" scrollHideDelay={500}>
                <Paper shadow="sm" radius="md" p="lg" withBorder style={{flex: 1, height: '100%'}}>
                    <ul>
                        <React.Suspense fallback={<Loader color="indigo" variant="dots"/>}>
                            {contacts.map((contact) => (
                                <ContactItem key={contact.id} contact={contact}/>
                            ))}
                        </React.Suspense>
                    </ul>
                </Paper>
            </ScrollArea>

        </MantineProvider>
    );
});