<script setup lang="ts">
import { RouterView } from "vue-router";
import { Notifications } from "@kyvg/vue3-notification";
</script>

<template>
  <Notifications position="bottom right" style="font-size: 1.2rem !important" />
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
  @media (max-width: 1200px) {
    width: 1920px;
    overflow: auto;
  }
}
</style>

<style>
.vue-notification {
  font-size: 1.5rem !important;
}
</style>
