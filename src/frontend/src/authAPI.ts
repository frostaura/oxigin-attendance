import api from './api';
import axios from 'axios'
import type { Client as User, EmployeeRegistrationFormValues, ErrorResponse } from './types';


export async function signIn(email: string, password: string): Promise<User> {
  try {
    const response = await api.post<User>('/User/SignIn', { email, password });
    return response.data;
  } catch (error: any) {
    if (axios.isAxiosError(error) && error.response) {
      const errData = error.response.data as ErrorResponse;
      throw new Error(`Error: ${errData.message} (origin: ${errData.origin})`);
    }
    throw error;
  }
}

export async function registerEmployee(data: EmployeeRegistrationFormValues) {
    try {
      const payload = {
        name: `${data.firstName} ${data.lastName}`,
        contactNr: data.phone,
        email: data.email,
        userType: 1, // 0 might be client, 1 might be employee — adjust pls
        password: data.password,
      };
  
      const response = await api.post('/User/SignUp', payload);
  
      return response.data;
    } catch (error: any) {
      if (axios.isAxiosError(error) && error.response) {
        const errData = error.response.data as ErrorResponse;
        throw new Error(`Error: ${errData.message} (origin: ${errData.origin})`);
      }
      throw error;
    }
  }
