import React, {FC} from 'react';
import classes from "./Center.module.css";
import {Outlet} from "react-router-dom";
import {Content} from "./Content";
import {createStyles} from "@mantine/core";
import {Header} from "./Header";

const useStyles = createStyles(() => ({
    rootContainer: {
        display: 'grid',
        gridTemplateRows: 'auto 1fr',
        height: '100vh',
    }
}))

export const Main: FC = ({}) => {
    const {classes} = useStyles();

    return (
        <div className={classes.rootContainer}>
            <Header/>
            <Content>
                <Outlet/>
            </Content>
        </div>
    );
};