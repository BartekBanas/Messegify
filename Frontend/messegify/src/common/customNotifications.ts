import {showNotification} from "@mantine/notifications";
import notificationStyles from "../styles/notificationStyles";

export function customSuccessNotification(message: string) {
    showNotification({
        color: 'green',
        title: 'Success',
        message: message,
        styles: notificationStyles,
    });
}

export function customErrorNotification(message: string) {
    showNotification({
        color: 'red',
        title: 'Error',
        message: message,
        styles: notificationStyles,
    })
}