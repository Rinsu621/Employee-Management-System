// src/services/salaryService.js
import api from './axios.js';

export const getPaymentModes = () => {
  return api.get('/salary/payment-mode');
};

export const addSalary = (salaryData) => {
  return api.post('/salary/add-salary', salaryData, { responseType: 'blob' });
};

