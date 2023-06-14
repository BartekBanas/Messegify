import React, {FC} from 'react';
import {MenuForm} from "../features/menu/MenuForm";

interface MenuPageProps {
}

export const MenuPage: FC<MenuPageProps> = () => {
    return (
        <MenuForm/>
    );
};