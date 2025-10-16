// src/services/salaryService.js
import api from './axios.js';

export const getPaymentModes = () => {
  return api.get('/salary/payment-mode');
};

export const addSalary = (salaryData) => {
  return api.post('/salary/add-salary', salaryData);
};

// Generate Salary PDF & trigger email
export const generateSalaryPdf = (salaryId) => {
  return api.post(`/salary/generate-pdf/${salaryId}`, null, { responseType: 'blob' });
};

