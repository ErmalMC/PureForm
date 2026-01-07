import api from './axiosConfig';

export const userApi = {
    getAll: async () => {
        const response = await api.get('/users');
        return response.data;
    },

    getById: async (id) => {
        const response = await api.get(`/users/${id}`);
        return response.data;
    },

    update: async (id, userData) => {
        const response = await api.put(`/users/${id}`, userData);
        return response.data;
    },

    delete: async (id) => {
        const response = await api.delete(`/users/${id}`);
        return response.data;
    }
};