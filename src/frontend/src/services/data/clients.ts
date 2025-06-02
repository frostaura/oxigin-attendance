import { GetAsync } from './backend';
import type { ClientData } from '../../types';

/**
 * Gets all clients from the database.
 * @returns A list of clients
 */
export async function getClientsAsync(): Promise<Array<ClientData>> {
    try {
        const sessionStr = localStorage.getItem("session");
        if (!sessionStr) {
            throw new Error("No session found");
        }
        const session = JSON.parse(sessionStr);
        const response = await GetAsync<Array<any>>('client', session.sessionId);
        return response.map(client => ({
            key: client.id,
            id: client.id,
            name: client.companyName || '',
            email: '',
            company: client.companyName || '',
            phone: client.contactNo || '',
            status: client.deleted ? 'Inactive' : 'Active'
        }));
    } catch (error) {
        console.error('Error in getClientsAsync:', error);
        throw error;
    }
} 