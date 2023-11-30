import React, {FC} from 'react';
import {Button, MantineProvider, Paper, Text} from '@mantine/core';
import Cookies from "js-cookie";
import {Link} from "react-router-dom";
import {UtilityDrawer} from "../../features/drawer/UtilityDrawer";

export function removeJWTToken() {
    Cookies.remove('auth_token');
}

export const Header: FC = () => {
    return (
        <header style={{display: 'flex', justifyContent: 'center'}}>
            <MantineProvider theme={{colorScheme: 'dark'}}>
                <Paper
                    shadow="sm"
                    radius="md"
                    p="lg"
                    withBorder
                    style={{
                        width: '100%',
                        display: 'flex',
                        justifyContent: 'space-between',
                        alignItems: 'center',
                        marginBottom: '15px',
                    }}
                >
                    <UtilityDrawer/>

                    <div style={{flex: '1', textAlign: 'center'}}>
                        <Text
                            color={'#D5D7E0'}
                            sx={{
                                fontSize: 32,
                                lineHeight: 1.4,
                                fontWeight: 'bold',
                                fontFamily: '"Open Sans", sans-serif',
                            }}
                        >
                            Messegify
                        </Text>
                    </div>

                    <Link to="/login" style={{textDecoration: 'none'}}>
                        <Button variant="light" onClick={removeJWTToken}>
                            Logout
                        </Button>
                    </Link>
                </Paper>
            </MantineProvider>
        </header>
    );
};