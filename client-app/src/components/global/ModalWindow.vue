<script lang="ts">
export function openModal(id: string, onModalHidden: (() => void) | undefined = undefined) {
  setTimeout(() => {
    const modal = document.getElementById(id);
    if (modal) {
      const modalInstance = Modal.getInstance(modal) ?? new Modal(modal);
      if (modalInstance) {
        modalInstance.show();

        if (onModalHidden) {
          modal.addEventListener("hidden.bs.modal", onModalHidden);
        }
      }
    }
  }, 0);
}

export function closeModal(id: string, now: boolean = false) {
  if (now) {
    const modal = document.getElementById(id);
    if (modal) {
      const modalInstance = Modal.getInstance(modal);
      if (modalInstance) {
        modalInstance.hide();
      }
    }
    return;
  }
  setTimeout(() => {
    const modal = document.getElementById(id);
    if (modal) {
      const modalInstance = Modal.getInstance(modal);
      if (modalInstance) {
        modalInstance.hide();
      }
    }
  }, 0);
}
</script>

<script setup lang="ts">
import { Modal } from "bootstrap";
import { computed } from "vue";

const props = defineProps<{
  id: string;
  width?: number;
  height?: number;
  removeBottomBorderRadius?: boolean;
}>();

const bottomBorderRadius = computed(() => {
  return props.removeBottomBorderRadius
    ? "    border-bottom-left-radius: 0;border-bottom-right-radius: 0;"
    : "";
});
</script>

<template>
  <div :id="id" class="modal" tabindex="-1">
    <div class="modal-dialog modal-lg modal-dialog-centered" :style="`width: ${width}rem`">
      <div class="modal-content" :style="bottomBorderRadius">
        <div v-if="$slots.header" class="modal-header">
          <slot name="header" />
        </div>
        <div v-if="$slots.body" class="modal-body p-0" :style="`height: ${height}rem`">
          <slot name="body" />
        </div>
        <div v-if="$slots.footer" class="modal-footer">
          <slot name="footer" />
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped lang="scss"></style>
