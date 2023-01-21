import {MenuPage} from "./MenuPage";
import {Center} from "./layout/Center";
import {LoginPage} from "./LoginPage";
import {Navigate, useRoutes} from "react-router-dom";
import {Main} from "./layout/Main";
import {ErrorPage} from "./ErrorPage";
import {FC} from "react";

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
  return useRoutes(publicRoutes);
};