<script setup lang="ts">
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";

type IconName = "calendar" | "xmark";

const props = defineProps<{
  iconName: IconName;
}>();
</script>

<template>
  <font-awesome-icon :icon="'fa-solid fa-' + props.iconName" />
</template>

<style scoped></style>
