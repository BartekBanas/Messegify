import {useDisclosure} from "@mantine/hooks";
import {Button, Flex, Group, Modal, Stack, TextInput} from "@mantine/core";
import React from "react";
import {createChatroomRequest} from "./api";
import {useForm} from "@mantine/form";

interface InviteToChatroomButtonProps {
    chatroomId: string;
}

export const CreateChatroomButton = () => {
    const [opened, {close, open}] = useDisclosure(false);
    const form = useForm<InviteToChatroomButtonProps>({})

    function handleSubmit(chatroomName: string) {
        try {
            createChatroomRequest(chatroomName);
        }
        catch (error) {
            console.error('Error creating chatroom:', error);
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
                    <form onSubmit={form.onSubmit(value => handleSubmit(value.chatroomId))}>
                        <Stack spacing="md">
                            <TextInput required type="" label="" {...form.getInputProps('chatroomId')}/>
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