import {showNotification} from "@mantine/notifications";

export const RegisterErrorNotification = () => {
    showNotification({
        color: 'red',
        title: 'Error',
        message: 'Registration failed',
    })
}