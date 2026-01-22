import api from './axiosConfig';

export const workoutApi = {
    getById: async (id) => {
        const response = await api.get(`/workoutplans/${id}`);
        return response.data;
    },

    getByUserId: async (userId) => {
        const response = await api.get(`/workoutplans/user/${userId}`);
        return response.data;
    },

    create: async (userId, workoutPlan) => {
        const response = await api.post(`/workoutplans/user/${userId}`, workoutPlan);
        return response.data;
    },

    update: async (id, workoutPlan) => {
        const response = await api.put(`/workoutplans/${id}`, workoutPlan);
        return response.data;
    },

    delete: async (id) => {
        const response = await api.delete(`/workoutplans/${id}`);
        return response.data;
    },

    generatePersonalized: async (userId, difficultyLevel = 'Intermediate') => {
        const response = await api.post(`/workoutplans/user/${userId}/generate?difficultyLevel=${difficultyLevel}`);
        return response.data;
    }
};