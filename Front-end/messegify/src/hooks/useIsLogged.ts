import {Account} from "../features/Messegify/types/account";
import {useAuth0} from '@auth0/auth0-react';
import {API_URL} from "../config";
import {oauthToken} from "@auth0/auth0-spa-js/dist/typings/api";
import Cookies, {get} from 'js-cookie';
import axios from 'axios';
import {useEffect, useState} from "react";

// const useAccountAuthorization = (account: Account) => {
//     const { user } = useAuth0();
//     const userId = user?.sub;
//
//     function isAuthorized() {
//         return userId === account.id;
//     }
//
//     return { isAuthorized };
// }

// async function useAccountAuthorization() {
//
//     const response = await fetch(`${API_URL}/authenticate`, {
//         method: 'POST',
//         headers: {
//             ContentType: 'application/json',
//             //Authorization: 'Basic' + window.btoa(oauthToken()),
//         },
//         credentials: 'include'
//     });
//
//     if (response.status !== 200) throw new Error('Unable to authorize');
//     return await response.text();
// }

const useAccountAuthorization = (): boolean => {
    const [isLogged, setIsLogged] = useState(false);

    useEffect(() => {
        const checkAuthorization = async () => {
            try {
                const token = Cookies.get('auth_token');
                if (!token) {
                    throw new Error('Token not found in cookies');
                }

                // const headers = {Authorization: `Bearer ${token}`};
                // const response = await axios.post(`${API_URL}/account/authenticate`, {headers});
                setIsLogged(true);

            } catch (error) {
                setIsLogged(false);
            }
        };
        checkAuthorization();
    }, []);
    return isLogged;
};

export default useAccountAuthorization;