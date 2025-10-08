import axios from 'axios';
const baseUrl = 'https://localhost:7070/api'

const api = axios.create({
  baseURL: 'https://localhost:7070/api',
});

api.interceptors.request.use(config => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

let isRefeshing = false;
let failedQueue = [];

const processQueue = (error, token = null) => {
  failedQueue.forEach(prom => {
    if (error) {
      prom.reject(error);
    }
    else {
      prom.resolve(token);
    }
  });
  failedQueue = [];
};

api.interceptors.response.use(
  response => response,
  async error => {
    const originalRequest = error.config;

    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;
      const refreshToken = localStorage.getItem('refreshToken');

      if (!refreshToken) return Promise.reject(error);

      try {
        const response = await axios.post(`${baseUrl}/auth/refresh-token`, {
          accessToken: localStorage.getItem('token'),
          refreshToken: refreshToken
        });
        console.log(response);

        const newToken = response.data.token;
        const newRefreshToken = response.data.refreshToken;

        localStorage.setItem('token', newToken);
        localStorage.setItem('refreshToken', newRefreshToken);

        originalRequest.headers['Authorization'] = 'Bearer ' + newToken;

        return api(originalRequest);
      } catch (err) {
        // Refresh token invalid → logout
        localStorage.removeItem('token');
        localStorage.removeItem('refreshToken');
        return Promise.reject(err);
      }
    }

    return Promise.reject(error);
  }
);


export default api;
