// src/services/salaryService.js
import api from './axios.js';

export const getPaymentModes = () => {
  return api.get('/salary/payment-mode');
};

//export const addSalary = (salaryData) => {
//  return api.post('/salary/add-salary-dapper', salaryData);
//};

export const addSalary = async (salaryData) => {
  try {
    const response = await api.post('/salary/add-salary-dapper', salaryData);
    return response.data;
  } catch (error) {
    if (error.response && error.response.status === 400) {
      // FluentValidation errors
      console.log(error.response.data);
      throw error.response.data; // send to frontend UI
    } else {
      throw { message: "Something went wrong!" };
    }
  }
};


// Generate Salary PDF & trigger email
export const generateSalaryPdf = (salaryId) => {
  return api.post(`/salary/generate-pdf/${salaryId}`, null, { responseType: 'blob' });
};

// Fetch all salaries by sending year & month in body
export const getAllSalaries = (year, month) => {
  return api.post('/salary/salary-result', { year, month });
};


export const updateSalaryStatus = (id, status) => {
  return api.put(`/salary/update-status/${id}`, { id, status });
};

