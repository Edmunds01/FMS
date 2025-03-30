<script setup lang="ts">
import FaIcon, { type IconName } from "@/components/global/fa-icon.vue";
import { icons as iconNames } from "@/components/global/fa-icon.vue";

defineProps<{
  iconName: IconName;
}>();

defineEmits<{
  selectIcon: [iconName: IconName];
}>();
</script>

<template>
  <div class="btn-group dropstart border-end rounded-0">
    <button
      type="button"
      class="btn dropdown-toggle"
      data-bs-toggle="dropdown"
      aria-expanded="false"
    >
      <div class="icon-container">
        <FaIcon :icon-name="iconName" size="lg" />
      </div>
    </button>
    <ul class="dropdown-menu">
      <div class="icon-grid">
        <li
          v-for="icon in iconNames"
          :key="icon"
          class="dropdown-item icon-container"
          @click="$emit('selectIcon', icon)"
        >
          <FaIcon :icon-name="icon" size="lg" />
        </li>
      </div>
    </ul>
  </div>
</template>

<style scoped lang="scss">
@import "../category-buttons/style.scss";
</style>

<style lang="scss">
.btn {
  border-top-right-radius: 0;
  border-bottom-right-radius: 0;
}

.icon-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 0.5rem;
  padding: 0.5rem;
}
</style>
