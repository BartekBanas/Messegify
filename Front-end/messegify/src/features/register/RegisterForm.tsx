import {useForm} from "@mantine/form";
import {FC} from "react";
import {Stack, TextInput, Button} from "@mantine/core";
import {useNavigate} from "react-router-dom";
import {register} from "./api";
import {RegisterFormType} from "./register-form.type";
import {RegisterErrorNotification} from "./notification";
import {Paper} from "@mantine/core";
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
            await register(data.Email, data.Password, data.Email);

            navigate('/login');
        } catch (error) {
            RegisterErrorNotification();
        }
    }

    return (
        <div>
            <Paper shadow="sm" radius="md" p="lg" withBorder className="dark-gray-bg">
                <form onSubmit={form.onSubmit(values => handleSubmit(values))}>
                    <Stack spacing="md">
                        <TextInput required type="username" label="Username" {...form.getInputProps('')}/>
                        <TextInput required type="password" label="Password" {...form.getInputProps('Password')}/>
                        <TextInput required type="email" label="Email" {...form.getInputProps('Email')}/>
                        <Button type="submit">Register</Button>
                    </Stack>
                </form>
            </Paper>
        </div>
    );
};