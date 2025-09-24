// src/services/authService.js
import api from './axios.js';


export const login = async (email, password) => {
  try {
    const response = await api.post('/auth/login', { email, password })
    if (response.data.token) {
      localStorage.setItem('token', response.data.token);
      localStorage.setItem('refreshToken', response.data.refreshToken);
      localStorage.setItem('name', response.data.name);
    }
    return response.data;
  } catch (error) {
    throw error;
  }
};
