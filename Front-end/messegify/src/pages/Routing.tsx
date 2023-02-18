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
import Cookies from "js-cookie";

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

    const token = Cookies.get('auth_token');
    console.log("outside: ", token)

    const isLogged = useIsLogged();

    const routes = isLogged() ? privateRoutes : publicRoutes
    //const routes = privateRoutes

    // console.log('routes')
    // console.log(routes)
    // console.log(publicRoutes)
    // console.log(privateRoutes)

    if (!isLogged) {
        console.log("Ooops an error")
        AuthenticationErrorNotification();
    }

    return useRoutes(routes);
};