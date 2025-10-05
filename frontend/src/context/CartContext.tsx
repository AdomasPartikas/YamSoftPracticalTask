import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import { Cart, Product } from '../types';
import { CartService } from '../services/api';
import { useAuth } from './AuthContext';

interface CartContextType {
  cart: Cart | null;
  loading: boolean;
  addToCart: (product: Product, quantity: number) => Promise<void>;
  removeFromCart: (productId: number) => Promise<void>;
  updateCartItem: (productId: number, quantity: number) => Promise<void>;
  refreshCart: () => Promise<void>;
}

const CartContext = createContext<CartContextType | undefined>(undefined);

export const useCart = () => {
  const context = useContext(CartContext);
  if (context === undefined) {
    throw new Error('useCart must be used within a CartProvider');
  }
  return context;
};

interface CartProviderProps {
  children: ReactNode;
}

export const CartProvider: React.FC<CartProviderProps> = ({ children }) => {
  const [cart, setCart] = useState<Cart | null>(null);
  const [loading, setLoading] = useState(false);
  const { user, isAuthenticated } = useAuth();

  useEffect(() => {
    if (isAuthenticated && user) {
      refreshCart();
    } else {
      setCart(null);
    }
  }, [isAuthenticated, user]);

  const refreshCart = async () => {
    if (!user) return;
    
    setLoading(true);
    try {
      const cartData = await CartService.getCartByUserId(user.id);
      setCart(cartData);
    } catch (error) {
      console.error('Failed to fetch cart:', error);
    } finally {
      setLoading(false);
    }
  };

  const addToCart = async (product: Product, quantity: number) => {
    if (!user) return;

    setLoading(true);
    try {
      let currentCart = cart;
      if (!currentCart) {
        currentCart = await CartService.getCartByUserId(user.id);
        setCart(currentCart);
      }
      
      await CartService.addToCart(currentCart.id, product.id, quantity);

      await refreshCart();
    } catch (error) {
      console.error('Failed to add to cart:', error);
      throw error;
    } finally {
      setLoading(false);
    }
  };

  const removeFromCart = async (productId: number) => {
    if (!user || !cart) return;

    setLoading(true);
    try {
      const cartItem = cart.cartItems.find(item => item.productId === productId);
      if (cartItem) {
        await CartService.removeFromCart(cartItem.id);
        await refreshCart();
      }
    } catch (error) {
      console.error('Failed to remove from cart:', error);
      throw error;
    } finally {
      setLoading(false);
    }
  };

  const updateCartItem = async (productId: number, quantity: number) => {
    if (!user || !cart) return;

    setLoading(true);
    try {
      const cartItem = cart.cartItems.find(item => item.productId === productId);
      if (cartItem) {
        await CartService.updateCartItem(cartItem.id, quantity);

        await refreshCart();
      }
    } catch (error) {
      console.error('Failed to update cart item:', error);
      throw error;
    } finally {
      setLoading(false);
    }
  };

  const value = {
    cart,
    loading,
    addToCart,
    removeFromCart,
    updateCartItem,
    refreshCart,
  };

  return (
    <CartContext.Provider value={value}>
      {children}
    </CartContext.Provider>
  );
};