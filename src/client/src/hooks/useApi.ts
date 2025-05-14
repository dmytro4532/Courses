import axios from 'axios';
import { useSelector } from 'react-redux';
import type { RootState } from '../store';

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL || 'http://localhost:5000',
});

export const useApi = () => {
  const { token } = useSelector((state: RootState) => state.auth);

  if (token) {
    api.defaults.headers.common['Authorization'] = `Bearer ${token}`;
  } else {
    delete api.defaults.headers.common['Authorization'];
  }

  return api;
}; 