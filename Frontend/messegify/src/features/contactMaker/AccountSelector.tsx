import React, {FC, useEffect, useState} from 'react';
import {TextInput, MantineProvider, Paper, Button} from '@mantine/core';
import ky from 'ky';
import {API_URL} from '../../config';
import Cookies from 'js-cookie';
import {Account} from "../../types/account";

export const AccountSelector: FC = () => {
    const [accounts, setAccounts] = useState<Account[]>([]);
    const [searchQuery, setSearchQuery] = useState<string>('');

    useEffect(() => {
        const fetchAccounts = async () => {
            try {
                const response = await ky.get(`${API_URL}/account`).json<Account[]>();
                setAccounts(response);
            } catch (error) {
                console.error('Error fetching accounts:', error);
            }
        };

        fetchAccounts();
    }, []);

    const handleAccountClick = async (accountId: string) => {
        const token = Cookies.get('auth_token');
        const authorizedKy = ky.extend({
            headers: {
                authorization: `Bearer ${token}`
            }
        });

        try {
            await authorizedKy.post(`${API_URL}/contact/${accountId}`);
        } catch (error) {
            console.error('Error creating contact:', error);
        }
    };

    const filteredAccounts = accounts.filter((account) =>
        account.name.toLowerCase().includes(searchQuery.toLowerCase())
    );

    return (
        <MantineProvider theme={{colorScheme: 'dark'}}>
            <Paper shadow="sm" radius="md" p="lg" withBorder>
                <TextInput
                    placeholder="Search accounts..."
                    value={searchQuery}
                    onChange={(event) => setSearchQuery(event.currentTarget.value)}
                    style={{marginBottom: '1rem'}}
                />
                {searchQuery.length > 0 ? (
                    <ul>
                        {filteredAccounts.map((account) => (
                            <li key={account.id} style={{marginBottom: '0.5rem'}}>
                                <Button
                                    variant="light"
                                    onClick={() => handleAccountClick(account.id)}
                                >
                                    {account.name}
                                </Button>
                            </li>
                        ))}
                    </ul>
                ) : (
                    <div></div>
                )}
            </Paper>
        </MantineProvider>
    );
};