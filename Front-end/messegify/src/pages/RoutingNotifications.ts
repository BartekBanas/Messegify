import {showNotification} from "@mantine/notifications";

export const AuthenticationErrorNotification = () => {
    showNotification({
        color: 'red',
        title: 'Error',
        message: 'You Must login first',
    })
}