import api from './axiosConfig';

export const foodApi = {
    getAll: async () => {
        const response = await api.get('/fooditems');
        return response.data;
    },

    getPopular: async () => {
        const response = await api.get('/fooditems/popular');
        return response.data;
    },

    search: async (query) => {
        const response = await api.get('/fooditems/search', {
            params: { query }
        });
        return response.data;
    },

    getByCategory: async (category) => {
        const response = await api.get(`/fooditems/category/${category}`);
        return response.data;
    },

    seedFoods: async () => {
        const response = await api.post('/fooditems/seed');
        return response.data;
    }
};