<script setup lang="ts">
import type { CategoryType } from "@/api/categories";
import FaIcon from "@/components/global/fa-icon.vue";

defineProps<{
  type: CategoryType;
}>();
</script>

<template>
  <div class="category-button text-center">
    <div class="icon-container">
      <FaIcon :icon-name="'plus'" size="xl" />
    </div>
    <div class="category-name">Pievienot</div>
  </div>
</template>

<style scoped lang="scss">
@import "./style.scss";

.category-button {
  margin-top: 0.75rem;
  opacity: 0.5;
}
</style>
