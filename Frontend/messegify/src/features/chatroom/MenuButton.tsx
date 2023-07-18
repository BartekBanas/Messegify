import React, {FC} from 'react';
import {Link} from "react-router-dom";
import {Button} from "@mantine/core";

export const MenuButton: FC = () => {
    return (
        <div>
            <Link to="/menu">
                <Button variant="gradient"
                        gradient={{from: 'teal', to: 'lime', deg: 105}}>
                    Menu
                </Button>
            </Link>
        </div>
    );
};