import React from 'react';
import './App.css';
import {BrowserRouter} from "react-router-dom";
import {Routing} from "./pages/Routing";
import {MantineProvider} from "@mantine/core";
import {NotificationsProvider} from "@mantine/notifications";

function App() {
    return (
        <BrowserRouter>
            <MantineProvider withGlobalStyles withNormalizeCSS>
                <NotificationsProvider>
                    <Routing/>
                </NotificationsProvider>
            </MantineProvider>
        </BrowserRouter>
    );
}

export default App;
