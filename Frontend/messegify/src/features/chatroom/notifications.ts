import {showNotification} from "@mantine/notifications";
import notificationStyles from "../../styles/notificationStyles";

export function sendMessageErrorNotification() {
    showNotification({
        color: 'red',
        title: 'Error',
        message: 'Could not send message',
        styles: notificationStyles,
    })
}