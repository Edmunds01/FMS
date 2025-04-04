<script lang="ts">
export const icons = [
  "calendar",
  "xmark",
  "wand-magic",
  "envelope",
  "tree",
  "gear",
  "paperclip",
  "pen",
  "cart-shopping",
  "plus",
  "credit-card",
  "money-bill",
  "sack-dollar",
  "money-bills",
  "floppy-disk",
  "trash",
  "money-bill-1",
] as const;
</script>

<script setup lang="ts">
export type IconName = typeof icons[number];
</script>

<template>
  <div>icon</div>
</template>

<style scoped></style>
