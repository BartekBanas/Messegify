import {showNotification} from "@mantine/notifications";
import notificationStyles from "../../../../styles/notificationStyles";

export function deletionErrorNotification() {
    showNotification({
        color: 'red',
        title: 'Error',
        message: 'Account could not be deleted',
        styles: notificationStyles,
    })
}

export function deletionSuccessNotification() {
    showNotification({
        color: 'green',
        title: 'Success',
        message: 'Account deleted successfully',
        styles: notificationStyles,
    });
}