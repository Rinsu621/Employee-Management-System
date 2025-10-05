import axios from 'axios';

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
      if (isRefreshing) {
        return new Promise((resolve, reject) => {
          failedQueue.push({ resolve, reject });
        }).then(token => {
          originalRequest.headers['Authorization'] = 'Bearer ' + token;
          return api(originalRequest);
        }).catch(err => Promise.reject(err));
      }
      isRefreshing = true;
      const refreshToken = localStorage.getItem('refreshToken');
      return new Promise(async (resolve, reject) => {
        try {
          const response = await api.post('/auth/refresh-token', {
            accessToken: localStorage.getItem('token'),
            refreshToken: refreshToken
          });

          const newToken = response.data.token;
          const newRefreshToken = response.data.refreshToken;

          localStorage.setItem('token', newToken);
          localStorage.setItem('refreshToken', newRefreshToken);

          api.defaults.headers['Authorization'] = 'Bearer ' + newToken;
          originalRequest.headers['Authorization'] = 'Bearer ' + newToken;

          processQueue(null, newToken);
          resolve(api(originalRequest));
        } catch (err) {
          processQueue(err, null);
          reject(err);
        } finally {
          isRefreshing = false;
        }
      });
    }

    return Promise.reject(error);
  }
);

export default api;
