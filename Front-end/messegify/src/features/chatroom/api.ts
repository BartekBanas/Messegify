import Cookies from "js-cookie";
import ky from "ky";
import {API_URL} from "../../config";
import {Message} from "../../types/message";
import {useState} from "react";
import useWebSocket from "react-use-websocket";

export function useGetMessages() {
    const currentUrl = window.location.href;
    const roomId = currentUrl.split('/').pop();
    const endpoint = new URL('/board/all', API_URL).href

    const token = Cookies.get('auth_token');
    const authorizedKy = ky.extend({
        headers: {
            authorization: `Bearer ${token}`
        }
    });

    return authorizedKy.get(`${API_URL}/chatRoom/${roomId}/message/list`).json<Message[]>();
}

export function useMessageWebSocket() {
    const currentUrl = window.location.href;
    const roomId = currentUrl.split('/').pop();
    const apiUri = process.env.REACT_APP_API_URL;
    const hostname = new URL('/chatRoom/' + {roomId} + '/message', apiUri).host;
    const endpoint = `wss://${hostname}/ws`;
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