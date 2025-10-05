import React from 'react';
import {
  Container,
  Typography,
  Box,
  Card,
  CardContent,
  Button,
  IconButton,
  TextField,
  AppBar,
  Toolbar,
  Divider,
  Alert,
  CircularProgress,
} from '@mui/material';
import { ArrowBack, Add, Remove, Delete } from '@mui/icons-material';
import { useCart } from '../context/CartContext';
import { useAuth } from '../context/AuthContext';
import { useNavigate } from 'react-router-dom';

const Cart: React.FC = () => {
  const { cart, loading, removeFromCart, updateCartItem } = useCart();
  const { user } = useAuth();
  const navigate = useNavigate();

  const handleQuantityChange = async (productId: number, newQuantity: number) => {
    if (newQuantity < 1) {
      await removeFromCart(productId);
    } else {
      await updateCartItem(productId, newQuantity);
    }
  };

  const handleRemoveItem = async (productId: number) => {
    await removeFromCart(productId);
  };

  if (loading) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" minHeight="200px">
        <CircularProgress />
      </Box>
    );
  }

  return (
    <>
      <AppBar position="static">
        <Toolbar>
          <IconButton
            edge="start"
            color="inherit"
            onClick={() => navigate('/products')}
            sx={{ mr: 2 }}
          >
            <ArrowBack />
          </IconButton>
          <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
            Shopping Cart - {user?.username}
          </Typography>
        </Toolbar>
      </AppBar>

      <Container maxWidth="md" sx={{ mt: 4, mb: 4 }}>
        <Typography variant="h4" component="h1" gutterBottom>
          Your Cart
        </Typography>

        {!cart || cart.cartItems.length === 0 ? (
          <Alert severity="info">
            Your cart is empty. <Button onClick={() => navigate('/products')}>Continue Shopping</Button>
          </Alert>
        ) : (
          <>
            {cart.cartItems.map((item) => (
              <Card key={item.id} sx={{ mb: 2 }}>
                <CardContent>
                  <Box display="flex" flexDirection={{ xs: 'column', sm: 'row' }} alignItems="center" gap={2}>
                    <Box flex={1}>
                      <Typography variant="h6">{item.product.name}</Typography>
                      <Typography variant="body2" color="text.secondary">
                        {item.product.description || 'No description'}
                      </Typography>
                      <Typography variant="h6" color="primary">
                        ${item.product.price.toFixed(2)}
                      </Typography>
                    </Box>
                    
                    <Box display="flex" alignItems="center" justifyContent="center">
                      <IconButton
                        onClick={() => handleQuantityChange(item.productId, item.quantity - 1)}
                        size="small"
                      >
                        <Remove />
                      </IconButton>
                      <TextField
                        type="number"
                        value={item.quantity}
                        onChange={(e) => {
                          const newQuantity = parseInt(e.target.value) || 0;
                          handleQuantityChange(item.productId, newQuantity);
                        }}
                        sx={{ width: 60, mx: 1 }}
                        inputProps={{ min: 0, style: { textAlign: 'center' } }}
                      />
                      <IconButton
                        onClick={() => handleQuantityChange(item.productId, item.quantity + 1)}
                        size="small"
                      >
                        <Add />
                      </IconButton>
                    </Box>
                    
                    <Box minWidth={100} textAlign="center">
                      <Typography variant="h6">
                        ${(item.product.price * item.quantity).toFixed(2)}
                      </Typography>
                    </Box>
                    
                    <Box>
                      <IconButton
                        onClick={() => handleRemoveItem(item.productId)}
                        color="error"
                      >
                        <Delete />
                      </IconButton>
                    </Box>
                  </Box>
                </CardContent>
              </Card>
            ))}

            <Divider sx={{ my: 2 }} />
            
            <Box display="flex" justifyContent="space-between" alignItems="center" sx={{ mt: 2 }}>
              <Typography variant="h5">
                Total: ${cart.totalAmount.toFixed(2)}
              </Typography>
              <Button
                variant="contained"
                size="large"
                onClick={() => {
                  // Checkout functionality would go here
                  alert('Checkout functionality not implemented in this demo');
                }}
              >
                Proceed to Checkout
              </Button>
            </Box>
          </>
        )}
      </Container>
    </>
  );
};

export default Cart;