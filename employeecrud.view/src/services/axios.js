import axios from 'axios';
const baseUrl = 'https://localhost:7070/api'

const api = axios.create({
  baseURL: 'https://localhost:7070/api',
});

//Run before every request, check if JWT access token stored in sessionStorage and if there is then auto adds to request header
api.interceptors.request.use(config => {
  const token = sessionStorage.getItem('token');
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
      prom.reject(error);// if the refresh failed , all queued request should fail
    }
    else {
      prom.resolve(token); // if the refresh succeeded, all queued requests can now retry using this new token
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
      const refreshToken = sessionStorage.getItem('refreshToken');

      if (!refreshToken) return Promise.reject(error);

      try {
        const response = await axios.post(`${baseUrl}/auth/refresh-token`, {
          accessToken: sessionStorage.getItem('token'),
          refreshToken: refreshToken
        });
        console.log(response);

        const newToken = response.data.token;
        const newRefreshToken = response.data.refreshToken;

        sessionStorage.setItem('token', newToken);
        sessionStorage.setItem('refreshToken', newRefreshToken);

        originalRequest.headers['Authorization'] = 'Bearer ' + newToken;

        return api(originalRequest);
      } catch (err) {
        // Refresh token invalid → logout
        sessionStorage.removeItem('token');
        sessionStorage.removeItem('refreshToken');
        return Promise.reject(err);
      }
    }

    return Promise.reject(error);
  }
);


export default api;
