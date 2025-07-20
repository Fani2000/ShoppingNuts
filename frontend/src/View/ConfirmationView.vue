<script setup lang="ts">
import { ref } from 'vue';
import { useRoute } from 'vue-router';

const route = useRoute();
const orderId = ref(route.query.orderId || '123456');
</script>

<template>
  <div class="container mx-auto px-4 py-16">
    <!-- Success Message -->
    <div class="bg-green-50 border border-green-200 rounded-2xl shadow-lg text-center p-10 max-w-lg mx-auto">
      <div class="bg-green-100 w-20 h-20 rounded-full flex items-center justify-center mx-auto mb-6">
        <svg xmlns="http://www.w3.org/2000/svg" class="h-10 w-10 text-green-700" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" />
        </svg>
      </div>
      <h1 class="text-4xl font-bold text-green-800 mb-4">Order Confirmed!</h1>
      <p class="text-gray-600 mt-4 mb-6">
        Thank you for your purchase! We're preparing your order of delicious nuts right away.
        You'll receive a confirmation email shortly.
      </p>
      <div class="bg-white p-5 rounded-xl inline-block shadow-sm">
        <p class="text-gray-600">Order ID: <span class="font-bold text-green-800">{{ orderId }}</span></p>
        <p class="text-sm text-gray-500 mt-2">Please save this ID for your reference</p>
      </div>
    </div>

    <!-- Actions -->
    <div class="flex flex-col sm:flex-row justify-center gap-4 mt-10">
      <router-link to="/" class="bg-green-700 px-8 py-3 text-white rounded-full hover:bg-green-600 transition shadow-md hover:shadow-lg font-medium text-center">
        Continue Shopping
      </router-link>
      <router-link :to="`/order-details/${orderId}`" class="px-8 py-3 text-green-800 border border-green-300 rounded-full hover:bg-green-100 transition font-medium text-center">
        View Order Details
      </router-link>
    </div>
  </div>
</template>