import {showNotification} from "@mantine/notifications";
import notificationStyles from "../../styles/notificationStyles";

export const RegisterErrorNotification = () => {
    showNotification({
        color: 'red',
        title: 'Error',
        message: 'Registration failed',
        styles: notificationStyles,
    })
}