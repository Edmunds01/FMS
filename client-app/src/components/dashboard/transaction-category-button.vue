<script setup lang="ts">
import FaIcon from "../global/fa-icon.vue";

defineProps<{
  amount: number;
  category: string;
  icon: string;
}>();
</script>

<template>
  <div class="category-button text-center">
    <div class="amount">{{ amount }}€</div>
    <div class="icon-container">
      <FaIcon icon-name="calendar" />
    </div>
    <div class="category-name">{{ category }}</div>
  </div>
</template>

<style scoped>
.category-button {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  width: 3rem;
  height: 6.7rem;
  padding: 0.2rem;
  background-color: #fff;
}

.amount {
  font-size: 1rem;
  font-weight: bold;
  margin-bottom: 5px;
}

.icon-container {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 50px;
  height: 50px;
  border: 2px solid black;
  border-radius: 50%;
  margin-bottom: 5px;
}

.icon {
  font-size: 1.5rem;
}

.category-name {
  font-size: 0.9rem;
  color: #555;
}
</style>
