import {showNotification} from "@mantine/notifications";
import {MantineProvider} from '@mantine/core';

export const loginErrorNotification = () => {
    showNotification({
        color: 'red',
        title: 'Error',
        message: 'Login failed',
        styles: (theme) => ({
            root: {
                backgroundColor: theme.colors.dark[6],
                borderColor: theme.colors.dark[7],

                '&::before': {backgroundColor: theme.colorScheme},
            },

            title: {color: theme.colors.gray[2]},
            description: {color: theme.colorScheme},
            closeButton: {
                color: theme.colorScheme,
                '&:hover': {backgroundColor: theme.colors.blue[7]},
            },
        }),
    });
}
