import {useForm} from "@mantine/form";
import {FC} from "react";
import {LoginFormType} from "./login-form.types";
import {Stack, TextInput, Button, MantineProvider} from "@mantine/core";
import {login} from "./api";
import {loginErrorNotification} from "./notifications";
import {useNavigate} from "react-router-dom";
import {Paper} from "@mantine/core";
import {Text} from "@mantine/core";
//import '../../pages/layout/DarkBackground.css'

export const LoginForm: FC = () => {
    const navigate = useNavigate();
    const form = useForm<LoginFormType>({
        initialValues: {
            UsernameOrEmail: '',
            Password: ''
        },
    })

    async function handleSubmit(data: LoginFormType) {
        try {
            await login(data.UsernameOrEmail, data.Password);

            navigate('/menu');
        } catch (error) {
            loginErrorNotification();
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
                            Login Page
                        </Text>
                    </Paper>
                </div>

                <Paper shadow="sm" radius="md" p="lg" withBorder>
                    <form onSubmit={form.onSubmit(values => handleSubmit(values))}>
                        <Stack spacing="md">
                            <TextInput required type="email" label="Email" {...form.getInputProps('UsernameOrEmail')}/>
                            <TextInput required type="password" label="Password" {...form.getInputProps('Password')}/>
                            <Button type="submit">Login</Button>

                        </Stack>
                    </form>
                </Paper>

                <Paper shadow="sm" radius="md" p="lg" withBorder>
                    <Stack spacing="md">
                        <RegisterButton/>
                    </Stack>
                </Paper>
            </div>
        </MantineProvider>
    );

    function RegisterButton() {
        return (
            <Button variant="gradient" gradient={{from: 'teal', to: 'lime', deg: 105}} size="lg"
                    onClick={() => navigate('/register')}>
                Sign up
            </Button>
        );
    }
};