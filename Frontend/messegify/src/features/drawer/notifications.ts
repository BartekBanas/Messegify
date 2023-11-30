import {showNotification} from "@mantine/notifications";
import notificationStyles from "../../styles/notificationStyles";

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

export function updateErrorNotification() {
    showNotification({
        color: 'red',
        title: 'Error',
        message: 'Account could not be updated',
        styles: notificationStyles,
    })
}

export function updateSuccessNotification() {
    showNotification({
        color: 'green',
        title: 'Success',
        message: 'Account updated successfully',
        styles: notificationStyles,
    });
}

export function contactDeletionErrorNotification() {
    showNotification({
        color: 'red',
        title: 'Error',
        message: 'Contact could not be deleted',
        styles: notificationStyles,
    })
}

export function contactDeletionSuccessNotification() {
    showNotification({
        color: 'green',
        title: 'Success',
        message: 'Contact deleted successfully',
        styles: notificationStyles,
    });
}