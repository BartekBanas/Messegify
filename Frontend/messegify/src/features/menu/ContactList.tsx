import React, {FC, useEffect} from 'react';
import {Contact} from "../../types/contact";
import {listContacts} from "./api";
import {ContactItem} from "../chatroom/ContactItem";

export const ContactList: FC = React.memo(() => {
    const [contacts, setContacts] = React.useState<Contact[]>([]);

    useEffect(() => {
        listContacts().then((data) => setContacts(data));
    }, []);

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