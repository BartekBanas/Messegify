export interface AccountClaims {
    "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid": string,
    "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid": string,
    "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name": string,
    "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress": string,
    nbf: string,
    exp: string,
    iat: string,
    iss: string,
    aud: string
}