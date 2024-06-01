import {useDisclosure} from "@mantine/hooks";
import {Button, Flex, Group, Modal, Select, Space, Text} from "@mantine/core";
import {FC, useEffect, useState} from "react";
import {Account} from "../../types/account";
import ky from "ky";
import {API_URL} from "../../config";
import {getAllAccountsRequest, getChatroomRequest, InviteToChatroomRequest} from "./api";

interface InviteToChatroomButtonProps {
    chatroomId: string;
}

export const InviteToChatroomButton: FC<InviteToChatroomButtonProps> = ({chatroomId}) => {
    const [opened, {close, open}] = useDisclosure(false);
    const [accounts, setAccounts] = useState<Account[]>([]);
    const [selectedAccountId, setSelectedAccountId] = useState<string | null>(null);
    const [nonMembers, setNonMembers] = useState<string[]>([]);

    useEffect(() => {
        fetchAccounts();
        fetchNonMembers();
    }, []);

    const fetchNonMembers = async () => {
        try {
            console.log("fetching non members");
            let accounts = await getAllAccountsRequest();
            let accountIds = accounts.map((account) => account.id);
            let chatroom = await getChatroomRequest(chatroomId);

            let nonMemberIds = accountIds.filter((element: string) => !chatroom.members.includes(element));
            let nonMemberAccounts = accounts.filter((account) => nonMemberIds.includes(account.id));
            let nonMemberNames = nonMemberAccounts.map(account => account.name);

            setNonMembers(nonMemberNames);
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

    const handleInviteToChatroom = async () => {
        if (selectedAccountId) {
            try {
                console.log(`Inviting probably ${selectedAccountId} to chatroom ${chatroomId}`)
                await InviteToChatroomRequest(chatroomId, selectedAccountId);
                setSelectedAccountId(null);

            } catch (error) {

            }
        }

        await fetchNonMembers();
        close();
    };

    return (
        <>
            <Modal opened={opened} onClose={close} size="auto" title={`Invite to ${chatroomId}`}>
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
                        data={nonMembers}
                        transitionProps={{transition: 'pop-top-left', duration: 80, timingFunction: 'ease'}}
                        onChange={(value) => {
                            const selectedAccount = accounts.find((account) => account.name === value);
                            if (selectedAccount) {
                                setSelectedAccountId(selectedAccount.id)
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

                    <Button color="red" onClick={handleInviteToChatroom}>
                        Invite to chatroom
                    </Button>
                </Flex>
            </Modal>
            <Group position="center">
                <Button color="blue" onClick={open}>Invite</Button>
            </Group>
        </>
    );
}