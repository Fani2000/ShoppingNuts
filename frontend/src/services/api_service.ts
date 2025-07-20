// src/services/api_service.ts

import axios from 'axios';

// Get API URL from environment variables with fallback chain
const getApiUrl = () => {
    // Try to get from import.meta.env (Vite)
    if (import.meta.env.VITE_BACKEND_API_URL) {
        return import.meta.env.VITE_BACKEND_API_URL;
    }

    // Try to get from Aspire environment variable
    if (import.meta.env.BackendApi__Url) {
        return import.meta.env.BackendApi__Url;
    }

    // Try to get from window.ENV (runtime injection)
    if ((window as any).ENV && (window as any).ENV.BackendApi__Url) {
        return (window as any).ENV.BackendApi__Url;
    }

    // Fallback to default
    return '';
};

const API_URL = getApiUrl();
console.log("Backend API URL:", API_URL);

// Base API configuration
const api = axios.create({
    baseURL: API_URL,
    headers: {
        'Content-Type': 'application/json'
    }
});

// Cart items endpoints - matches CartItemsController
export const cartItemsApi = {
    getAll: () => api.get('/api/CartItems'),
    getById: (id: number) => api.get(`/api/CartItems/${id}`),
    create: (item: any) => api.post('/api/CartItems', item),
    update: (id: number, item: any) => api.put(`/api/CartItems/${id}`, item),
    delete: (id: number) => api.delete(`/api/CartItems/${id}`)
};

// Orders endpoints - matches OrdersController
export const ordersApi = {
    getAll: () => api.get('/api/Orders'),
    getById: (id: string) => api.get(`/api/Orders/${id}`),
    create: (order: any) => api.post('/api/Orders', order),
    update: (id: string, order: any) => api.put(`/api/Orders/${id}`, order),
    delete: (id: string) => api.delete(`/api/Orders/${id}`),
    updateShippingAddress: (id: string, shippingAddress: any) => {
        return api.put(`/api/Orders/${id}/shipping-address`, shippingAddress);
    },
    // We should use the dedicated endpoint instead of this manual approach
    updateWithAddress: (id: string, shippingAddress: any) => {
        return api.put(`/api/Orders/${id}/shipping-address`, shippingAddress);
    }
};

// Note: It appears there's no ProductsController in the backend yet
// This is a temporary implementation until a proper ProductsController is added
export const productsApi = {
    getAll: () => api.get('/api/CartItems'),
    getById: (id: number) => api.get(`/api/CartItems/${id}`),
    getAllFallback: async () => {
        try {
            return await api.get('/api/CartItems');
        } catch (error) {
            const cartResponse = await cartItemsApi.getAll();
            const products = cartResponse.data.map((item: any) => ({
                id: item.id,
                name: item.name,
                description: `Premium quality ${item.name.toLowerCase()} - perfect for snacking and cooking.`,
                image: item.image,
                price: item.price,
                tags: ["Healthy", "Natural", "Premium"]
            }));
            return { data: products };
        }
    }
};

export default api;