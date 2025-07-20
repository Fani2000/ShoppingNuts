<script setup lang="ts">
defineProps({
  products: {
    type: Array<any>,
    required: true
  }
});

const emit = defineEmits(['add-to-cart']);

const handleAddToCart = (productId: number) => {
  emit('add-to-cart', productId);
};
</script>

<template>
  <div v-for="product in products" :key="product.id" class="bg-white rounded-xl shadow-md overflow-hidden hover:shadow-lg transition-shadow duration-300">
    <div class="relative">
      <img :src="product.image" :alt="product.name" class="w-full h-48 object-cover" />
      <div class="absolute top-3 right-3 bg-green-100 text-green-800 px-2 py-1 rounded-full text-sm font-medium shadow-sm">
        R {{ product.price }}
      </div>
    </div>
    <div class="p-5">
      <h3 class="text-xl font-bold text-green-800 mb-2">{{ product.name }}</h3>
      <p class="text-gray-600 mb-4">{{ product.description }}</p>
      
      <!-- Tags -->
      <div class="flex flex-wrap gap-2 mb-4">
        <span 
          v-for="(tag, index) in product.tags" 
          :key="index"
          class="bg-green-50 text-green-700 px-3 py-1 rounded-full text-xs font-medium border border-green-100"
        >
          {{ tag }}
        </span>
      </div>
      
      <!-- Add to Cart Button -->
      <button 
        @click="handleAddToCart(product.id)"
        class="w-full bg-green-700 text-white py-2 rounded-lg hover:bg-green-600 transition flex items-center justify-center gap-2 transform hover:-translate-y-1 duration-200"
      >
        <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z"></path>
        </svg>
        Add to Cart
      </button>
    </div>
  </div>
</template>