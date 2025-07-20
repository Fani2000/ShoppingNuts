<script setup lang="ts">

</script>

<template>
  <div class="px-6 h-[330px] mb-6 header-container bg-no-repeat bg-cover">
    <div class="relative top-10 flex flex-col gap-20 text-white bg-gradient-to-b from-transparent">
      <div class="flex justify-between">
        <div>
          <img src="../assets/Images/logo.svg" />
        </div>
        <div class="flex gap-6">
          <div>
            <img src="../assets/Images/search.svg" />
          </div>
          <div>
            <img src="../assets/Images/cart.svg" />
          </div>
        </div>
      </div>
      <div class="">
        <h1 class="text-4xl font-bold">Discover Nature's Bounty</h1>
        <p class="text-xl mt-2">
          Discover our premium selection of fresh and healthy nuts just for you!
        </p>
        <p class="mt-2">
          Fuel your body and mind with nature's most delicious energy source.
          Explore our wide variety of nuts and discover the perfect fit for your
          healthy lifestyle.
        </p>
      </div>
    </div>
  </div>
</template>

<style scoped>
.header-container {
  background-image: url("../assets/Images/background.svg");
}
</style>