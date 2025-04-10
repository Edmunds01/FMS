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
  "user",
  "sack-dollar",
  "money-bills",
  "floppy-disk",
  "trash",
  "money-bill-1",
  "right-from-bracket",
  "r",
  "p",
  "s",
  "wallet",
  "pizza-slice",
  "house",
  "person",
  "gift",
  "gifts",
  "percent",
  "plane",
  "plane-departure",
  "car-side",
  "cart-plus",
] as const;
</script>

<script setup lang="ts">
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
import { computed } from "vue";

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
  iconName?: IconName | null | string;
  size?: IconSize;
}>();

const defaultIcon = "money-bill-1";

const resolvedIconName = computed(() => {
  return icons.includes(props.iconName as IconName) ? props.iconName : defaultIcon;
});
</script>

<template>
  <font-awesome-icon :icon="'fa-solid fa-' + resolvedIconName" :size="size" />
</template>

<style scoped></style>
