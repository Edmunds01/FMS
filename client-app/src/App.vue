<script setup lang="ts">
import { RouterView } from "vue-router";
</script>

<template>
  <RouterView class="app-container" />
</template>

<style lang="scss" scoped>
:root {
  /* bootstrap */
  --bs-body-font-size: 1rem;
}

body {
  margin: 0;
  margin-block-end: 0;
  margin-block-start: 0;
}

h1 {
  margin: 0;
  margin-block-end: 0;
  margin-block-start: 0;
}

.app-container {
  @media (max-width: 1920px) {
    width: 1920px;
    overflow: auto;
  }
}
</style>

<style scoped></style>
