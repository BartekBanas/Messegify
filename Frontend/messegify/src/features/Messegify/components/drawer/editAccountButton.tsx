import {useDisclosure} from '@mantine/hooks';
import {Modal, Button, Group, Paper, Stack, TextInput, PasswordInput, Space} from '@mantine/core';
import Cookies from "js-cookie";
import ky from "ky";
import {API_URL} from "../../../../config";
import {updateErrorNotification, updateSuccessNotification} from "./notifications";
import React from "react";
import {useForm} from "@mantine/form";
import {RegisterFormType} from "../../../register/register-form.type";

export async function updateAccountRequest(username: string | null, password: string | null, email: string | null) {
    const requestBody: { username?: string; password?: string; email?: string } = {};

    requestBody.username = username ?? undefined;
    requestBody.password = password ?? undefined;
    requestBody.email = email ?? undefined;

    const token = Cookies.get('auth_token');
    const authorizedKy = ky.extend({
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*',
            authorization: `Bearer ${token}`
        },
        body: JSON.stringify(requestBody),
    });

    const response = authorizedKy.put(`${API_URL}/account/me`);

    return response;
}

export function UpdateAccountButton() {
    const [opened, {close, open}] = useDisclosure(false);
    const form = useForm<RegisterFormType>({
        initialValues: {
            Username: '',
            Password: '',
            Email: '',
        },
    });

    const handleUpdateAccount = async () => {
        try {
            await updateAccountRequest(
                form.values.Username || null,
                form.values.Password || null,
                form.values.Email || null
            );
            updateSuccessNotification();
        } catch (error) {
            updateErrorNotification();
        }

        close();
    };

    return (
        <>
            <Modal
                opened={opened}
                onClose={close}
                size="auto"
                styles={{
                    title: {
                        fontSize: '30px',
                        textAlign: 'center',
                        marginTop: '20px',
                    },
                }}
                title="Update your account"
            >
                <Space h="xl"/>
                <Paper shadow="sm" radius="md" p="lg" withBorder>
                    <form onSubmit={form.onSubmit(handleUpdateAccount)}>
                        <Stack spacing="md">
                            <TextInput type="username" label="Username" {...form.getInputProps('Username')} />
                            <PasswordInput label="Password" withAsterisk {...form.getInputProps('Password')} />
                            <TextInput type="email" label="Email" {...form.getInputProps('Email')} />
                            <Button type="submit">
                                Update Account
                            </Button>
                        </Stack>
                    </form>
                </Paper>
            </Modal>
            <Group position="center">
                <Button color="orange" onClick={open}>
                    Update Account
                </Button>
            </Group>
        </>
    );
}