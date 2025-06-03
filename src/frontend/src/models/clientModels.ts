// clientModels.ts
// Model representing a Client entity, matching the backend C# model.

export interface Client {
    /**
     * The unique identifier for the client.
     */
    id: string;
    /**
     * The name of the client.
     */
    name: string;
    /**
     * The registration number of the client (if applicable).
     */
    registrationNo?: string;
    /**
     * The address of the client.
     */
    address?: string;
    /**
     * The contact number for the client.
     */
    contact?: string;
    /**
     * The email address for the client.
     */
    email?: string;
    /**
     * The company name (if applicable).
     */
    company?: string;
    // Add any other fields present in your backend Client model.
}

export interface ClientData {
    key: string;
    id: string;
    name: string;
    email: string;
    company: string;
    phone: string;
    status?: string;
    registrationNo?: string;
    address?: string;
    contactNo?: string;
}

export interface ClientRegistrationFormValues {
    email: string;
    password: string;
    confirmPassword: string;
    firstName: string;
    lastName: string;
    phone: string;
    companyName: string;
    address: string;
}
