import api from './axiosConfig';

export const authApi = {
    register: async (userData) => {
        const response = await api.post('/auth/register', userData);
        return response.data;
    },

    login: async (credentials) => {
        const response = await api.post('/auth/login', credentials);
        return response.data;
    },

    checkEmail: async (email) => {
        const response = await api.get('/auth/check-email', {
            params: { email }
        });
        return response.data;
    }
};