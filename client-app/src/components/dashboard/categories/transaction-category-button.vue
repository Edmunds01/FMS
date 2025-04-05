<script setup lang="ts">
import type { Category } from "@/api/auto-generated-client";
import FaIcon from "@/components/global/fa-icon.vue";

defineProps<{
  category: Category;
}>();

defineEmits<{
  (e: "left-click", category: Category): void;
  (e: "right-click", category: Category): void;
}>();
</script>

<template>
  <button
    class="category-button text-center"
    @click="$emit('left-click', category)"
    @contextmenu.prevent="$emit('right-click', category)"
  >
    <div class="amount">{{ category.sumOfTransactions.toEurFormat() }}</div>
    <div class="icon-container">
      <FaIcon :icon-name="category.icon" size="xl" />
    </div>
    <div class="category-name">{{ !!category.name ? category.name : "&nbsp;" }}</div>
  </button>
</template>

<style scoped lang="scss">
@use "./style.scss";
</style>
