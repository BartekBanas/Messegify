import {useState} from "react";
import {Contact} from "../../types/contact";

export function useGetMessages() {
    const [messages, setMessages] = useState<Contact[]>([]);


    return messages;
}