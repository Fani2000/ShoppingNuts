<script lang="ts" setup>
const handleSubmit = () => {}
</script>

<template>
  <div class="container mx-auto py-12 px-4">
    <div class="max-w-2xl mx-auto bg-white rounded-2xl shadow-lg overflow-hidden border border-green-100">
      <div class="md:flex">
        <!-- Image/Color Section -->
        <div class="md:w-2/5 bg-gradient-to-br from-green-700 to-green-800 p-8 text-white flex flex-col justify-center">
          <h2 class="text-2xl font-bold mb-6">Get in Touch</h2>
          <p class="mb-8 opacity-90">We'd love to hear from you. Send us a message and we'll respond as soon as possible.</p>
          
          <div class="space-y-4">
            <div class="flex items-center">
              <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 mr-3 text-green-300" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 8l7.89 5.26a2 2 0 002.22 0L21 8M5 19h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z" />
              </svg>
              <span>contact@nutshop.com</span>
            </div>
            <div class="flex items-center">
              <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 mr-3 text-green-300" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 5a2 2 0 012-2h3.28a1 1 0 01.948.684l1.498 4.493a1 1 0 01-.502 1.21l-2.257 1.13a11.042 11.042 0 005.516 5.516l1.13-2.257a1 1 0 011.21-.502l4.493 1.498a1 1 0 01.684.949V19a2 2 0 01-2 2h-1C9.716 21 3 14.284 3 6V5z" />
              </svg>
              <span>(123) 456-7890</span>
            </div>
          </div>
        </div>
        
        <!-- Form Section -->
        <div class="md:w-3/5 p-8">
          <h1 class="text-2xl font-bold text-green-800 mb-6">Contact Us</h1>
          <form @submit.prevent="handleSubmit" class="space-y-6">
            <div>
              <label for="name" class="block text-gray-700 text-sm font-medium mb-2">Name</label>
              <input
                  id="name"
                  type="text"
                  class="w-full p-3 border border-green-200 rounded-lg focus:ring focus:ring-green-200 focus:border-green-400 transition"
                  placeholder="Your Name"
              />
            </div>
            <div>
              <label for="email" class="block text-gray-700 text-sm font-medium mb-2">Email</label>
              <input
                  id="email"
                  type="email"
                  class="w-full p-3 border border-green-200 rounded-lg focus:ring focus:ring-green-200 focus:border-green-400 transition"
                  placeholder="Your Email"
              />
            </div>
            <div>
              <label for="message" class="block text-gray-700 text-sm font-medium mb-2">Message</label>
              <textarea
                  id="message"
                  rows="4"
                  class="w-full p-3 border border-green-200 rounded-lg focus:ring focus:ring-green-200 focus:border-green-400 transition"
                  placeholder="Your Message"
              ></textarea>
            </div>
            <button
                type="submit"
                class="bg-green-700 text-white px-6 py-3 rounded-full hover:bg-green-600 transition shadow-md hover:shadow-lg font-medium transform hover:-translate-y-1"
            >
              Send Message
            </button>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>