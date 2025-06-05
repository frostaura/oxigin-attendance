// client.ts
// Service functions for interacting with the ClientController backend API.
// Provides utility methods for CRUD operations on clients.
import type { Client } from "../../models/clientModels";
import { GetAsync, PostAsync, DeleteAsync, PutAsync } from "./backend";

/**
 * Get all clients.
 * @returns {Promise<Client[]>} List of Client entities.
 */
export async function getClientsAsync(): Promise<Client[]> {
    return await GetAsync<Client[]>("Client");
}

/**
 * Get a client by ID.
 * @param {string} id - The Client ID.
 * @returns {Promise<Client | null>} The Client entity, or null if not found.
 */
export async function getClientByIdAsync(id: string): Promise<Client | null> {
    return await GetAsync<Client>(`Client/${id}`);
}

/**
 * Add a new client.
 * @param {Partial<Client>} client - The Client entity to add.
 * @returns {Promise<Client>} The created Client entity.
 */
export async function addClientAsync(client: Partial<Client>): Promise<Client> {
    return await PostAsync<Client>("Client", client);
}

/**
 * Update an existing client.
 * @param {Client} client - The Client entity with updated details.
 * @returns {Promise<Client>} The updated Client entity.
 */
export async function updateClientAsync(client: Client): Promise<Client> {
    // Send the client object directly - property names already match the backend
    console.log('Updating client with data:', client);
    return await PutAsync<Client>("Client", client);
}

/**
 * Remove a client by ID.
 * @param {string} id - The Client ID.
 * @returns {Promise<void>} Resolves when the client is removed.
 */
export async function removeClientAsync(id: string): Promise<void> {
    await DeleteAsync<void>(`Client/${id}`);
}
