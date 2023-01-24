import React, {FC} from 'react';
import {RegisterForm} from "../features/register/RegisterForm";

interface RegisterPageProps {
}

export const RegisterPage: FC<RegisterPageProps> = () => {
    return (
        <RegisterForm/>
    );
};