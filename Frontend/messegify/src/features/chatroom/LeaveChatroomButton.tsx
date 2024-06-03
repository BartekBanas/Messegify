import {useDisclosure} from "@mantine/hooks";
import {Button, Group, Modal, Text} from "@mantine/core";
import React, {useEffect, useState} from "react";
import {getChatroomRequest, leaveChatroomRequest} from "./api";
import {useLocation} from "react-router-dom";
import {Chatroom} from "../../types/chatroom";
import {leaveChatroomErrorNotification, leaveChatroomSuccessNotification} from "./notifications";

export const LeaveChatroomButton = () => {
    const [opened, {close, open}] = useDisclosure(false);
    const [chatroom, setChatroom] = useState<Chatroom | null>(null);
    const chatroomId = window.location.href.split('/').pop() ?? '';
    const location = useLocation();

    useEffect(() => {
        getChatroomRequest(chatroomId).then((response: Chatroom) => {
            setChatroom(response);
        }).catch((error) => {
            console.error(`Error fetching chatroom ${chatroomId}:`, error);
        });
    }, [location]);

    function handleSubmit() {
        try {
            leaveChatroomRequest(chatroomId);
            leaveChatroomSuccessNotification();
        }
        catch (error) {
            leaveChatroomErrorNotification();
        }

        close();
    }

    return (
        <>
            <Modal opened={opened} onClose={close} size="auto" title={`Leave chatroom`}>
                <Text>Are you sure you want to leave {chatroom?.name}?</Text>

                <Group mt="xl" position="center">
                    <Button color="gray" onClick={close}>
                        No, don't leave
                    </Button>
                    <Button color="red" onClick={handleSubmit}>
                        Leave chatroom
                    </Button>
                </Group>
            </Modal>
            <Group position="center">
                <Button color="red" onClick={open}>Leave</Button>
            </Group>
        </>
    );
}