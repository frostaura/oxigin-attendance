import { GetAsync, PostAsync, DeleteAsync, PutAsync } from './backend';
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
        console.log('Raw client response:', response); // This will help us see the exact field names
        return response.map(client => ({
            key: client.id,
            id: client.id,
            name: client.companyName || '',
            email: '',
            company: client.companyName || '',
            phone: client.contactNo || '',
            registrationNo: client.regNo || '',
            address: client.address || ''
        }));
    } catch (error) {
        console.error('Error in getClientsAsync:', error);
        if (error instanceof Error) {
            throw new Error(`Failed to fetch clients: ${error.message}`);
        }
        throw error;
    }
}

/**
 * Adds a new client to the database.
 * @param client The client data to add
 * @returns The created client
 */
export async function addClientAsync(client: Partial<ClientData>): Promise<ClientData> {
    if (!client.registrationNo) {
        throw new Error("Registration number is required");
    }
    
    try {
        const response = await PostAsync<any>('Client', {
            companyName: client.company,
            contactNo: client.phone,
            address: client.address,
            regNo: client.registrationNo
        });
        return response;
    } catch (error) {
        console.error('Error in addClientAsync:', error);
        if (error instanceof Error) {
            throw new Error(`Failed to add client: ${error.message}`);
        }
        throw error;
    }
}

/**
 * Updates an existing client in the database.
 * @param client The client data to update
 * @returns The updated client
 */
export async function updateClientAsync(client: ClientData): Promise<ClientData> {
    if (!client.registrationNo) {
        throw new Error("Registration number is required");
    }

    try {
        // Map to backend model format
        const backendClient = {
            id: client.id,
            CompanyName: client.company,
            ContactNo: client.phone,
            Address: client.address,
            RegNo: client.registrationNo
        };
        
        console.log('Updating client with data:', backendClient);
        
        const response = await PutAsync<any>('Client', backendClient);
        
        console.log('Update response:', response);
        return response;
    } catch (error) {
        console.error('Error details:', error);
        if (error instanceof Error) {
            throw new Error(`Failed to update client: ${error.message}`);
        }
        throw error;
    }
}

/**
 * Deletes a client from the database.
 * @param id The ID of the client to delete
 */
export async function deleteClientAsync(id: string): Promise<void> {
    try {
        await DeleteAsync(`Client/${id}`);
    } catch (error) {
        console.error('Error in deleteClientAsync:', error);
        if (error instanceof Error) {
            throw new Error(`Failed to delete client: ${error.message}`);
        }
        throw error;
    }
} 