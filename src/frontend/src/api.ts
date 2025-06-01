import axios from 'axios';

const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || 'http://localhost:5275/', // fallback
  headers: {
    'Content-Type': 'application/json',
  },
});


export const fetchJobs = async () => {
  const response = await api.get('/jobs');
  return response.data;
};



export default api;
