<script setup lang="ts">
import ModalWindow from "@/components/global/modal-window.vue";
import IconDropdown from "./icon-dropdown.vue";
import { ref } from "vue";
import FaIcon, { type IconName } from "@/components/global/fa-icon.vue";
import { api, type Account } from "@/api/auto-generated-client";

const props = defineProps<{
  account: Account;
}>();

const editAccount = ref<Account>(props.account);
const isEditMode = ref(false);
const newName = ref(editAccount.value.name);

defineEmits<{
  (e: "delete-account", accountId?: number): void;
}>();

async function iconChanged(icon: IconName) {
  editAccount.value.icon = icon;

  await api.saveAccountIcon(editAccount.value.accountId, icon);
}

async function iconNameSaved() {
  isEditMode.value = false;

  editAccount.value.name = newName.value;
  await api.saveAccountName(editAccount.value.accountId, newName.value);
}
</script>

<template>
  <ModalWindow id="accountEditModal">
    <template #body>
      <div class="d-flex align-items-center">
        <IconDropdown
          :icon-name="editAccount.icon ?? ''"
          @select-icon="(icon) => iconChanged(icon)"
        />
        <div class="row flex-grow-1">
          <div
            v-if="isEditMode"
            class="col d-flex align-items-center justify-content-center"
          >
            <input
              v-model="newName"
              type="text"
              class="form-control form-control-sm me-4"
              style="width: 30rem"
            />
            <button class="p-0" @click="iconNameSaved()">
              <FaIcon icon-name="floppy-disk" size="lg" />
            </button>
            <button
              class="p-0 ms-4"
              @click="
                isEditMode = false;
                newName = editAccount.name;
              "
            >
              <FaIcon icon-name="xmark" size="lg" />
            </button>
          </div>
          <div v-else class="col d-flex align-items-center justify-content-center">
            <div class="p-0 me-5">{{ editAccount.name }}</div>
            <button class="p-0" @click="isEditMode = true">
              <FaIcon icon-name="pen" size="lg" />
            </button>
          </div>
        </div>

        <div class="d-flex justify-content-between align-items-center">
          <button
            v-if="editAccount.showDeleteButton"
            class="p-0"
            @click="$emit('delete-account', editAccount.accountId)"
          >
            <FaIcon icon-name="trash" size="lg" />
          </button>
          <button
            type="button"
            class="btn-close ms-3 me-3"
            data-bs-dismiss="modal"
            aria-label="Close"
          ></button>
        </div>
      </div>
    </template>
  </ModalWindow>
</template>

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
</style>
