import React, {FC} from 'react';
import {useNavigate} from "react-router-dom";
import {Group} from "@mantine/core";
import {ContactList} from "./ContactList";
import {ContactMaker} from "../contactMaker/AccountSelector";

interface MenuFormProps {
}

export const MenuForm: FC<MenuFormProps> = () => {
    const navigate = useNavigate();

    return (
        <Group>
            <ContactList/>
            <ContactMaker/>
        </Group>
    );
};