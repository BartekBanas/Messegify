import React, {FC} from 'react';
import {useNavigate} from 'react-router-dom';
import {Group} from '@mantine/core';
import {ChatroomList} from '../chatroomList/ChatroomList';

interface MenuFormProps {
}

export const MenuForm: FC<MenuFormProps> = () => {
    const navigate = useNavigate();

    return (
        <Group>
            <div>
                <ChatroomList/>
            </div>
        </Group>
    );
};