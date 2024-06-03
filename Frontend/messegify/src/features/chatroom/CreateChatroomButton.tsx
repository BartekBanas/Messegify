import {useDisclosure} from "@mantine/hooks";
import {Button, Flex, Group, Modal, Stack, TextInput} from "@mantine/core";
import React from "react";
import {createChatroomRequest} from "./api";
import {useForm} from "@mantine/form";
import {createChatroomErrorNotification, createChatroomSuccessNotification} from "./notifications";
import {useNavigate} from "react-router-dom";
import {Chatroom} from "../../types/chatroom";

export const CreateChatroomButton = () => {
    const [opened, {close, open}] = useDisclosure(false);
    const navigate = useNavigate();
    const form = useForm({
        initialValues: {
            chatroomId: '',
        },
    });

    async function handleSubmit(chatroomName: string) {
        try {
            let response = await createChatroomRequest(chatroomName);
            let createdChatroom : Chatroom = await response.json();
            createChatroomSuccessNotification();
            navigate(`/chatroom/${createdChatroom.id}`);
        }
        catch (error) {
            createChatroomErrorNotification();
        }

        close();
    }

    return (
        <>
            <Modal opened={opened} onClose={close} size="auto" title={`Create a new chatroom`}>
                <Flex
                    justify="center"
                    align="center"
                    direction="column"
                >
                    <form onSubmit={form.onSubmit(input => handleSubmit(input.chatroomId))}>
                        <Stack spacing="md">
                            <TextInput {...form.getInputProps('chatroomId')}/>
                            <Button type="submit">Done</Button>
                        </Stack>
                    </form>
                </Flex>
            </Modal>
            <Group position="center">
                <Button color="blue" onClick={open}>Create</Button>
            </Group>
        </>
    );
}