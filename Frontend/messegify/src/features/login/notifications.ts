import {showNotification} from "@mantine/notifications";
import {MantineProvider} from '@mantine/core';

export const loginErrorNotification = () => {
    showNotification({
        color: 'red',
        title: 'Error',
        message: 'Login failed',
        styles: (theme) => ({
            root: {
                backgroundColor: theme.colors.gray[8],
                borderColor: theme.colors.gray[7],

                '&::before': {backgroundColor: theme.colorScheme},
            },

            title: {color: theme.colors.gray[3]},
            description: {color: theme.colorScheme},
            closeButton: {
                color: theme.colorScheme,
                '&:hover': {backgroundColor: theme.colors.blue[7]},
            },
        }),
    });
}
