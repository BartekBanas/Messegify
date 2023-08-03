import React from 'react';
import './App.css';
import {BrowserRouter} from "react-router-dom";
import {Routing} from "./pages/Routing";
import {MantineProvider} from "@mantine/core";
import {Notifications} from '@mantine/notifications';
import {SpotlightProvider} from "@mantine/spotlight";

function App() {
    return (
        <MantineProvider theme={{colorScheme: 'dark'}}>
            <BrowserRouter>
                <MantineProvider withGlobalStyles withNormalizeCSS>
                    <Notifications/>
                    <SpotlightProvider actions={[]}>
                        <Routing/>
                    </SpotlightProvider>
                </MantineProvider>
            </BrowserRouter>
        </MantineProvider>
    );
}

export default App;
