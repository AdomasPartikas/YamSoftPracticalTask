import axios, { AxiosResponse } from 'axios';
import { 
  UserLoginDto, 
  UserRegisterDto, 
  AuthResponse, 
  Product, 
  Cart
} from '../types';
import { CookieManager } from '../utils/cookies';

const API_BASE_URL = process.env.REACT_APP_API_URL ?? (() => { throw new Error('REACT_APP_API_URL is not defined in environment variables'); })();

const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

apiClient.interceptors.request.use(
  (config) => {
    const token = CookieManager.get('auth_token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      CookieManager.remove('auth_token');
      localStorage.removeItem('user');
      
      if (!window.location.pathname.includes('/login') && !window.location.pathname.includes('/')) {
        window.location.href = '/';
      }
      
      return Promise.reject(new Error('Unauthorized. Please log in again.'));
    }

    if (error.response?.data?.error) {
      throw new Error(error.response.data.error);
    }
    throw error;
  }
);

export class AuthService {
  static async login(credentials: UserLoginDto): Promise<AuthResponse> {
    try {
      const response: AxiosResponse<AuthResponse> = await apiClient.post('/auth/login', credentials);
      return response.data;
    } catch (error) {
      throw error;
    }
  }

  static async register(userData: UserRegisterDto): Promise<AuthResponse> {
    try {
      const response: AxiosResponse<AuthResponse> = await apiClient.post('/auth/register', userData);
      return response.data;
    } catch (error) {
      throw error;
    }
  }
}

export class ProductService {
  static async getAllProducts(): Promise<Product[]> {
    try {
      const response: AxiosResponse<Product[]> = await apiClient.get('/product');
      return response.data;
    } catch (error) {
      throw error;
    }
  }

  static async getProductById(id: number): Promise<Product> {
    try {
      const response: AxiosResponse<Product> = await apiClient.get(`/product/${id}`);
      return response.data;
    } catch (error) {
      throw error;
    }
  }

  static async searchProducts(name: string): Promise<Product[]> {
    try {
      const response: AxiosResponse<Product[]> = await apiClient.get(`/product/search?name=${name}`);
      return response.data;
    } catch (error) {
      throw error;
    }
  }
}

export class CartService {
  static async getCartByUserId(userId: number): Promise<Cart> {
    try {
      const response: AxiosResponse<Cart> = await apiClient.get(`/cart/user/${userId}`);
      return response.data;
    } catch (error) {
      throw error;
    }
  }

  static async addToCart(cartId: number, productId: number, quantity: number): Promise<any> {
    try {
      const response: AxiosResponse<any> = await apiClient.post(`/cart/${cartId}/items`, {
        productId,
        quantity
      });
      return response.data;
    } catch (error) {
      throw error;
    }
  }

  static async removeFromCart(cartItemId: number): Promise<void> {
    try {
      await apiClient.delete(`/cart/items/${cartItemId}`);
    } catch (error) {
      throw error;
    }
  }

  static async updateCartItem(cartItemId: number, quantity: number): Promise<any> {
    try {
      const response: AxiosResponse<any> = await apiClient.put(`/cart/items/${cartItemId}`, { 
        quantity 
      });
      return response.data;
    } catch (error) {
      throw error;
    }
  }
}