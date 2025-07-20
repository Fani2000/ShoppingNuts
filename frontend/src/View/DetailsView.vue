<script setup lang="ts">
import MinHeader from '../Components/MinHeader.vue';
import { ref, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { cartItemsApi } from '../services/api_service';

// Get the product ID from the route
const route = useRoute();
const router = useRouter();
const productId = route.params.id;

// Define product state
const product = ref({
  id: 0,
  name: '',
  category: '',
  description: '',
  size: '',
  price: 0,
  image: '',
  quantity: 1
});

const loading = ref(true);
const error = ref('');
const addedToCart = ref(false);

// Fetch product details on component mount
onMounted(async () => {
  if (!productId) {
    error.value = 'Product ID is missing';
    loading.value = false;
    return;
  }

  try {
    const response = await cartItemsApi.getById(Number(productId));
    product.value = response.data;
    loading.value = false;
  } catch (err) {
    console.error('Error fetching product details:', err);
    error.value = 'Failed to load product details';
    loading.value = false;
  }
});

// Handle add to cart without navigating away
const addToCart = async () => {
  try {
    if (product.value.id) {
      // If product already exists in cart, update quantity
      await cartItemsApi.update(product.value.id, {
        ...product.value,
        quantity: product.value.quantity
      });
    } else {
      // Otherwise add as new item
      await cartItemsApi.create({
        ...product.value,
        quantity: product.value.quantity
      });
    }
    
    // Show success message instead of navigating
    addedToCart.value = true;
    
    // Hide success message after 3 seconds
    setTimeout(() => {
      addedToCart.value = false;
    }, 3000);
    
  } catch (err) {
    console.error('Error adding to cart:', err);
    error.value = 'Failed to add product to cart';
  }
};

// Navigate to cart
const viewCart = () => {
  router.push('/add-to-cart');
};

// Format price to currency
const formatPrice = (price: number) => {
  return `R${price.toFixed(2)}`;
};
</script>

<template>
  <div class="details-view">
    <!-- Mini Header -->
    <MinHeader title="Product Details" />
    
    <div class="container mx-auto px-4 py-8">
      <!-- Loading State -->
      <div v-if="loading" class="flex justify-center items-center h-64">
        <div class="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-green-600"></div>
      </div>
      
      <!-- Error State -->
      <div v-else-if="error" class="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded max-w-4xl mx-auto">
        <p>{{ error }}</p>
        <button @click="router.push('/products')" class="mt-2 text-green-600 hover:underline">
          Back to Products
        </button>
      </div>
      
      <!-- Product Details Card -->
      <div v-else class="bg-white shadow-lg rounded-2xl overflow-hidden max-w-4xl mx-auto border border-green-100">
        <div class="md:flex">
          <!-- Product Image -->
          <div class="md:w-1/2 bg-green-50">
            <img :src="product.image" :alt="product.name" class="w-full h-full object-cover" />
          </div>

          <!-- Product Info -->
          <div class="md:w-1/2 p-8">
            <h2 class="text-3xl font-bold text-green-800 mb-2">{{ product.name }}</h2>
            
            <!-- Product Size and Category -->
            <div class="mb-4">
              <span class="bg-green-200 text-green-800 text-sm px-3 py-1 rounded-full">
                {{ product.size }}
              </span>
              <span v-if="product.category" class="ml-2 bg-green-100 text-green-800 text-sm px-3 py-1 rounded-full">
                {{ product.category }}
              </span>
            </div>
            
            <!-- Product Price -->
            <div class="text-2xl font-bold text-green-700 mb-4">
              {{ formatPrice(product.price) }}
            </div>
            
            <!-- Product Description -->
            <p class="text-gray-700 leading-relaxed mb-6">
              {{ product.description }}
            </p>

            <!-- Nutritional Info -->
            <div class="bg-green-50 p-4 rounded-xl mb-8 border-l-4 border-green-600">
              <h3 class="font-medium text-green-800 mb-2">Nutritional Benefits:</h3>
              <ul class="text-gray-700 text-sm space-y-1">
                <li>• Rich in healthy fats and protein</li>
                <li>• Good source of essential vitamins and minerals</li>
                <li>• Excellent for heart health</li>
              </ul>
            </div>

            <!-- Added to cart notification -->
            <div v-if="addedToCart" class="bg-green-100 text-green-800 p-3 rounded-lg mb-4 flex items-center">
              <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 mr-2" viewBox="0 0 20 20" fill="currentColor">
                <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clip-rule="evenodd" />
              </svg>
              Added to cart successfully!
            </div>

            <!-- Quantity Selector -->
            <div class="flex items-center mb-6">
              <span class="mr-3 text-gray-700">Quantity:</span>
              <div class="flex items-center border border-gray-200 rounded">
                <button 
                  @click="product.quantity > 1 ? product.quantity-- : 1" 
                  class="px-3 py-1 bg-gray-100 text-gray-600 hover:bg-gray-200"
                >
                  -
                </button>
                <span class="px-4 py-1">{{ product.quantity }}</span>
                <button 
                  @click="product.quantity++" 
                  class="px-3 py-1 bg-gray-100 text-gray-600 hover:bg-gray-200"
                >
                  +
                </button>
              </div>
            </div>

            <!-- Action Buttons -->
            <div class="flex flex-wrap gap-3">
            <button 
                @click="addToCart" 
                class="bg-green-600 px-6 py-3 text-white rounded-full hover:bg-green-500 transition shadow-md hover:shadow-lg font-medium transform hover:-translate-y-1"
              >
                Add to Cart
              </button>
              
              <button 
                @click="viewCart" 
                class="bg-white border-2 border-green-600 px-6 py-3 text-green-600 rounded-full hover:bg-green-50 transition shadow-md hover:shadow-lg font-medium transform hover:-translate-y-1"
              >
                View Cart
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>