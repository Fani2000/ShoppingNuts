<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { cartItemsApi, ordersApi } from '../services/api_service';
import MinHeader from '../Components/MinHeader.vue';

interface OrderItem {
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

interface Order {
  id: string;
  date: string;
  total: number;
  items: OrderItem[];
  status: string;
  estimatedDelivery: string;
  shippingAddress: ShippingAddress;
}

const route = useRoute();
const router = useRouter();
const orderId = route.params.id?.toString() || '';

// Default shipping address
const defaultAddress: ShippingAddress = {
  fullName: '',
  streetAddress: '',
  city: '',
  state: '',
  zipCode: '',
  phone: ''
};

const order = ref<Order>({
  id: orderId,
  date: new Date().toISOString().split('T')[0],
  total: 0,
  items: [],
  status: 'Processing',
  estimatedDelivery: new Date(Date.now() + 5 * 24 * 60 * 60 * 1000).toLocaleDateString(),
  shippingAddress: defaultAddress
});

const loading = ref(true);
const error = ref('');
const addressSaving = ref(false);
const addressSaved = ref(false);

// Get delivery status steps
const deliverySteps = [
  { name: 'Order Placed', status: 'completed' },
  { name: 'Processing', status: 'current' },
  { name: 'Shipped', status: 'upcoming' },
  { name: 'Delivered', status: 'upcoming' }
];

// Fetch order details from the API
const fetchOrderDetails = async () => {
  try {
    loading.value = true;
    
    // Try to get order details from the API
    try {
      const response = await ordersApi.getById(orderId);
      order.value = response.data.slice(0,6);
      
      // If no shipping address exists in the order, check localStorage as fallback
      if (!order.value.shippingAddress) {
        const savedAddress = localStorage.getItem(`order_${orderId}_address`);
        if (savedAddress) {
          order.value.shippingAddress = JSON.parse(savedAddress);
        }
      }
    } catch (err) {
      console.warn('Order not found in API, using cart items as fallback');
      // In a real application, you would call an order API with the order ID
      // For now, we'll simulate it with cart items API
      const response = await cartItemsApi.getAll();
      
      order.value.items = response.data.slice(0,6);
      // Calculate total price from items
      order.value.total = order.value.items.reduce((sum, item) => 
        sum + (item.price * item.quantity), 0);
      
      // Check localStorage for address as fallback
      const savedAddress = localStorage.getItem(`order_${orderId}_address`);
      if (savedAddress) {
        order.value.shippingAddress = JSON.parse(savedAddress);
      }
    }
  } catch (err) {
    console.error('Error fetching order details:', err);
    error.value = 'Failed to load order details. Please try again later.';
  } finally {
    loading.value = false;
  }
};

// Save order with shipping address to API
const saveOrderWithAddress = async () => {
  try {
    addressSaving.value = true;
    
    // Create order object with items and shipping address
    const orderData = {
      id: orderId,
      date: new Date().toISOString(),
      total: order.value.total,
      items: order.value.items,
      shippingAddress: order.value.shippingAddress
    };
    
    // Update or create the order with shipping address
    if (orderId) {
      await ordersApi.update(orderId, orderData);
    } else {
      const response = await ordersApi.create(orderData);
      // Update the order ID if this is a new order
      order.value.id = response.data.id;
    }
    
    // Also save to localStorage as backup
    localStorage.setItem(`order_${orderId}_address`, JSON.stringify(order.value.shippingAddress));
    
    addressSaved.value = true;
    setTimeout(() => {
      addressSaved.value = false;
    }, 3000);
    
    return true;
  } catch (err) {
    console.error('Error saving order with address:', err);
    error.value = 'Failed to save shipping address. Please try again.';
    return false;
  } finally {
    addressSaving.value = false;
  }
};

// Function to handle shipping address form submission
const submitAddressForm = async () => {
  await saveOrderWithAddress();
};

// Add a function to confirm the order and navigate to confirmation
const confirmOrder = async () => {
  // Save the order with shipping address
  const success = await saveOrderWithAddress();
  
  if (success) {
    // Navigate to confirmation page with order ID
    router.push(`/confirmation?orderId=${orderId}`);
  }
};

onMounted(() => {
  if (orderId) {
    fetchOrderDetails();
  } else {
    error.value = 'Invalid order ID';
    loading.value = false;
  }
});
</script>

<template>
  <div>
    <MinHeader title="Order Details" />
    
    <div class="container mx-auto px-4 py-8 max-w-5xl">
      <!-- Loading State -->
      <div v-if="loading" class="text-center py-10">
        <div class="w-16 h-16 border-t-4 border-green-500 border-solid rounded-full animate-spin mx-auto"></div>
        <p class="mt-4 text-gray-600">Loading your order details...</p>
      </div>

      <!-- Error State -->
      <div v-else-if="error" class="bg-red-50 text-red-700 p-4 rounded-lg text-center">
        {{ error }}
        <button v-if="orderId" @click="fetchOrderDetails" class="ml-2 text-red-800 underline">
          Try Again
        </button>
      </div>

      <div v-else-if="order.items.length > 0" class="grid grid-cols-1 lg:grid-cols-3 gap-8">
        <div class="lg:col-span-2">
          <!-- Order Status -->
          <div class="bg-white shadow-lg rounded-2xl p-6 border border-green-100 mb-8">
            <div class="flex items-center justify-between mb-6">
              <h2 class="text-xl font-bold text-green-800">Order Status</h2>
              <div class="bg-green-100 text-green-800 px-4 py-2 rounded-full font-medium text-sm">
                {{ order.status }}
              </div>
            </div>
            
            <!-- Progress Timeline -->
            <div class="relative">
              <div class="absolute inset-0 flex items-center">
                <div class="h-1 w-full bg-green-200 rounded"></div>
              </div>
              
              <div class="relative flex justify-between">
                <div v-for="(step, index) in deliverySteps" :key="index" class="text-center">
                  <div class="flex items-center justify-center">
                    <!-- Green circle for completed steps -->
                    <div v-if="step.status === 'completed'" 
                         class="h-8 w-8 rounded-full bg-green-500 flex items-center justify-center text-white">
                      <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
                        <path fill-rule="evenodd" d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z" clip-rule="evenodd" />
                      </svg>
                    </div>
                    
                    <!-- Blue circle for current step -->
                    <div v-else-if="step.status === 'current'" 
                         class="h-8 w-8 rounded-full bg-blue-500 flex items-center justify-center text-white">
                      <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
                        <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm1-12a1 1 0 10-2 0v4a1 1 0 00.293.707l2.828 2.829a1 1 0 101.415-1.415L11 9.586V6z" clip-rule="evenodd" />
                      </svg>
                    </div>
                    
                    <!-- Gray circle for upcoming steps -->
                    <div v-else class="h-8 w-8 rounded-full bg-gray-300 flex items-center justify-center text-white">
                      <span class="text-sm font-medium">{{ index + 1 }}</span>
                    </div>
                  </div>
                  <div class="text-xs mt-2">{{ step.name }}</div>
                </div>
              </div>
            </div>
            
            <div class="mt-6 text-gray-600 text-sm">
              <p>Estimated delivery: <span class="font-medium">{{ order.estimatedDelivery }}</span></p>
            </div>
          </div>
          
          <!-- Order Items -->
          <div class="bg-white shadow-lg rounded-2xl p-6 border border-green-100">
            <h2 class="text-xl font-bold text-green-800 mb-6">Order Items</h2>
            
            <div v-for="item in order.items" :key="item.id" class="flex items-center border-b border-gray-100 py-4">
              <div class="w-16 h-16 bg-gray-100 rounded-lg overflow-hidden flex-shrink-0">
                <img v-if="item.image" :src="item.image" alt="Product" class="w-full h-full object-cover" />
                <div v-else class="w-full h-full flex items-center justify-center text-gray-400">
                  <svg xmlns="http://www.w3.org/2000/svg" class="h-8 w-8" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16l4.586-4.586a2 2 0 012.828 0L16 16m-2-2l1.586-1.586a2 2 0 012.828 0L20 14m-6-6h.01M6 20h12a2 2 0 002-2V6a2 2 0 00-2-2H6a2 2 0 00-2 2v12a2 2 0 002 2z" />
                  </svg>
                </div>
              </div>
              
              <div class="ml-4 flex-grow">
                <h3 class="font-medium text-gray-800">{{ item.name }}</h3>
                <p class="text-gray-500 text-sm">Quantity: {{ item.quantity }}</p>
              </div>
              
              <div class="text-right">
                <p class="font-medium text-gray-800">${{ (item.price * item.quantity).toFixed(2) }}</p>
                <p class="text-gray-500 text-sm">${{ item.price.toFixed(2) }} each</p>
              </div>
            </div>
            
            <div class="mt-6 flex justify-between border-t border-gray-100 pt-4">
              <span class="font-medium">Total:</span>
              <span class="font-bold text-xl text-green-800">${{ order.total.toFixed(2) }}</span>
            </div>
          </div>
        </div>
        
        <!-- Shipping Address Form -->
        <div class="bg-white shadow-lg rounded-2xl p-6 border border-green-100 h-fit">
          <h2 class="text-xl font-bold text-green-800 mb-6">Shipping Address</h2>
          
          <form @submit.prevent="submitAddressForm">
            <div class="space-y-4">
              <div>
                <label for="fullName" class="block text-sm font-medium text-gray-700 mb-1">Full Name</label>
                <input 
                  type="text" 
                  id="fullName" 
                  v-model="order.shippingAddress.fullName" 
                  class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                  required
                />
              </div>
              
              <div>
                <label for="streetAddress" class="block text-sm font-medium text-gray-700 mb-1">Street Address</label>
                <input 
                  type="text" 
                  id="streetAddress" 
                  v-model="order.shippingAddress.streetAddress" 
                  class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                  required
                />
              </div>
              
              <div class="grid grid-cols-2 gap-4">
                <div>
                  <label for="city" class="block text-sm font-medium text-gray-700 mb-1">City</label>
                  <input 
                    type="text" 
                    id="city" 
                    v-model="order.shippingAddress.city" 
                    class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                    required
                  />
                </div>
                <div>
                  <label for="state" class="block text-sm font-medium text-gray-700 mb-1">State</label>
                  <input 
                    type="text" 
                    id="state" 
                    v-model="order.shippingAddress.state" 
                    class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                    required
                  />
                </div>
              </div>
              
              <div class="grid grid-cols-2 gap-4">
                <div>
                  <label for="zipCode" class="block text-sm font-medium text-gray-700 mb-1">ZIP Code</label>
                  <input 
                    type="text" 
                    id="zipCode" 
                    v-model="order.shippingAddress.zipCode" 
                    class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                    required
                  />
                </div>
                <div>
                  <label for="phone" class="block text-sm font-medium text-gray-700 mb-1">Phone Number</label>
                  <input 
                    type="tel" 
                    id="phone" 
                    v-model="order.shippingAddress.phone" 
                    class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                  />
                </div>
              </div>
              
              <div class="pt-4">
                <button 
                  type="submit" 
                  class="w-full bg-green-600 hover:bg-green-700 text-white py-2 px-4 rounded-lg transition-colors"
                  :disabled="addressSaving"
                >
                  <span v-if="addressSaving">Saving...</span>
                  <span v-else>Save Address</span>
                </button>
                
                <div v-if="addressSaved" class="mt-2 text-green-600 text-sm text-center">
                  Address saved successfully!
                </div>
              </div>
            </div>
          </form>
          
          <div class="mt-6 pt-6 border-t border-gray-100">
            <button 
              @click="confirmOrder" 
              class="w-full bg-blue-600 hover:bg-blue-700 text-white py-3 px-4 rounded-lg transition-colors font-medium"
              :disabled="!order.shippingAddress.fullName || addressSaving"
            >
              Confirm Order
            </button>
          </div>
        </div>
      </div>
      
      <!-- Empty State -->
      <div v-else class="text-center py-10">
        <div class="text-gray-400 mb-4">
          <svg xmlns="http://www.w3.org/2000/svg" class="h-16 w-16 mx-auto" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 11V7a4 4 0 00-8 0v4M5 9h14l1 12H4L5 9z" />
          </svg>
        </div>
        <h3 class="text-lg font-medium text-gray-700 mb-2">No order found</h3>
        <p class="text-gray-500">There are no items in this order.</p>
      </div>
    </div>
  </div>
</template>