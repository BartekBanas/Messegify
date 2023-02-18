import Cookies, {get} from 'js-cookie';
import {useEffect, useState} from "react";

const useAccountAuthorization = () => {
    const [isLogged, setIsLogged] = useState(false);

    console.log("Let's authorize!")

    const token = Cookies.get('auth_token');

    useEffect(() => {
        try {
            console.log("Let's authorize!")

            console.log("inside: ", token)
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