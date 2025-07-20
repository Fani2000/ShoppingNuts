<script setup lang="ts">
import { ref, onMounted, computed } from 'vue';
import { cartItemsApi } from '../services/api_service';
import { useRouter } from 'vue-router';

interface CartItem {
  id: number;
  name: string;
  quantity: number;
  price: number;
  image: string;
}

interface ShippingAddress {
  fullName: string;
  streetAddress: string;
  city: string;
  state: string;
  zipCode: string;
  phone: string;
}

const router = useRouter();
const cartItems = ref<CartItem[]>([]);
const loading = ref(true);
const error = ref('');
const processingOrder = ref(false);
const showAddressForm = ref(false);

// Shipping address information
const shippingAddress = ref<ShippingAddress>({
  fullName: '',
  streetAddress: '',
  city: '',
  state: '',
  zipCode: '',
  phone: ''
});

// Form validation
const addressErrors = ref({
  fullName: '',
  streetAddress: '',
  city: '',
  state: '',
  zipCode: '',
  phone: ''
});

// Calculate total price
const totalPrice = computed(() => {
  return cartItems.value
    .reduce((sum, item) => sum + item.price * item.quantity, 0)
    .toFixed(2);
});

// Format price to currency
const formatPrice = (price: number) => {
  return `R${price.toFixed(2)}`;
};

// Fetch cart items from the backend
const fetchCartItems = async () => {
  try {
    loading.value = true;
    const response = await cartItemsApi.getAll();
  cartItems.value = response.data.slice(0, 6);
  } catch (err) {
    console.error('Error fetching cart items:', err);
    error.value = 'Failed to load cart items. Please try again later.';
  } finally {
    loading.value = false;
  }
};

// Remove item from cart
const removeItem = async (id: number) => {
  try {
    await cartItemsApi.delete(id);
    // Remove item from local state
    cartItems.value = cartItems.value.filter(item => item.id !== id);
  } catch (err) {
    console.error('Error removing item:', err);
    error.value = 'Failed to remove item. Please try again.';
  }
};

// Update item quantity
const updateQuantity = async (id: number, newQuantity: number) => {
  if (newQuantity < 1) return;
  
  try {
    const item = cartItems.value.find(item => item.id === id);
    if (!item) return;
    
    const updatedItem = { ...item, quantity: newQuantity };
    await cartItemsApi.update(id, updatedItem);
    
    // Update local state
    const index = cartItems.value.findIndex(item => item.id === id);
    cartItems.value[index].quantity = newQuantity;
  } catch (err) {
    console.error('Error updating quantity:', err);
    error.value = 'Failed to update quantity. Please try again.';
  }
};

// View product details
const viewProductDetails = (id: number) => {
  router.push(`/products/${id}`);
};

// Open address form
const proceedToAddress = () => {
  if (cartItems.value.length === 0) return;
  showAddressForm.value = true;
};

// Validate address form
const validateAddressForm = () => {
  let isValid = true;
  
  // Reset errors
  Object.keys(addressErrors.value).forEach(key => {
    addressErrors.value[key as keyof typeof addressErrors.value] = '';
  });
  
  // Validate full name
  if (!shippingAddress.value.fullName.trim()) {
    addressErrors.value.fullName = 'Full name is required';
    isValid = false;
  }
  
  // Validate street address
  if (!shippingAddress.value.streetAddress.trim()) {
    addressErrors.value.streetAddress = 'Street address is required';
    isValid = false;
  }
  
  // Validate city
  if (!shippingAddress.value.city.trim()) {
    addressErrors.value.city = 'City is required';
    isValid = false;
  }
  
  // Validate state
  if (!shippingAddress.value.state.trim()) {
    addressErrors.value.state = 'State is required';
    isValid = false;
  }
  
  // Validate zip code
  if (!shippingAddress.value.zipCode.trim()) {
    addressErrors.value.zipCode = 'ZIP code is required';
    isValid = false;
  } else if (!/^\d{4}(-\d{4})?$/.test(shippingAddress.value.zipCode.trim())) {
    addressErrors.value.zipCode = 'Please enter a valid ZIP code';
    isValid = false;
  }
  
  // Validate phone
  if (!shippingAddress.value.phone.trim()) {
    addressErrors.value.phone = 'Phone number is required';
    isValid = false;
  } else if (!/^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$/.test(shippingAddress.value.phone.trim())) {
    addressErrors.value.phone = 'Please enter a valid phone number';
    isValid = false;
  }
  
  return isValid;
};

// Process the order
const processOrder = async () => {
  if (!validateAddressForm()) return;
  
  try {
    processingOrder.value = true;
    
    // Here you would send the order to your backend
    // For now, we'll just simulate success and redirect
    
    // Clear cart items
    for (const item of cartItems.value) {
      await cartItemsApi.delete(item.id);
    }
    
    // Redirect to confirmation page
    router.push('/confirmation');
  } catch (err) {
    console.error('Error processing order:', err);
    error.value = 'Failed to process your order. Please try again.';
  } finally {
    processingOrder.value = false;
  }
};

// Fetch cart items on component mount
onMounted(() => {
  fetchCartItems();
});
</script>

<template>
  <div class="cart-view min-h-screen bg-gray-50">
    <div class="container mx-auto px-4 py-8">
      <h1 class="text-3xl font-bold text-center text-green-800 mb-8">Your Cart</h1>
      
      <!-- Loading State -->
      <div v-if="loading" class="flex justify-center items-center h-64">
        <div class="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-green-600"></div>
      </div>
      
      <!-- Error State -->
      <div v-else-if="error" class="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded">
        <p>{{ error }}</p>
      </div>
      
      <!-- Empty Cart State -->
      <div v-else-if="cartItems.length === 0" class="text-center py-12">
        <svg xmlns="http://www.w3.org/2000/svg" class="h-16 w-16 mx-auto text-gray-400 mb-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z" />
        </svg>
        <h2 class="text-xl font-semibold text-gray-700 mb-2">Your cart is empty</h2>
        <p class="text-gray-500 mb-6">Looks like you haven't added any items to your cart yet.</p>
        <button 
          @click="router.push('/')" 
          class="bg-green-600 text-white px-6 py-2 rounded-full hover:bg-green-500 transition"
        >
          Browse Products
        </button>
      </div>
      
      <!-- Cart Items -->
      <div v-else-if="!showAddressForm" class="max-w-4xl mx-auto">
        <div class="bg-white rounded-lg shadow-md p-6 mb-6">
          <h2 class="text-xl font-semibold text-green-800 mb-4">Cart Items</h2>
          
          <!-- Item List -->
          <div class="space-y-4">
            <div 
              v-for="item in cartItems" 
              :key="item.id" 
              class="flex flex-col sm:flex-row items-center p-4 border border-gray-200 rounded-lg hover:shadow-md transition"
            >
              <!-- Product Image -->
              <div class="w-24 h-24 flex-shrink-0 bg-gray-100 rounded-lg overflow-hidden mb-4 sm:mb-0">
                <img :src="item.image" :alt="item.name" class="w-full h-full object-cover" />
              </div>
              
              <!-- Product Info -->
              <div class="flex-grow sm:ml-6 text-center sm:text-left">
                <h3 class="font-medium text-gray-800">{{ item.name }}</h3>
                <p class="text-gray-500 text-sm">{{ formatPrice(item.price) }} each</p>
              </div>
              
              <!-- Quantity Controls -->
              <div class="flex items-center mt-4 sm:mt-0">
                <button 
                  @click="updateQuantity(item.id, item.quantity - 1)" 
                  class="w-8 h-8 flex items-center justify-center bg-gray-100 rounded-full text-gray-600 hover:bg-gray-200"
                >
                  -
                </button>
                <span class="mx-3 w-8 text-center">{{ item.quantity }}</span>
                <button 
                  @click="updateQuantity(item.id, item.quantity + 1)" 
                  class="w-8 h-8 flex items-center justify-center bg-gray-100 rounded-full text-gray-600 hover:bg-gray-200"
                >
                  +
                </button>
              </div>
              
              <!-- Item Total and Actions -->
              <div class="flex flex-col items-end ml-6 mt-4 sm:mt-0">
                <span class="font-semibold text-gray-800">{{ formatPrice(item.price * item.quantity) }}</span>
                <div class="flex mt-2 space-x-2">
                  <button 
                    @click="viewProductDetails(item.id)" 
                    class="text-green-600 hover:text-green-500 text-sm underline"
                  >
                    View Details
                  </button>
                  <button 
                    @click="removeItem(item.id)" 
                    class="text-red-600 hover:text-red-500 text-sm"
                  >
                    Remove
                  </button>
                </div>
              </div>
            </div>
          </div>
          
          <!-- Cart Summary -->
          <div class="mt-8 border-t pt-6">
            <div class="flex justify-between items-center font-semibold text-lg text-gray-800 mb-6">
              <span>Total:</span>
              <span class="text-green-600">R{{ totalPrice }}</span>
            </div>
            
            <button 
              @click="proceedToAddress" 
              class="w-full bg-green-600 text-white py-3 rounded-lg hover:bg-green-500 transition font-medium"
              :disabled="cartItems.length === 0"
            >
              Proceed to Checkout
            </button>
          </div>
        </div>
      </div>
      
      <!-- Shipping Address Form -->
      <div v-else class="max-w-3xl mx-auto">
        <div class="bg-white rounded-lg shadow-md p-6">
          <h2 class="text-xl font-semibold text-green-800 mb-4">Shipping Address</h2>
          
          <!-- Form Fields -->
          <div class="space-y-4">
            <!-- Full Name -->
            <div>
              <label for="fullName" class="block text-gray-700 mb-1">Full Name</label>
              <input 
                id="fullName"
                v-model="shippingAddress.fullName"
                type="text"
                class="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-green-500"
                placeholder="John Doe"
              />
              <p v-if="addressErrors.fullName" class="text-red-500 text-sm mt-1">{{ addressErrors.fullName }}</p>
            </div>
            
            <!-- Street Address -->
            <div>
              <label for="streetAddress" class="block text-gray-700 mb-1">Street Address</label>
              <input 
                id="streetAddress"
                v-model="shippingAddress.streetAddress"
                type="text"
                class="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-green-500"
                placeholder="123 Main St"
              />
              <p v-if="addressErrors.streetAddress" class="text-red-500 text-sm mt-1">{{ addressErrors.streetAddress }}</p>
            </div>
            
            <!-- City and State -->
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <label for="city" class="block text-gray-700 mb-1">City</label>
                <input 
                  id="city"
                  v-model="shippingAddress.city"
                  type="text"
                  class="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-green-500"
                  placeholder="Cape Town"
                />
                <p v-if="addressErrors.city" class="text-red-500 text-sm mt-1">{{ addressErrors.city }}</p>
              </div>
              
              <div>
                <label for="state" class="block text-gray-700 mb-1">Province</label>
                <input 
                  id="state"
                  v-model="shippingAddress.state"
                  type="text"
                  class="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-green-500"
                  placeholder="Western Cape"
                />
                <p v-if="addressErrors.state" class="text-red-500 text-sm mt-1">{{ addressErrors.state }}</p>
              </div>
            </div>
            
            <!-- Zip Code and Phone -->
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <label for="zipCode" class="block text-gray-700 mb-1">Postal Code</label>
                <input 
                  id="zipCode"
                  v-model="shippingAddress.zipCode"
                  type="text"
                  class="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-green-500"
                  placeholder="8001"
                />
                <p v-if="addressErrors.zipCode" class="text-red-500 text-sm mt-1">{{ addressErrors.zipCode }}</p>
              </div>
              
              <div>
                <label for="phone" class="block text-gray-700 mb-1">Phone Number</label>
                <input 
                  id="phone"
                  v-model="shippingAddress.phone"
                  type="tel"
                  class="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-green-500"
                  placeholder="(012) 345-6789"
                />
                <p v-if="addressErrors.phone" class="text-red-500 text-sm mt-1">{{ addressErrors.phone }}</p>
              </div>
            </div>
          </div>
          
          <!-- Order Summary -->
          <div class="mt-8 border-t pt-6">
            <h3 class="font-semibold text-gray-800 mb-3">Order Summary</h3>
            <div class="flex justify-between text-sm text-gray-600 mb-2">
              <span>Subtotal ({{ cartItems.length }} items):</span>
              <span>R{{ totalPrice }}</span>
            </div>
            <div class="flex justify-between text-sm text-gray-600 mb-2">
              <span>Shipping:</span>
              <span>Free</span>
            </div>
            <div class="flex justify-between font-semibold text-lg text-gray-800 mt-4 mb-6">
              <span>Total:</span>
              <span class="text-green-600">R{{ totalPrice }}</span>
            </div>
            
            <!-- Action Buttons -->
            <div class="flex flex-col sm:flex-row sm:justify-between space-y-3 sm:space-y-0">
              <button 
                @click="showAddressForm = false" 
                class="px-6 py-2 border border-green-600 text-green-600 rounded-lg hover:bg-green-50 transition"
              >
                Back to Cart
              </button>
              
              <button 
                @click="processOrder" 
                class="px-6 py-2 bg-green-600 text-white rounded-lg hover:bg-green-500 transition"
                :disabled="processingOrder"
              >
                <span v-if="processingOrder">Processing...</span>
                <span v-else>Place Order</span>
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>