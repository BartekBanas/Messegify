import Cookies from "js-cookie";
import ky from "ky";
import {API_URL} from "../../config";
import {Message} from "../../types/message";
import {useState} from "react";
import useWebSocket from "react-use-websocket";
import {sendMessageErrorNotification} from "./notifications";
import {Account} from "../../types/account";
import {Chatroom} from "../../types/chatroom";

export function useGetMessages() {
    const currentUrl = window.location.href;
    const roomId = currentUrl.split('/').pop();

    const token = Cookies.get('auth_token');
    const authorizedKy = ky.extend({
        headers: {
            authorization: `Bearer ${token}`
        }
    });

    return authorizedKy.get(`${API_URL}/chatRoom/${roomId}/message`).json<Message[]>();
}

export function useMessageWebSocket() {
    const currentUrl = window.location.href;
    const roomId = currentUrl.split('/').pop();
    const endpoint = `wss://${API_URL}`;
    const [socketUrl,] = useState(endpoint);

    const {
        sendMessage,
        lastMessage,
        readyState,
        getWebSocket,
        lastJsonMessage,
        sendJsonMessage
    } = useWebSocket(socketUrl);

    return {
        sendMessage,
        lastMessage,
        readyState,
        getWebSocket,
        lastJsonMessage,
        sendJsonMessage
    }
}

export async function handleSubmit(data: Message, roomId: string) {
    try {
        const token = Cookies.get('auth_token');
        const authorizedKy = ky.extend({
            headers: {
                'Content-Type': 'application/json',
                'Access-Control-Allow-Origin': '*',
                authorization: `Bearer ${token}`
            },
            body: JSON.stringify({
                textContent: data.textContent
            })
        });

        return authorizedKy.post(`${API_URL}/chatRoom/${roomId}/message`).json<Message[]>();
    } catch (error) {
        sendMessageErrorNotification();
    }
}

export async function InviteToChatroomRequest(chatroomId: string, accountId: string) {
    const token = Cookies.get('auth_token');
    const kyInstance = ky.extend({
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*',
            authorization: `Bearer ${token}`
        },
        body: JSON.stringify({
            ChatroomId: chatroomId,
            AccountId: accountId,
        })
    });

    return kyInstance.post(`${API_URL}/chatroom/invite`);
}

export async function getAllAccountsRequest(): Promise<Account[]> {
    const token = Cookies.get('auth_token');
    const authorizedKy = ky.extend({
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*',
            authorization: `Bearer ${token}`
        }
    });

    return authorizedKy.get(`${API_URL}/account`).json<Account[]>();
}

export async function getChatroomRequest(chatroomId: string): Promise<Chatroom> {
    const token = Cookies.get('auth_token');
    const authorizedKy = ky.extend({
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*',
            authorization: `Bearer ${token}`
        }
    });

    return authorizedKy.get(`${API_URL}/chatroom/${chatroomId}`).json<Chatroom>();
}

export async function createChatroomRequest(chatroomName: string) {
    const token = Cookies.get('auth_token');
    const authorizedKy = ky.extend({
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*',
            authorization: `Bearer ${token}`
        },
        body: JSON.stringify({
            Name: chatroomName,
        })
    });

    return authorizedKy.post(`${API_URL}/chatroom`);
}