import {API_URL} from "../../config";
import {useCookies} from "react-cookie";
import {log} from "util";


export const useLoginApi = () => {
    const authCookieName = 'auth_token'
    const [, setCookie,] = useCookies([authCookieName]);


    return async (username: string, password: string) => {
        //const jwt = null; // get jwt somehow


        const response = await fetch(`${API_URL}/account/authenticate`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Access-Control-Allow-Origin': '*',
                // 'Authorization': `Bearer ${jwt}`
            },
            body: JSON.stringify({
                UsernameOrEmail: username,
                Password: password,
            })
        });

        const JWT = await response.text();

        setCookie(authCookieName, JWT, {
            expires: new Date(Date.now() + 1000 * 60 * 15),     // expires in 15 minutes
            sameSite: true
        });

        if (response.status !== 200) throw new Error('Login failed');
        return await response.text();
    }
}