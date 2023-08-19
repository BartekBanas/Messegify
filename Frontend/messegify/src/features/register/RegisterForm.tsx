import {useForm} from "@mantine/form";
import React, {FC} from "react";
import {Stack, TextInput, Button, MantineProvider, Text, Paper, PasswordInput} from "@mantine/core";
import {useNavigate} from "react-router-dom";
import {register} from "./api";
import {RegisterFormType} from "./register-form.type";
import {RegisterErrorNotification, RegisterSuccessNotification} from "./notifications";
import '../../pages/layout/DarkBackground.css'

export const RegisterForm: FC = () => {
    const navigate = useNavigate();
    const form = useForm<RegisterFormType>({
        initialValues: {
            Name: '',
            Password: '',
            Email: ''
        },
    })

    async function handleSubmit(data: RegisterFormType) {
        console.log(data)
        try {
            await register(data.Name, data.Password, data.Email);
            RegisterSuccessNotification();
            navigate("/login");
        } catch (error: any) {
            console.log(error.message)
            RegisterErrorNotification(error.message);
        }
    }

    return (
        <MantineProvider theme={{colorScheme: 'dark'}}>
            <div>
                <div style={{marginBottom: "30px"}}>
                    <Paper shadow="sm" radius="md" p="lg" style={{textAlign: 'center', width: '300px'}}>
                        <Text color={'#D5D7E0'} sx={{
                            fontSize: 32,
                            lineHeight: 1.4,
                            fontWeight: 'bold',
                            fontFamily: '"Open Sans", sans-serif'
                        }}>
                            Messegify
                        </Text>
                    </Paper>
                </div>
                <Paper shadow="sm" radius="md" p="lg" withBorder>
                    <form onSubmit={form.onSubmit(values => handleSubmit(values))}>
                        <Stack spacing="md">
                            <TextInput required type="username" label="Username" {...form.getInputProps('Name')}/>
                            <PasswordInput label="Password" withAsterisk {...form.getInputProps('Password')}/>
                            <TextInput required type="email" label="Email" {...form.getInputProps('Email')}/>
                            <Button type="submit">Register</Button>
                        </Stack>
                    </form>
                </Paper>
            </div>
        </MantineProvider>
    );
};