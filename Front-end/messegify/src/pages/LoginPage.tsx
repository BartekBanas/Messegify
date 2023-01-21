import React, { FC } from 'react';
import {Paper} from "@mantine/core";
import {LoginForm} from "../features/login/LoginForm";

interface LoginPageProps {}

export const LoginPage: FC<LoginPageProps> = () => {
  return (
    <Paper shadow="sm" radius="md" p="lg" withBorder>
        <LoginForm/>
    </Paper>
  );
};