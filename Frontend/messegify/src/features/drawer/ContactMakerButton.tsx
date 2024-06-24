import React, {FC, useEffect, useState} from 'react';
import {Button, Flex, Group, Modal, Select, Space, Text} from '@mantine/core';
import ky from 'ky';
import {API_URL} from '../../config';
import Cookies from 'js-cookie';
import {Account} from '../../types/account';
import {Contact} from '../../types/contact';
import {getUserId, getActiveContactsRequest} from "../chatroomList/api";
import {useDisclosure} from "@mantine/hooks";

export const ContactMakerButton: FC = () => {
    const [opened, {close, open}] = useDisclosure(false);
    const [accounts, setAccounts] = useState<Account[]>([]);
    const [contacts, setContacts] = useState<Contact[]>([]);
    const [userId, setUserId] = useState<string>();
    const [selectedAccountId, setSelectedAccountId] = useState<string | null>(null);

    useEffect(() => {
        fetchAccounts();
        fetchContacts();
        fetchUserId();
    }, []);

    const fetchUserId = async () => {
        try {
            const response = await getUserId();
            setUserId(response);
        } catch (error) {
            console.error('Error fetching user:', error);
        }
    };

    const fetchAccounts = async () => {
        try {
            const response = await ky.get(`${API_URL}/account`).json<Account[]>();
            setAccounts(response);
        } catch (error) {
            console.error('Error fetching accounts:', error);
        }
    };

    const fetchContacts = async () => {
        try {
            const response = await getActiveContactsRequest();
            setContacts(response);
        } catch (error) {
            console.error('Error fetching contacts:', error);
        }
    };

    const handleContactCreate = async () => {
        const token = Cookies.get('auth_token');
        const authorizedKy = ky.extend({
            headers: {
                authorization: `Bearer ${token}`,
            },
        });

        try {
            await authorizedKy.post(`${API_URL}/contact/${selectedAccountId}`);
            await fetchContacts();
        } catch (error) {
            console.error('Error creating contact:', error);
        }

        close();
    };

    const contactIds = contacts.reduce((contactedAccounts, contact) => {
        contactedAccounts.add(contact.firstAccountId);
        contactedAccounts.add(contact.secondAccountId);
        return contactedAccounts;
    }, new Set<string>());

    const filteredAccounts = accounts.filter(
        (account) =>
            !contactIds.has(account.id) &&
            account.id !== userId
    );

    return (
        <>
            <Modal opened={opened} onClose={close} size="auto" title="Add a new contact">
                <Flex
                    justify="center"
                    align="center"
                    direction="column"
                >
                    <Select
                        size="md"
                        searchable
                        clearable
                        maw={320}
                        mx="auto"
                        placeholder="Contact's name"
                        nothingFound="No available contacts"
                        data={filteredAccounts.map((account) => account.name)}
                        transitionProps={{transition: 'pop-top-left', duration: 80, timingFunction: 'ease'}}
                        onChange={(value) => {
                            const selectedAccount = accounts.find((account) => account.name === value);
                            if (selectedAccount) {
                                setSelectedAccountId(selectedAccount.id);
                            }
                        }}
                    />

                    <Space h="xl"/>
                    <Text>Add this account to your contacts</Text>
                    <Space h="xl"/>
                    <Text>This will cause you to be</Text>
                    <Text>in their contacts as well</Text>
                    <Space h="xl"/>

                    <Button color="green" onClick={handleContactCreate}>
                        Add to contacts
                    </Button>
                </Flex>
            </Modal>
            <Group position="center">
                <Button color="green" size="md" onClick={open}>Add a new contact</Button>
            </Group>
        </>
    );
};