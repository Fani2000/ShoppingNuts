<script setup lang="ts">
import { defineProps } from 'vue';

defineProps<{
  title?: string;
}>();
</script>

<template>
  <div class="min-header flex items-center justify-between p-4 bg-green-50 border-b border-green-200 mb-6">
    <router-link 
      to="/" 
      class="flex items-center text-green-700 hover:text-green-500 transition-colors"
    >
      <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 mr-1" viewBox="0 0 20 20" fill="currentColor">
        <path fill-rule="evenodd" d="M9.707 16.707a1 1 0 01-1.414 0l-6-6a1 1 0 010-1.414l6-6a1 1 0 011.414 1.414L5.414 9H17a1 1 0 110 2H5.414l4.293 4.293a1 1 0 010 1.414z" clip-rule="evenodd" />
      </svg>
      <span class="font-medium">Home</span>
    </router-link>
    
    <h2 v-if="title" class="text-lg font-medium text-green-800">{{ title }}</h2>
    
    <div class="w-24"><!-- Spacer to balance the layout --></div>
  </div>
</template>