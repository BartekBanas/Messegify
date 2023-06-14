import React, {FC} from 'react';
import {useNavigate} from "react-router-dom";
import {Button, Group, MantineProvider, Paper, Stack, Text, TextInput} from "@mantine/core";
import {ContactList} from "./ContactList";

interface MenuFormProps {
}

export const MenuForm: FC<MenuFormProps> = () => {
    const navigate = useNavigate();

    return (
        <Group>
            <ContactList/>
            <MantineProvider theme={{colorScheme: 'dark'}}>
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
            </MantineProvider>
            <MantineProvider theme={{colorScheme: 'dark'}}>
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
            </MantineProvider>
        </Group>
    );
};