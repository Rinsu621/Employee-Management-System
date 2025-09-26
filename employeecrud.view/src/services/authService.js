// src/services/authService.js
import api from './axios.js';
import jwt_decode from 'jwt-decode';


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

export const logout = () => {
  localStorage.removeItem('token');
  localStorage.removeItem('refreshToken');
  localStorage.removeItem('name');
  window.location.href = '/login'; 
};

export const getToken = () => localStorage.getItem("token");

export const isTokenValid = () => {
  const token = getToken();
  if (!token) return false;
  try {
    const decode = jwt_decode(token);
    return Date.now() < decode.exp * 1000;
  }
  catch {
    return false;
  }
};

export const getUserRole = () => {
  const token = getToken();
  if (!token) return null;
  try {
    const decoded = jwt_decode(token);
    return decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
  } catch {
    return null;
  }
};
