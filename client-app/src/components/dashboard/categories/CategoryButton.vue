<script setup lang="ts">
import type { Category } from "@/api/auto-generated-client";
import FaIcon from "@/components/global/FaIcon.vue";

defineProps<{
  category: Category;
}>();

const emit = defineEmits<{
  (e: "left-click"): void;
  (e: "right-click"): void;
  (e: "double-click"): void;
}>();

let clicks = 0;
let timer = 0;

// Prevents double-clicking from triggering the left-click event
function onClick() {
  clicks++;
  if (clicks === 1) {
    timer = setTimeout(() => {
      clicks = 0;
      emit("left-click");
    }, 200);
  } else {
    clearTimeout(timer);
    clicks = 0;
  }
}
</script>

<template>
  <button
    class="category-button text-center"
    @click="onClick"
    @contextmenu.prevent="$emit('right-click')"
    @dblclick.prevent="$emit('double-click')"
  >
    <div class="amount">{{ category.sumOfTransactions.toEurFormat() }}</div>
    <div class="icon-container">
      <FaIcon :icon-name="category.icon" size="xl" />
    </div>
    <div class="category-name text-nowrap">
      {{ !!category.name ? category.name : "&nbsp;" }}
    </div>
  </button>
</template>

<style scoped lang="scss">
@use "./style.scss";
</style>
