// src/services/authService.js
import api from './axios.js';


export const getAllEmployees = () => api.get('/employee/getallemployee')

export const createEmployee = (employee) => {
  return api.post('/employee', {
    employee: {
      EmpName: employee.empName,
      Email: employee.email,
      Phone: employee.phone,
      Role: employee.role
    }
  });
};
// Update an existing employee
export const updateEmployee = (employee) =>
  api.put('/employee/update', {
    Id: employee.id,
    EmpName: employee.empName,
    Email: employee.email,
    Phone: employee.phone,
    Role: employee.role
  });

export const deleteEmployeeById = (id) =>
  api.delete(`/employee/${id}`);


export const getRoles = () => api.get('/auth/roles'); 



    

