import {showNotification} from "@mantine/notifications";

export const MissingUserNotification = () => {
    showNotification({
        color: 'red',
        title: 'Error',
        message: 'Login failed',
    })
}