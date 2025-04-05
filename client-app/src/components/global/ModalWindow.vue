<script lang="ts">
export function openModal(id: string, onModalHidden: () => void) {
  setTimeout(() => {
    const modal = document.getElementById(id);
    if (modal) {
      const modalInstance = Modal.getInstance(modal) ?? new Modal(modal);
      if (modalInstance) {
        modalInstance.show();

        modal.addEventListener("hidden.bs.modal", onModalHidden);
      }
    }
  }, 0);
}

export function closeModal(id: string) {
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

defineProps<{
  id: string;
  width?: number;
  height?: number;
}>();
</script>

<template>
  <div :id="id" class="modal" tabindex="-1">
    <div class="modal-dialog modal-lg modal-dialog-centered" :style="`width: ${width}rem`">
      <div class="modal-content">
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

<style scoped></style>
