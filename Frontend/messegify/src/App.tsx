import React from 'react';
import './App.css';
import {BrowserRouter} from "react-router-dom";
import {Routing} from "./pages/Routing";
import {MantineProvider} from "@mantine/core";
import {NotificationsProvider} from "@mantine/notifications";
import {SpotlightProvider} from "@mantine/spotlight";

function App() {
    return (
        <MantineProvider theme={{colorScheme: 'dark'}}>
            <BrowserRouter>
                <MantineProvider withGlobalStyles withNormalizeCSS>
                    <SpotlightProvider actions={[]}>
                        <NotificationsProvider>
                            <Routing/>
                        </NotificationsProvider>
                    </SpotlightProvider>
                </MantineProvider>
            </BrowserRouter>
        </MantineProvider>
    );
}

export default App;
