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

export const RegisterSuccessNotification = () => {
    showNotification({
        color: 'green',
        title: 'Success',
        message: 'Registration successful',
        styles: notificationStyles,
    });
};