// employeeModels.ts
// Model representing an Employee entity, matching the backend C# model.

//match the below interface with the style of the above interface


export interface Employee {
    /**
     * The unique identifier for the employee.
     */
    id: string;
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
    contactNo: string;
    /**
     * Bank name for payment details
     */
    bankName: string;
    /**
     * Account holder name for payment details
     */
    accountHolderName: string;
    /**
     * Branch code for payment details
     */
    branchCode: string;
    /**
     * Account number for payment details
     */
    accountNumber: string;
}
