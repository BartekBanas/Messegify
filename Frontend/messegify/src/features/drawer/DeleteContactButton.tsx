import {useDisclosure} from "@mantine/hooks";
import {contactDeletionErrorNotification, contactDeletionSuccessNotification} from "./notifications";
import {Button, Flex, Group, Modal, Select, Space, Text} from "@mantine/core";
import {useEffect, useState} from "react";
import {Account} from "../../types/account";
import {Contact} from "../../types/contact";
import {getUserId, getContactsRequest} from "../chatroomList/api";
import {API_URL} from "../../config";
import ky from "ky";
import {deleteContactRequest} from "./api";

export function DeleteContactButton() {
    const [opened, {close, open}] = useDisclosure(false);
    const [accounts, setAccounts] = useState<Account[]>([]);
    const [contacts, setContacts] = useState<Contact[]>([]);
    const [userId, setUserId] = useState<string>();
    const [selectedContactId, setSelectedContactId] = useState<string | null>(null);

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
            const response = await getContactsRequest();
            setContacts(response);
        } catch (error) {
            console.error('Error fetching contacts:', error);
        }
    };

    const contactNames = contacts.map((contact) => {
        if (contact.firstAccountId === userId) {
            return accounts.find((account) => account.id === contact.secondAccountId)?.name || '';
        }
        return accounts.find((account) => account.id === contact.firstAccountId)?.name || '';
    });

    const handleDeleteContact = async () => {
        if (selectedContactId) {
            try {
                await deleteContactRequest(selectedContactId);
                setSelectedContactId(null);
                contactDeletionSuccessNotification();

            } catch (error) {
                contactDeletionErrorNotification();
            }
        }

        close();
    };

    return (
        <>
            <Modal opened={opened} onClose={close} size="auto" title="Delete contact">
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
                        data={contactNames}
                        transitionProps={{transition: 'pop-top-left', duration: 80, timingFunction: 'ease'}}
                        onChange={(value) => {
                            const selectedAccount = accounts.find((account) => account.name === value);
                            if (selectedAccount) {
                                const selectedContact = contacts.find((contact) =>
                                    contact.firstAccountId === selectedAccount.id || contact.secondAccountId === selectedAccount.id);

                                if (selectedContact) {
                                    setSelectedContactId(selectedContact.id)
                                }
                            }
                        }}
                    />

                    <Space h="xl"/>
                    <Text>This action cannot be reversed</Text>
                    <Space h="xl"/>
                    <Text>This will not prevent you</Text>
                    <Text>from becoming contacts again</Text>
                    <Text>All your messages will be lost</Text>
                    <Space h="xl"/>
                    <Space h="xl"/>

                    <Button color="red" onClick={handleDeleteContact}>
                        Delete Contact
                    </Button>
                </Flex>
            </Modal>
            <Group position="center">
                <Button color="yellow" size="md" onClick={open}>Delete Contact</Button>
            </Group>
        </>
    );
}