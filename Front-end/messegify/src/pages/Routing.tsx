import {MenuPage} from "./MenuPage";
import {Center} from "./layout/Center";
import {LoginPage} from "./LoginPage";
import {Navigate, useRoutes} from "react-router-dom";
import {Main} from "./layout/Main";
import {ErrorPage} from "./ErrorPage";
import {FC} from "react";
import useIsLogged from "../hooks/useIsLogged";

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
                path: "*",
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
                path: '/',
                element: <MenuPage/>
            },
            {
                path: '*',
                element: <ErrorPage/>
            }
        ]
    }
]

export const Routing: FC = () => {
    const isLogged = useIsLogged();

    try {
        // eslint-disable-next-line @typescript-eslint/no-unused-expressions
        isLogged;
        const routes = publicRoutes;
    }   catch (error) {
        const routes = privateRoutes;
    }


  return useRoutes(publicRoutes);
};