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

            console.log('logged');

            setIsLogged(true);

        } catch (error) {

            console.log('not logged');

            setIsLogged(false);
        }
    }, [token]);
    return () => {
        return isLogged;
    }
};

export default useAccountAuthorization;