import {showNotification} from "@mantine/notifications";
import notificationStyles from "../../styles/notificationStyles";

export const MissingUserNotification = () => {
    showNotification({
        color: 'red',
        title: 'Error',
        message: 'Login failed',
        styles: notificationStyles,
    })
}