import React, {FC} from 'react';
import {useNavigate} from 'react-router-dom';
import {Group} from '@mantine/core';
import {ContactList} from '../contactList/ContactList';

interface MenuFormProps {
}

export const MenuForm: FC<MenuFormProps> = () => {
    const navigate = useNavigate();

    return (
        <Group>
            <div>
                <ContactList/>
            </div>
        </Group>
    );
};