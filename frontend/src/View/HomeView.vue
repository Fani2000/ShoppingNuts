<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { cartItemsApi, productsApi } from '../services/api_service';
import Header from '../Components/Header.vue'

interface Product {
  id: number;
  name: string;
  description: string;
  image: string;
  tags: string[];
  price?: number;
}

const products = ref<Product[]>([]);
const loading = ref(true);
const error = ref('');

// Add a product to cart
const addToCart = async (productId: number) => {
  try {
    const product = products.value.find(p => p.id === productId);
    if (!product) return;
    
    // Check if product is already in cart
    const cartResponse = await cartItemsApi.getAll();
    const existingItem = cartResponse.data.find((item: any) => item.id === productId);
    
    if (existingItem) {
      // Update quantity if already in cart
      await cartItemsApi.update(productId, {
        ...existingItem,
        quantity: existingItem.quantity + 1
      });
    } else {
      // Add new item to cart
      await cartItemsApi.create({
        id: product.id,
        name: product.name,
        quantity: 1,
        price: product.price || 9.99, // Default price if not available
        image: product.image
      });
    }
    
    alert(`${product.name} added to cart!`);
  } catch (err) {
    console.error('Error adding to cart:', err);
    error.value = 'Failed to add item to cart. Please try again.';
  }
};

// Fetch products data
const fetchProducts = async () => {
  try {
    loading.value = true;
    // For now, we're using cartItems API as a source of products
    // In a real app, you'd create a dedicated products API endpoint
    const response = await cartItemsApi.getAll();
    
    // Transform cart items into product format or use an empty array if no data
    if (response.data && response.data.length > 0) {
      console.log("products: ", response.data.slice(0,6))
      products.value = response.data.slice(0,6).map((item: any) => ({
        id: item.id,
        name: item.name,
        description: item.description,
        image: item.image,
        price: item.price,
        tags: ["Healthy", "Natural", "Premium"]
      }));
    } else {
      // Fallback to sample data if no products in cart
      const response = await productsApi.getAllFallback();
      products.value = response.data;
    }
  } catch (err) {
    console.error('Error fetching products:', err);
    error.value = 'Failed to load products. Please try again later.';
    
    // Fallback to sample data if API fails
    products.value = [
      {
        id: 1,
        name: "Almonds",
        description: "Rich in nutrients, an excellent source of fiber and healthy fats.",
        image: "https://via.placeholder.com/300x200?text=Almonds",
        price: 15.99,
        tags: ["Healthy", "Fiber", "Energy"],
      },
      {
        id: 2,
        name: "Cashews",
        description: "A delicious, creamy snack packed with essential vitamins.",
        image: "https://via.placeholder.com/300x200?text=Cashews",
        price: 12.49,
        tags: ["Creamy", "Snack", "Magnesium"],
      },
      {
        id: 3,
        name: "Walnuts",
        description: "Boosts brain health, a perfect source of omega-3 fatty acids.",
        image: "https://via.placeholder.com/300x200?text=Walnuts",
        price: 18.29,
        tags: ["Omega-3", "Brain Food", "Crunchy"],
      }
    ];
  } finally {
    loading.value = false;
  }
};

// Load data when component mounts
onMounted(() => {
  fetchProducts();
});
</script>

<template>
  <div>
    <!-- Heading -->
    <Header />

    <!-- Loading State -->
    <div v-if="loading" class="container mx-auto text-center py-10">
      <div class="w-16 h-16 border-t-4 border-green-500 border-solid rounded-full animate-spin mx-auto"></div>
      <p class="mt-4 text-gray-600">Loading products...</p>
    </div>

    <!-- Error State -->
    <div v-else-if="error" class="container mx-auto bg-red-50 text-red-700 p-4 rounded-lg my-8 mx-4 text-center">
      {{ error }}
      <button @click="fetchProducts" class="ml-2 text-red-800 underline">Try Again</button>
    </div>

    <!-- Products Grid -->
    <div v-else class="container mx-auto px-4 py-12">
      <h2 class="text-3xl font-bold text-green-800 mb-8 text-center">Our Premium Nuts</h2>
      
      <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
        <div v-for="product in products" :key="product.id" class="bg-white rounded-2xl shadow-lg overflow-hidden border border-green-100 transition-transform hover:scale-105">
          <!-- Product Image -->
          <div class="h-48 bg-green-50 flex items-center justify-center p-4">
            <img :src="product.image" :alt="product.name" class="h-full object-contain" />
          </div>
          
          <!-- Product Details -->
          <div class="p-6">
            <div class="flex justify-between items-start">
              <h3 class="text-xl font-bold text-green-800">{{ product.name }}</h3>
              <span class="bg-green-100 text-green-800 px-3 py-1 rounded-full font-bold">R{{ product.price?.toFixed(2) }}</span>
            </div>
            
            <p class="text-gray-600 mt-2">{{ product.description }}</p>
            
            <!-- Tags -->
            <div class="flex flex-wrap gap-2 mt-4">
              <span v-for="(tag, index) in product.tags" :key="index" 
                    class="bg-green-50 text-green-700 px-3 py-1 rounded-full text-sm">
                {{ tag }}
              </span>
            </div>
            
            <!-- Action Buttons -->
            <div class="mt-6 flex gap-3">
              <!-- View Details Button -->
              <router-link :to="`/products/${product.id}`" class="flex-1 px-4 py-2 rounded-full text-green-800 border border-green-300 hover:bg-green-100 transition text-center font-medium">
                View Details
              </router-link>
              
              <!-- Add to Cart Button -->
              <button @click="addToCart(product.id)" 
                      class="flex-1 bg-green-700 px-4 py-2 text-white rounded-full hover:bg-green-600 transition shadow-md text-center font-medium">
                Add to Cart
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>