// src/services/authService.js
import api from './axios.js';


export const getAllEmployees = (page, pageSize) => {
  return api.get('/employee/getallemployee', {
    params: { page, pageSize }
  });
};

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
  api.put('/employee/update-using-dapper', {
    Id: employee.id,
    EmpName: employee.empName,
    Email: employee.email,
    Phone: employee.phone,
    DepartmentId: employee.departmentId,  // add this
    Role: employee.role
  });

export const getDepartments = () => api.get('/department');

export const deleteEmployeeById = (id) =>
  api.delete(`/employee/${id}`);


export const getRoles = () => api.get('/auth/roles'); 

export const getEmployeeByEmail = (email) => {
  return api.post('/employee/get-by-email', { email });
};


    

