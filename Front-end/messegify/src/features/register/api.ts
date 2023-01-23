import {API_URL} from "../../config";
import {RegisterFormType} from "./register-form.type";

export const register = async (Username: string, Password: string, Email: string) => {\

    var registerAccountDto: RegisterFormType;
    registerAccountDto = {
        email: Email,
        password: Password,
        username: Username
    }

    const response = await fetch(`${API_URL}/account`, {
        method: 'POST',
        headers: {
            ContentType: 'application/json',
            // Authorization: 'Basic' + window.btoa(Username + ':' + Password + ':' + Email),
            'Access-Control-Allow-Origin': '*'
        },
        credentials: 'include',
        body: registerAccountDto // TODO  serialzie this
    });

    if (response.status !== 200) throw new Error('Registration failed');
    return await response.text();
}