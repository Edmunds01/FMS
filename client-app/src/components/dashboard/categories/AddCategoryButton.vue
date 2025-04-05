<script setup lang="ts">
import FaIcon from "@/components/global/FaIcon.vue";

defineEmits<{
  (e: "left-click"): void;
}>();
</script>

<template>
  <button class="category-button text-center" @click="$emit('left-click')">
    <div class="amount">&nbsp;</div>

    <div class="icon-container">
      <FaIcon :icon-name="'plus'" size="xl" />
    </div>

    <div class="category-name">Pievienot</div>
  </button>
</template>

<style scoped lang="scss">
@use "./style";

.category-button {
  opacity: 0.5;
}
</style>
