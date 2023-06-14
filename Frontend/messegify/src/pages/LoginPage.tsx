import React, {FC} from 'react';
import {LoginForm} from "../features/login/LoginForm";

interface LoginPageProps {
}

export const LoginPage: FC<LoginPageProps> = () => {
    return (
        <LoginForm/>
    );
};