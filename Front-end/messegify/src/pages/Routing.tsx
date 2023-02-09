import {MenuPage} from "./MenuPage";
import {Center} from "./layout/Center";
import {LoginPage} from "./LoginPage";
import {Navigate, useRoutes} from "react-router-dom";
import {Main} from "./layout/Main";
import {ErrorPage} from "./ErrorPage";
import {FC} from "react";
import useIsLogged from "../hooks/useIsLogged";
import {RegisterPage} from "./RegisterPage";
import {AuthenticationErrorNotification} from "./RoutingNotifications";

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
                element: <Navigate to="/login" replace/>
            }
        ]
    }
];

const privateRoutes = [
    {
        path: '/',
        element: <Center/>,
        children: [
            {
                path: '/menu',
                element: <MenuPage/>
            },
            // {
            //     path: '/login',
            //     element: <LoginPage/>
            // },
            {
                path: '*',
                element: <Navigate to="/Menu" replace/>
                //element: <ErrorPage/>
            }
        ]
    }
]

export const Routing: FC = () => {
    const isLogged = useIsLogged();

    const routes = isLogged ? privateRoutes : publicRoutes
    //const routes = privateRoutes

    console.log('routes')
    console.log(routes)
    console.log(publicRoutes)
    console.log(privateRoutes)

    if (!isLogged) {
        AuthenticationErrorNotification();
    }

    return useRoutes(routes);
};