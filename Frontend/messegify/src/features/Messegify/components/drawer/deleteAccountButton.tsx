import {Button, Text} from '@mantine/core';
import {modals} from '@mantine/modals';
import Cookies from "js-cookie";
import ky from "ky";
import {API_URL} from "../../../../config";
import {Message} from "../../../../types/message";
import {removeJWTToken} from "../../../../pages/layout/Header";

function DeleteAccount() {
    const token = Cookies.get('auth_token');
    const authorizedKy = ky.extend({
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*',
            authorization: `Bearer ${token}`
        },
    });

    const response = authorizedKy.delete(`${API_URL}/account/me`).json<Message[]>();

    removeJWTToken();

    return response;
}

export function DeleteAccountButton() {
    const openDeleteModal = () =>
        modals.openConfirmModal({
            title: 'Delete your profile',
            centered: true,
            children: (
                <Text size="sm">
                    Are you sure you want to delete your account? This action cannot be reversed
                </Text>
            ),
            labels: {confirm: 'Delete account', cancel: "No don't delete it"},
            confirmProps: {color: 'red'},
            onCancel: () => console.log('Account saved'),
            onConfirm: () => DeleteAccount(),
        });

    return <Button onClick={openDeleteModal} color="red">Delete account</Button>;
}