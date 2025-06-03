import { GetAsync } from './backend';
import type { ClientData } from '../../models/clientModels';

/**
 * Gets all clients from the database.
 * @returns A list of clients
 */
export async function getClientsAsync(): Promise<Array<ClientData>> {
    try {
        // Check if we have a session first
        const session = localStorage.getItem("session");
        if (!session) {
            throw new Error("No active session found. Please sign in.");
        }

        const response = await GetAsync<Array<any>>('Client');
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
        if (error instanceof Error) {
            throw new Error(`Failed to fetch clients: ${error.message}`);
        }
        throw error;
    }
} 