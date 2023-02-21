import React, {FC, useState} from 'react';
import {_Text} from "@mantine/core/lib/Text/Text";
import {useGetMessages} from "./api";
import {Message} from "../../types/message";
import {Contact} from "../../types/contact";
import {Group, MantineProvider} from "@mantine/core";
import {ContactList} from "../menu/ContactList";


export const ChatroomForm: FC = () => {

    const [messages, setMessages] = useState<Message[]>([]);
    const getMessages = useGetMessages();

    async function recieveMessages() {
        const recievedMessages = await getMessages;

        setMessages(recievedMessages);

        console.log(messages);
    }


    return (
        <Group>
            <ContactList/>
            
        </Group>
    );
};