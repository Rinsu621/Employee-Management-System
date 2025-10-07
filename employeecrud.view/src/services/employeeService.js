// src/services/authService.js
import api from './axios.js';


//export const getAllEmployees = (page, pageSize, role = null, departmentId = null, fromDate = null, toDate = null, searchTerm = null, sortKey = "createdAt", sortAsc = true) => {
//  return api.post('/employee/getallemployee', {
//    page, pageSize, role, departmentId, fromDate, toDate, searchTerm, sortKey, sortAsc
//  });
//};

export const getAllEmployees = (page, pageSize, role = null, departmentId = null, fromDate = null, toDate = null, searchTerm = null, sortKey = "createdAt", sortAsc = true) => {
  return api.post('/employee/get-all-employees-using-dapper', {
    page, pageSize, role, departmentId, fromDate, toDate, searchTerm, sortKey, sortAsc
  });
};

export const createEmployee = (employee) => {
  return api.post('/employee', {

      EmpName: employee.empName,
      Email: employee.email,
     Phone: employee.phone,
     DepartmentId: employee.departmentId,
      Role: employee.role
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


    

