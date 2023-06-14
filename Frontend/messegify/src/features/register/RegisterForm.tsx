import {useForm} from "@mantine/form";
import {FC} from "react";
import {Stack, TextInput, Button, MantineProvider, Text, Paper} from "@mantine/core";
import {useNavigate} from "react-router-dom";
import {register} from "./api";
import {RegisterFormType} from "./register-form.type";
import {RegisterErrorNotification} from "./notification";
import '../../pages/layout/DarkBackground.css'

export const RegisterForm: FC = () => {
    const navigate = useNavigate();
    const form = useForm<RegisterFormType>({
        initialValues: {
            Username: '',
            Password: '',
            Email: ''
        },
    })

    async function handleSubmit(data: RegisterFormType) {
        try {
            await register(data.Username, data.Password, data.Email);

            
            navigate('/login');
        } catch (error) {
            RegisterErrorNotification();
        }
    }

    return (
        <MantineProvider theme={{colorScheme: 'dark'}}>
            <div>
                <div style={{marginBottom: "30px"}}>
                    <Paper shadow="sm" radius="md" p="lg">
                        <Text color={'#D5D7E0'} sx={{
                            fontSize: 32,
                            lineHeight: 1.4,
                            fontWeight: 'bold',
                            fontFamily: '"Open Sans", sans-serif'
                        }}>
                            Registration Page
                        </Text>
                    </Paper>
                </div>
                <Paper shadow="sm" radius="md" p="lg" withBorder>
                    <form onSubmit={form.onSubmit(values => handleSubmit(values))}>
                        <Stack spacing="md">
                            <TextInput required type="username" label="Username" {...form.getInputProps('Username')}/>
                            <TextInput required type="password" label="Password" {...form.getInputProps('Password')}/>
                            <TextInput required type="email" label="Email" {...form.getInputProps('Email')}/>
                            <Button type="submit">Register</Button>
                        </Stack>
                    </form>
                </Paper>
            </div>
        </MantineProvider>
    );
};