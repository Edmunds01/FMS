<script setup lang="ts">
import FaIcon from "@/components/global/fa-icon.vue";
</script>

<template>
  <div class="category-button text-center">
    <div class="amount">&nbsp;</div>
    <div class="icon-container">
      <FaIcon :icon-name="'plus'" size="xl" />
    </div>
    <div class="category-name">Pievienot</div>
  </div>
</template>

<style scoped lang="scss">
@import "./style.scss";

.category-button {
  opacity: 0.5;
}
</style>
