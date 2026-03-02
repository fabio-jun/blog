import axios from "axios";

// Creates an acios instance with fixed based URL, to avoid repeating the base URL in all requests
const api = axios.create({
    baseURL: "http://localhost:5073/api",
});

// Interceptor: adds the token in all requests automatically
api.interceptors.request.use((config) => {
    const token = localStorage.getItem("accessToken");
    if (token){
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

export default api;