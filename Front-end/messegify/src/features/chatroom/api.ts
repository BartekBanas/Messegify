import {useState} from "react";
import Cookies from "js-cookie";
import ky from "ky";
import {API_URL} from "../../config";
import {Message} from "../../types/message";

export function useGetMessages() {
    const [messages, setMessages] = useState<Message[]>([]);

    const currentUrl = window.location.href;
    const roomId = currentUrl.split('/').pop();
    console.log(roomId);


    const token = Cookies.get('auth_token');
    const authorizedKy = ky.extend({
        headers: {
            authorization: `Bearer ${token}`
        }
    });

    return authorizedKy.get(`${API_URL}/chatRoom/${roomId}/message/list`).json<Message[]>();
}