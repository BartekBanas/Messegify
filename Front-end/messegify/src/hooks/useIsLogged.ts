import {Account} from "../features/Messegify/types/account";
import { useAuth0 } from '@auth0/auth0-react';

const useAccountAuthorization = (account: Account) => {
    const { user } = useAuth0();
    const userId = user?.sub;

    function isAuthorized() {
        return userId === account.id;
    }

    return { isAuthorized };
}

export default useAccountAuthorization;