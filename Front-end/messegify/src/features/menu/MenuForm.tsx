import React, {FC} from 'react';
import {useNavigate} from "react-router-dom";
import {Button, MantineProvider, Paper, Stack, Text, TextInput} from "@mantine/core";

interface MenuFormProps {
}

export const MenuForm: FC<MenuFormProps> = () => {
    const navigate = useNavigate();


    return (
        <MantineProvider theme={{colorScheme: 'dark'}}>
            <div>
                <Paper shadow="sm" radius="md" p="lg" withBorder>
                    <div style={{marginBottom: "30px"}}>
                        <Paper shadow="sm" radius="md" p="lg">
                            <Text color={'#D5D7E0'} sx={{
                                fontSize: 32,
                                lineHeight: 1.4,
                                fontWeight: 'bold',
                                fontFamily: '"Open Sans", sans-serif'
                            }}>
                                Menu Page
                            </Text>
                        </Paper>
                    </div>
                </Paper>
            </div>

        </MantineProvider>
    );
};