import {useDisclosure} from '@mantine/hooks';
import {Modal, Button, Group, Text} from '@mantine/core';
import {deletionErrorNotification, deletionSuccessNotification} from "./notifications";
import {deleteAccountRequest} from "./api";

export function DeleteAccountButton() {
    const [opened, {close, open}] = useDisclosure(false);

    const handleDeleteAccount = async () => {
        try {
            await deleteAccountRequest();
            deletionSuccessNotification();
        } catch (error) {
            deletionErrorNotification();
        }

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
                <Button color="red" size="md" onClick={open}>Delete Account</Button>
            </Group>
        </>
    );
}