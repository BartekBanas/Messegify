import {useForm} from "@mantine/form";
import {FC} from "react";
import {LoginFormType} from "./login-form.types";
import {Stack, TextInput, Button} from "@mantine/core";
import {login} from "./api";
import {loginErrorNotification} from "./notifications";
import {useNavigate} from "react-router-dom";

export const LoginForm: FC = () => {
    const navigate = useNavigate();
    const form = useForm<LoginFormType>({
        initialValues: {
            email: '',
            password: ''
        },
    })

    async function handleSubmit(data: LoginFormType) {
        try {
            await login(data.email, data.password);


            navigate('/menu');
        } catch (error) {
            loginErrorNotification();
        }
    }

    return (
        <form onSubmit={form.onSubmit(values => handleSubmit(values))}>
            <Stack spacing="md">
                <TextInput required type="email" label="Email" {...form.getInputProps('email')}/>
                <TextInput required type="password" label="Password" {...form.getInputProps('password')}/>
                <Button type="submit">Login</Button>
            </Stack>
        </form>
    );
};