// employeeModels.ts
// Model representing an Employee entity, matching the backend C# model.

export interface Employee {
    /**
     * The unique identifier for the employee.
     */
    id: string;
    /**
     * The name of the employee.
     */
    name: string;
    /**
     * The surname of the employee.
     */
    surname?: string;
    /**
     * The ID number of the employee.
     */
    idNumber?: string;
    /**
     * The address of the employee.
     */
    address?: string;
    /**
     * The contact number for the employee.
     */
    contact?: string;
    /**
     * The email address for the employee.
     */
    email?: string;
    /**
     * The company or department (if applicable).
     */
    company?: string;
    // Add any other fields present in your backend Employee model.
}
