<script setup lang="ts">
import FaIcon, { type IconName } from "@/components/global/fa-icon.vue";
import { icons as iconNames } from "@/components/global/fa-icon.vue";

defineProps<{
  iconName: IconName;
}>();

defineEmits(["selectIcon"]);
</script>

<template>
  <div class="btn-group dropstart">
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

<style scoped>
.btn-primary {
  font-size: 1rem;
  padding: 0.5rem 1rem;
  border-radius: 0.25rem;
}

.btn-close {
  font-size: 1.25rem;
  width: 2rem;
  height: 2rem;
  display: flex;
  align-items: center;
  justify-content: center;
  background-color: transparent;
  border: none;
  cursor: pointer;
}

.btn-close:hover {
  background-color: #f0f0f0;
  border-radius: 50%;
}

.account-div {
  max-height: 100vh;
  overflow-y: auto;
}

.account-icon {
  width: 1rem;
  height: 1rem;
}

.icon-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 0.5rem;
  padding: 0.5rem;
}

.account-details {
  flex: 1;
  margin-left: 3rem;
}

.no-gutters {
  margin-right: 0;
  margin-left: 0;

  > .col,
  > [class*="col-"] {
    padding-right: 0;
    padding-left: 0;
  }
}

.full-center-text {
  display: flex;
  align-items: center;
  justify-content: center;
}

.dashed-bottom-border {
  border-bottom: var(--bs-border-width) dashed var(--bs-border-color) !important;
}

.text-ellipsis {
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  padding: 0 5px;
  text-align: left !important;
}

.icon {
  flex: 0 0 20%;
  display: flex;
  align-items: center;
  justify-content: center;
}
</style>
