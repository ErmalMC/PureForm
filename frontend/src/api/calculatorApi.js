import api from './axiosConfig';

export const calculatorApi = {
    getRecommendations: async (userId) => {
        const response = await api.get(`/nutritioncalculator/recommendations/${userId}`);
        return response.data;
    },

    applyRecommendations: async (userId) => {
        const response = await api.post(`/nutritioncalculator/apply-recommendations/${userId}`);
        return response.data;
    }
};