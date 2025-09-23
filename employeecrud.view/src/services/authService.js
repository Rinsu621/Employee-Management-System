// src/services/authService.js
import axios from 'axios';

const API_URL = 'https://localhost:7070/api/auth';

export const login = async (email, password) => {
  try {
    const response = await axios.post(`${API_URL}/login`, { email, password });
    if (response.data.token) {
      localStorage.setItem('token', response.data.token);
      localStorage.setItem('refreshToken', response.data.refreshToken);
    }
    return response.data;
  } catch (error) {
    throw error.response?.data || error;
  }
};
