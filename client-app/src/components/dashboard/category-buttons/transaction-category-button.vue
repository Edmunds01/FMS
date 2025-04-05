<script setup lang="ts">
import type { Category } from "@/api/auto-generated-client";
import FaIcon from "@/components/global/fa-icon.vue";

defineProps<{
  category: Category;
}>();
</script>

<template>
  <div class="category-button text-center">
    <div class="amount">{{ category.sumOfTransactions.toEurFormat() }}</div>
    <div class="icon-container">
      <FaIcon :icon-name="category.icon" size="xl" />
    </div>
    <div class="category-name">{{ category.name }}</div>
  </div>
</template>

<style scoped lang="scss">
@use "./style.scss";
</style>
