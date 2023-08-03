import {showNotification} from "@mantine/notifications";
import notificationStyles from "../../styles/notificationStyles";

export const loginErrorNotification = () => {
    showNotification({
        color: 'red',
        title: 'Error',
        message: 'Login failed',
        styles: notificationStyles,
    });
}