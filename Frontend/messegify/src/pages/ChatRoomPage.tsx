import React, {FC} from 'react';
import {ChatroomForm} from "../features/chatroom/ChatroomForm";

interface ChatRoomPageProps {
}

export const ChatRoomPage: FC<ChatRoomPageProps> = ({}) => {
    return (
        <ChatroomForm/>
    );
};