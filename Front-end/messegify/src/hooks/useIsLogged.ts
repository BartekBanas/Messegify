import Cookies from 'js-cookie';
import {useEffect, useState} from "react";

const useAccountAuthorization = () => {
    const [isLogged, setIsLogged] = useState(false);

    const token = Cookies.get('auth_token');

    useEffect(() => {
        try {
            if (!token) {
                throw new Error('Token not found in cookies');
            }

            setIsLogged(true);

        } catch (error) {
            setIsLogged(false);
        }
    }, [token]);
    return () => {
        return isLogged;
    }
};

export default useAccountAuthorization;