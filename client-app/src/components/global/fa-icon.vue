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
  "plus",
] as const;

export type IconName = (typeof icons)[number];
type IconSize =
  | "2xs"
  | "xs"
  | "sm"
  | "lg"
  | "xl"
  | "2xl"
  | "1x"
  | "2x"
  | "3x"
  | "4x"
  | "5x"
  | "6x"
  | "7x"
  | "8x"
  | "9x"
  | "10x";

const props = defineProps<{
  iconName: IconName;
  size?: IconSize;
}>();
</script>

<template>
  <font-awesome-icon :icon="'fa-solid fa-' + props.iconName" :size="size" />
</template>

<style scoped></style>
