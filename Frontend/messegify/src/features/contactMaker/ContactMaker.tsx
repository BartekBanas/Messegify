import React, {FC, useEffect, useState} from 'react';
import {TextInput, MantineProvider, Paper, Button} from '@mantine/core';
import ky from 'ky';
import {API_URL} from '../../config';
import Cookies from 'js-cookie';
import {Account} from "../../types/account";
import {Contact} from "../../types/contact";
import {listContacts} from "../menu/api";

export const ContactMaker: FC = () => {
    const [accounts, setAccounts] = useState<Account[]>([]);
    const [contacts, setContacts] = useState<Contact[]>([]);
    const [searchQuery, setSearchQuery] = useState<string>('');

    useEffect(() => {
        fetchAccounts();
        fetchContacts();
    }, []);

    const fetchAccounts = async () => {
        try {
            const response = await ky.get(`${API_URL}/account`).json<Account[]>();
            setAccounts(response);
        } catch (error) {
            console.error('Error fetching accounts:', error);
        }
    };

    const handleContactCreate = async (accountId: string) => {
        const token = Cookies.get('auth_token');
        const authorizedKy = ky.extend({
            headers: {
                authorization: `Bearer ${token}`,
            },
        });

        try {
            await authorizedKy.post(`${API_URL}/contact/${accountId}`);
        } catch (error) {
            console.error('Error creating contact:', error);
        }
    };

    const fetchContacts = async () => {
        try {
            const response = await listContacts();
            setContacts(response);
        } catch (error) {
            console.error('Error fetching contacts:', error);
        }
    };

    const contactIds = contacts.reduce((contactedAccounts, contact) => {
        contactedAccounts.add(contact.firstAccountId);
        contactedAccounts.add(contact.secondAccountId);
        return contactedAccounts;
    }, new Set<string>());

    const filteredAccounts = accounts.filter(
        (account) =>
            account.name.toLowerCase().includes(searchQuery.toLowerCase()) &&
            !contactIds.has(account.id)
    );

    return (
        <MantineProvider theme={{colorScheme: 'dark'}}>
            <Paper shadow="sm" radius="md" p="lg" withBorder>
                <TextInput
                    placeholder="Add a new contact..."
                    value={searchQuery}
                    onChange={(event) => setSearchQuery(event.currentTarget.value)}
                    style={{marginBottom: '1rem'}}
                />
                {searchQuery.length > 0 && (
                    <ul>
                        {filteredAccounts.map((account) => (
                            <li key={account.id} style={{marginBottom: '0.5rem'}}>
                                <Button variant="light" onClick={() => handleContactCreate(account.id)}>
                                    {account.name}
                                </Button>
                            </li>
                        ))}
                    </ul>
                )}
            </Paper>
        </MantineProvider>
    );
};