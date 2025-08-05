import axios from 'axios'

// Create axios instance with base configuration
const api = axios.create({
  baseURL: process.env.VUE_APP_API_URL || 'http://localhost:8000',
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json'
  }
})

// Request interceptor to add auth token
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token')
    if (token) {
      config.headers.Authorization = `Bearer ${token}`
    }
    return config
  },
  (error) => {
    return Promise.reject(error)
  }
)

// Response interceptor to handle auth errors
api.interceptors.response.use(
  (response) => {
    return response
  },
  (error) => {
    if (error.response?.status === 401) {
      // Clear auth data and redirect to login
      localStorage.removeItem('user')
      localStorage.removeItem('token')
      window.location.href = '/login'
    }
    return Promise.reject(error)
  }
)

// API methods
export const authAPI = {
  register: (userData) => api.post('/user/register', userData),
  login: (credentials) => api.post('/user/login', credentials),
  getProfile: () => api.get('/user/profile')
}

export const urlAPI = {
  shorten: (urlData) => api.post('/shorten', urlData)
}

export default api 