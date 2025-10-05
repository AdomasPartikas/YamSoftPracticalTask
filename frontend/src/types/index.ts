export interface User {
  id: number;
  username: string;
  createdAt: string;
}

export interface UserLoginDto {
  username: string;
  password: string;
}

export interface UserRegisterDto {
  username: string;
  password: string;
}

export interface AuthResponse {
  user: User;
  token: string;
  expiresAt: string;
}

export interface Product {
  id: number;
  name: string;
  description?: string;
  price: number;
  stock: number;
  imageUrl?: string;
}

export interface CartItem {
  id: number;
  cartId: number;
  productId: number;
  product: Product;
  quantity: number;
  addedAt: string;
}

export interface Cart {
  id: number;
  userId: number;
  createdAt: string;
  updatedAt: string;
  totalAmount: number;
  cartItems: CartItem[];
}

export interface AddToCartDto {
  userId: number;
  productId: number;
  quantity: number;
}

export interface ApiError {
  error: string;
}