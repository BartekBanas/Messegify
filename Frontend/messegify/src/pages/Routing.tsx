import {MenuPage} from "./MenuPage";
import {Center} from "./layout/Center";
import {LoginPage} from "./LoginPage";
import {Navigate, useRoutes} from "react-router-dom";
import {Main} from "./layout/Main";
import {FC} from "react";
import useIsLogged from "../hooks/useIsLogged";
import {RegisterPage} from "./RegisterPage";
import {AuthenticationErrorNotification} from "./RoutingNotifications";
import {ChatRoomPage} from "./ChatRoomPage";

const publicRoutes = [
    {
        path: "/",
        element: <Center/>,
        children: [
            {
                path: '/login',
                element: <LoginPage/>
            },
            {
                path: '/register',
                element: <RegisterPage/>
            },
            // {
            //     path: '/menu',
            //     element: <MenuPage/>
            // },
            {
                path: "*",
                AuthenticationErrorNotification,
                element: <Navigate to="/login" replace/>
            },
            {
                path: "",
                AuthenticationErrorNotification,
                element: <Navigate to="/login" replace/>
            }
        ]
    }
];

const privateRoutes = [
    {
        path: '/',
        element: <Main/>,
        children: [
            {
                path: '/menu',
                element: <MenuPage/>
            },
            {
                path: '/chatroom/*',
                element: <ChatRoomPage/>
            },
            {
                path: '*',
                element: <Navigate to="/Menu" replace/>
            }
        ]
    }
]

export const Routing: FC = function () {
    const isLogged = useIsLogged();

    const routes = isLogged() ? privateRoutes : publicRoutes

    if (!isLogged) {
        AuthenticationErrorNotification();
    }

    return useRoutes(routes);
};