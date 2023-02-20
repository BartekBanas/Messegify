import React, {FC} from 'react';
import {MantineProvider, Paper, Text} from "@mantine/core";


export const Header: FC = () => {
    return (
        <header style={{display: 'flex', justifyContent: 'center'}}>
            <MantineProvider theme={{colorScheme: 'dark'}}>
                <Paper shadow="sm" radius="md" p="lg" withBorder
                       style={{width: '100%', display: 'flex', justifyContent: 'center'}}>
                    <Text color={'#D5D7E0'} sx={{
                        fontSize: 32,
                        lineHeight: 1.4,
                        fontWeight: 'bold',
                        fontFamily: '"Open Sans", sans-serif'
                    }}>
                        Messegify
                    </Text>
                </Paper>
            </MantineProvider>
        </header>
    );
};