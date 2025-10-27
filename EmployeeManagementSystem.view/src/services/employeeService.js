// src/services/authService.js
import api from './axios.js';

export const getAllEmployees = (page, pageSize, role = null, departmentId = null, fromDate = null, toDate = null, searchTerm = null, sortKey = "CreatedAt", sortAsc = true) => {
  return api.post('/employee/get-all-employees-using-dapper', {
    page, pageSize, role, departmentId, fromDate, toDate, searchTerm, sortKey, sortAsc
  });
};

export const createEmployee = (employee) => {
  return api.post('/employee/add-employee-using-dapper', {
    EmpName: employee.empName,
    Email: employee.email,
    Phone: employee.phone,
    DepartmentId: employee.departmentId,
    Role: employee.role
  });
};

export const updateEmployee = (employee) =>
  api.put('/employee/update-using-dapper', {
    Id: employee.id,
    EmpName: employee.empName,
    Email: employee.email,
    Phone: employee.phone,
    DepartmentId: employee.departmentId,
    Role: employee.role
  });

export const getDepartments = () => api.get('/department');

export const deleteEmployeeById = (id) =>
  api.delete(`/employee/delete-employee-using-dapper/${id}`);

export const getRoles = () => api.get('/auth/roles');

export const getEmployeeByEmail = (email) => {
  return api.post('/employee/get-by-email', { email });
};

export const exportEmployeesToExcel = (filters) => {
  return api.get('/employee/export', {
    params: filters,
    responseType: 'blob'
  });
};

export const exportEmployeesToPdf = (filters) => {
  return api.post('/employee/quest-pdf', filters, {
    responseType: 'blob'
  });
};
