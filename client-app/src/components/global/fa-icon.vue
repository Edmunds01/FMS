<script setup lang="ts">
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";

// eslint-disable-next-line @typescript-eslint/no-unused-vars
const icons = [
  "calendar",
  "xmark",
  "wand-magic",
  "envelope",
  "tree",
  "gear",
  "paperclip",
  "pen",
  "cart-shopping",
] as const;

export type IconName = (typeof icons)[number];

const props = defineProps<{
  iconName: IconName;
}>();
</script>

<template>
  <font-awesome-icon :icon="'fa-solid fa-' + props.iconName" />
</template>

<style scoped></style>
