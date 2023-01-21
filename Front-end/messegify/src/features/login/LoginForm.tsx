import {useForm} from "@mantine/form";
import {FC} from "react";
import {LoginFormType} from "./login-form.types";
import {Stack, TextInput, Button} from "@mantine/core";

export const LoginForm: FC = () => {
    const form = useForm<LoginFormType>({
        initialValues: {
            email: '',
            password: ''
        },
    })

    async function handleSubmit(data: LoginFormType) {
        
    }

    return (
        <form onSubmit={form.onSubmit(values => handleSubmit(values))}>
            <Stack spacing="md">
                <TextInput required type={"email"} label="Email" {...form.getInputProps('email')}/>
                <TextInput required type={"password"} label="Password" {...form.getInputProps('password')}/>
                <Button type="submit">Login</Button>
            </Stack>
        </form>
    );
};