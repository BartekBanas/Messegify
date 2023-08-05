import React, {FC, useEffect, useState} from 'react';
import {MantineProvider, Select} from '@mantine/core';
import ky from 'ky';
import {API_URL} from '../../../../config';
import Cookies from 'js-cookie';
import {Account} from '../../../../types/account';
import {Contact} from '../../../../types/contact';
import {listContacts} from '../../../menu/api';

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
            fetchContacts();
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
            <Select
                searchable
                clearable
                maw={320}
                mx="auto"
                label="Add a new contact"
                placeholder="Contact's name"
                nothingFound="No available contacts"
                data={filteredAccounts.map((account) => account.name)}
                transitionProps={{transition: 'pop-top-left', duration: 80, timingFunction: 'ease'}}
                onChange={(value) => {
                    const selectedAccount = accounts.find((account) => account.name === value);
                    if (selectedAccount) {
                        handleContactCreate(selectedAccount.id);
                    }
                }}
            />
        </MantineProvider>
    );
};