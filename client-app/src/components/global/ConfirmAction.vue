<script lang="ts">
export let isConfirmModal = false;
</script>

<script setup lang="ts">
import { ref, getCurrentInstance } from "vue";
import ModalWindow, { closeModal, openModal } from "@/components/global/ModalWindow.vue";

const confirmModalId = "confirmModal";

const visible = ref(false);
const message = ref("");
let resolver: ((value: boolean) => void) | null = null;

const isConfirmed = ref(false);

function open(messageRaw: string, reopenModalId: string): Promise<boolean> {
  message.value = messageRaw;
  visible.value = true;
  isConfirmModal = true;

  closeModal(reopenModalId);

  openModal(confirmModalId, () => {
    visible.value = false;
    isConfirmModal = false;

    if (!isConfirmed.value) {
      openModal(reopenModalId, () => {});
    }
  });

  return new Promise((resolve) => {
    resolver = resolve;
  });
}

function confirm() {
  isConfirmed.value = true;
  closeModal(confirmModalId);

  resolver?.(true);
  resolver = null;
}

function cancel() {
  isConfirmed.value = false;
  closeModal(confirmModalId);

  resolver?.(false);
  resolver = null;
}

const internal = getCurrentInstance();
if (internal) {
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  (internal.proxy as any).open = open;
}
</script>

<template>
  <ModalWindow v-if="visible" :id="confirmModalId" :width="25">
    <template #header>
      <h2 class="confirm-title">{{ "Apsitpriniet" }}</h2>
    </template>
    <template #body>
      <div class="confirm-modal">
        <p class="confirm-message">{{ message }}</p>
        <div class="confirm-actions">
          <button class="btn btn-secondary" @click="cancel">Cancel</button>
          <button class="btn btn-danger" @click="confirm">Confirm</button>
        </div>
      </div>
    </template>
  </ModalWindow>
</template>

<style scoped lang="scss">
.confirm-modal {
  display: flex;
  flex-direction: column;
  align-items: center;
  text-align: center;
  padding: 20px;
}

.confirm-title {
  font-size: 1.2rem;
  font-weight: bold;
  margin-bottom: 10px;
}

.confirm-message {
  font-size: 1.5rem;
  font-weight: normal;
  margin-bottom: 20px;
}

.confirm-actions {
  display: flex;
  gap: 10px;
}
</style>
