import {HTTPError} from "ky";
import {customErrorNotification} from "./customNotifications";

export async function handleRequest<T>(request: Promise<T>, resource?: string): Promise<T> {
    try {
        return await request;
    } catch (error) {
        if (error instanceof HTTPError) {
            const status = error.response.status;
            if (error.message && error.message.length > 0) {
                customErrorNotification(error.message);
            } else if (status === 401) {
                customErrorNotification('Unauthorized. Please log in to access this resource.');
            } else if (status === 403) {
                customErrorNotification('Access denied. You do not have permission to access this resource.')
            } else if (status === 404) {
                resource !== undefined ? customErrorNotification(`${resource} was not found.`) : customErrorNotification('Resource not found.');
            } else {
                customErrorNotification(`Server response error code: ${status}`);
            }
        } else {
            console.log('An unexpected error occurred:', error);
        }
        throw error;
    }
}