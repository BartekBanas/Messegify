import {MenuPage} from "./MenuPage";
import {ChatRoomPage} from "./ChatRoomPage";
import {Center} from "./layout/Center";
import {LoginPage} from "./LoginPage";
import {Navigate} from "react-router-dom";
import {Main} from "./layout/Main";

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
        ]
    }
]