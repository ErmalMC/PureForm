import { createContext, useContext, useState, useEffect } from 'react';
import { authApi } from '../api/authApi';
import api from '../api/axiosConfig';

const AuthContext = createContext(null);

export const AuthProvider = ({ children }) => {
    const [user, setUser] = useState(() => {
        const savedUser = localStorage.getItem('user');
        return savedUser ? JSON.parse(savedUser) : null;
    });
    const [token, setToken] = useState(() => localStorage.getItem('token'));
    const [loading, setLoading] = useState(false);

    const login = async (email, password) => {
        try {
            const response = await authApi.login({ email, password });

            localStorage.setItem('token', response.token);
            localStorage.setItem('user', JSON.stringify(response.user));

            setToken(response.token);
            setUser(response.user);

            return { success: true };
        } catch (error) {
            return {
                success: false,
                error: error.response?.data?.message || 'Login failed'
            };
        }
    };

    const register = async (userData) => {
        try {
            const response = await authApi.register(userData);

            localStorage.setItem('token', response.token);
            localStorage.setItem('user', JSON.stringify(response.user));

            setToken(response.token);
            setUser(response.user);

            return { success: true };
        } catch (error) {
            return {
                success: false,
                error: error.response?.data?.message || 'Registration failed'
            };
        }
    };

    const logout = () => {
        localStorage.removeItem('token');
        localStorage.removeItem('user');
        setToken(null);
        setUser(null);
    };

    const refreshUser = async () => {
        try {
            const savedToken = localStorage.getItem('token');
            if (!savedToken) return;

            const response = await api.get('/auth/me');

            const userData = response.data;
            localStorage.setItem('user', JSON.stringify(userData));
            setUser(userData);
            console.log('User refreshed:', userData);
        } catch (error) {
            console.error('Failed to refresh user:', error);
        }
    };

    const value = {
        user,
        token,
        login,
        register,
        logout,
        refreshUser, // ADD THIS
        isAuthenticated: !!token,
    };

    if (loading) {
        return <div>Loading...</div>;
    }

    return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export const useAuth = () => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error('useAuth must be used within an AuthProvider');
    }
    return context;
};