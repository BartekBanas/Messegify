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

export function InviteToChatroomSuccessNotification() {
    showNotification({
        color: 'green',
        title: 'Success',
        message: 'Successfully invited user to chatroom',
        styles: notificationStyles,
    });
}

export function InviteToChatroomErrorNotification() {
    showNotification({
        color: 'red',
        title: 'Error',
        message: 'An error occurred while inviting user to chatroom',
        styles: notificationStyles,
    })
}

export function createChatroomSuccessNotification() {
    showNotification({
        color: 'green',
        title: 'Success',
        message: 'Successfully created chatroom',
        styles: notificationStyles,
    });
}

export function createChatroomErrorNotification() {
    showNotification({
        color: 'red',
        title: 'Error',
        message: 'An error occurred while creating chatroom',
        styles: notificationStyles,
    })
}