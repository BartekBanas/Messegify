import React, {FC, useEffect, useState} from 'react';
import {Contact} from "../../types/contact";
import {listContacts} from "./api";
import {ContactItem} from "../chatroom/ContactItem";

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
        <ul>
            <React.Suspense fallback={<div>Loading...</div>}>
                {contacts.map((contact) => (
                    <ContactItem key={contact.id} contact={contact}/>
                ))}
            </React.Suspense>
        </ul>
    );
});