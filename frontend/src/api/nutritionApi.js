import api from './axiosConfig';

export const nutritionApi = {
    getById: async (id) => {
        const response = await api.get(`/nutrition/${id}`);
        return response.data;
    },

    getByUserId: async (userId, startDate = null, endDate = null) => {
        const params = {};
        if (startDate) params.startDate = startDate;
        if (endDate) params.endDate = endDate;

        const response = await api.get(`/nutrition/user/${userId}`, { params });
        return response.data;
    },

    create: async (userId, nutritionLog) => {
        const response = await api.post(`/nutrition/user/${userId}`, nutritionLog);
        return response.data;
    },

    update: async (id, nutritionLog) => {
        const response = await api.put(`/nutrition/${id}`, nutritionLog);
        return response.data;
    },

    delete: async (id) => {
        const response = await api.delete(`/nutrition/${id}`);
        return response.data;
    },

    getDailyTotals: async (userId, date) => {
        const response = await api.get(`/nutrition/user/${userId}/daily-totals`, {
            params: { date }
        });
        return response.data;
    }
};