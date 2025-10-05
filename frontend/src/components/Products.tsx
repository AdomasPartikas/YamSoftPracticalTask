import React, { useState, useEffect, useCallback } from 'react';
import {
  Container,
  Card,
  CardContent,
  CardMedia,
  Typography,
  Button,
  Box,
  CircularProgress,
  Alert,
  AppBar,
  Toolbar,
  Badge,
  IconButton,
} from '@mui/material';
import { ShoppingCart, Logout } from '@mui/icons-material';
import { Product } from '../types';
import { ProductService } from '../services/api';
import { useAuth } from '../context/AuthContext';
import { useCart } from '../context/CartContext';
import { useNavigate } from 'react-router-dom';

const Products: React.FC = () => {
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string>('');
  const [hasLoaded, setHasLoaded] = useState(false);

  const { user, logout } = useAuth();
  const { cart, addToCart } = useCart();
  const navigate = useNavigate();

  const loadProducts = useCallback(async () => {
    if (hasLoaded) return;
    
    setLoading(true);
    setError('');

    try {
      const productsData = await ProductService.getAllProducts();
      setProducts(productsData);
      setHasLoaded(true);
    } catch (error: any) {
      setError(error.message || 'Failed to load products');
    } finally {
      setLoading(false);
    }
  }, [hasLoaded]);

  useEffect(() => {
    loadProducts();
  }, [loadProducts]);

  const handleScroll = useCallback(() => {
    if (
      window.innerHeight + document.documentElement.scrollTop
      >= document.documentElement.offsetHeight - 1000
    ) {
      // Load more products here
    }
  }, []);

  useEffect(() => {
    window.addEventListener('scroll', handleScroll);
    return () => window.removeEventListener('scroll', handleScroll);
  }, [handleScroll]);

  const handleAddToCart = async (product: Product) => {
    try {
      await addToCart(product, 1);
    } catch (error: any) {
      setError(error.message || 'Failed to add to cart');
    }
  };

  const handleLogout = () => {
    logout();
    navigate('/');
  };

  const getCartItemCount = () => {
    if (!cart) return 0;
    return cart.cartItems.reduce((total, item) => total + item.quantity, 0);
  };

  return (
    <>
      <AppBar position="static">
        <Toolbar>
          <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
            YamSoft Shop - Welcome {user?.username}
          </Typography>
          <IconButton
            color="inherit"
            onClick={() => navigate('/cart')}
          >
            <Badge badgeContent={getCartItemCount()} color="error">
              <ShoppingCart />
            </Badge>
          </IconButton>
          <IconButton color="inherit" onClick={handleLogout}>
            <Logout />
          </IconButton>
        </Toolbar>
      </AppBar>

      <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
        <Typography variant="h4" component="h1" gutterBottom>
          Products
        </Typography>

        {error && (
          <Alert severity="error" sx={{ mb: 2 }}>
            {error}
          </Alert>
        )}

        {loading && !hasLoaded && (
          <Box display="flex" justifyContent="center" my={4}>
            <CircularProgress />
          </Box>
        )}

        <Box
          display="grid"
          gridTemplateColumns={{
            xs: 'repeat(1, 1fr)',
            sm: 'repeat(2, 1fr)',
            md: 'repeat(3, 1fr)',
          }}
          gap={3}
        >
          {products.map((product) => (
            <Card key={product.id} sx={{ height: '100%', display: 'flex', flexDirection: 'column' }}>
              <CardMedia
                component="img"
                height="200"
                image={product.imageUrl || 'https://via.placeholder.com/300x200?text=No+Image'}
                alt={product.name}
              />
              <CardContent sx={{ flexGrow: 1 }}>
                <Typography gutterBottom variant="h6" component="div">
                  {product.name}
                </Typography>
                <Typography variant="body2" color="text.secondary" paragraph>
                  {product.description || 'No description available'}
                </Typography>
                <Typography variant="h6" color="primary">
                  ${product.price.toFixed(2)}
                </Typography>
                <Typography variant="body2" color="text.secondary">
                  Stock: {product.stock}
                </Typography>
              </CardContent>
              <Box sx={{ p: 2 }}>
                <Button
                  variant="contained"
                  fullWidth
                  onClick={() => handleAddToCart(product)}
                  disabled={product.stock === 0}
                >
                  {product.stock === 0 ? 'Out of Stock' : 'Add to Cart'}
                </Button>
              </Box>
            </Card>
          ))}
        </Box>

        {loading && hasLoaded && (
          <Box display="flex" justifyContent="center" my={4}>
            <CircularProgress />
          </Box>
        )}
      </Container>
    </>
  );
};

export default Products;