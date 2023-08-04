import {useDisclosure} from '@mantine/hooks';
import {Modal, Button, Group, Text} from '@mantine/core';
import Cookies from "js-cookie";
import ky from "ky";
import {API_URL} from "../../../../config";
import {removeJWTToken} from "../../../../pages/layout/Header";

function deleteAccount() {
    const token = Cookies.get('auth_token');
    const authorizedKy = ky.extend({
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*',
            authorization: `Bearer ${token}`
        },
    });

    const response = authorizedKy.delete(`${API_URL}/account/me`);

    removeJWTToken();

    return response;
}

export function DeleteAccountButton() {
    const [opened, {close, open}] = useDisclosure(false);

    const handleDeleteAccount = async () => {
        await deleteAccount();
        close();
    };

    return (
        <>
            <Modal opened={opened} onClose={close} size="auto" title="Delete your account">
                <Text>Are you sure you want to delete your account?</Text>
                <Text>This action cannot be reversed</Text>

                <Group mt="xl" position="center">
                    <Button color="gray" onClick={close}>
                        No don't delete it
                    </Button>
                    <Button color="red" onClick={handleDeleteAccount}>
                        Delete Account
                    </Button>
                </Group>
            </Modal>
            <Group position="center">
                <Button color="red" onClick={open}>Delete Account</Button>
            </Group>
        </>
    );
}