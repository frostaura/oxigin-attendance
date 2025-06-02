// employee.ts
// Service functions for interacting with the EmployeeController backend API.
// Provides utility methods for CRUD operations on employees.
import type { Employee } from "../../models/employeeModels";
import { GetAsync, PostAsync, PutAsync, DeleteAsync } from "./backend";

/**
 * Get all employees.
 * @returns {Promise<Employee[]>} List of Employee entities.
 */
export async function getEmployeesAsync(): Promise<Employee[]> {
    return await GetAsync<Employee[]>("Employee");
}

/**
 * Get an employee by ID.
 * @param {string} id - The Employee ID.
 * @returns {Promise<Employee | null>} The Employee entity, or null if not found.
 */
export async function getEmployeeByIdAsync(id: string): Promise<Employee | null> {
    return await GetAsync<Employee>(`Employee/${id}`);
}

/**
 * Add a new employee.
 * @param {Partial<Employee>} employee - The Employee entity to add.
 * @returns {Promise<Employee>} The created Employee entity.
 */
export async function addEmployeeAsync(employee: Partial<Employee>): Promise<Employee> {
    return await PostAsync<Employee>("Employee", employee);
}

/**
 * Update an existing employee.
 * @param {Employee} employee - The Employee entity with updated details.
 * @returns {Promise<Employee>} The updated Employee entity.
 */
export async function updateEmployeeAsync(employee: Employee): Promise<Employee> {
    return await PutAsync<Employee>("Employee", employee); // If your backend expects PUT, adjust accordingly
}

/**
 * Remove an employee by ID.
 * @param {string} id - The Employee ID.
 * @returns {Promise<void>} Resolves when the employee is removed.
 */
export async function removeEmployeeAsync(id: string): Promise<void> {
    await DeleteAsync<void>(`Employee/${id}`);
}
