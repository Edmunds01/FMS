<script setup lang="ts">
import ModalWindow from "@/components/global/ModalWindow.vue";
import SelectIconDropdown from "@/components/global/SelectIconDropdown.vue";
import { ref, watch } from "vue";
import FaIcon, { type IconName } from "@/components/global/FaIcon.vue";
import { api, type Account } from "@/api/auto-generated-client";
import { useConfirm } from "@/utils/confirm";
import { useNotification } from "@kyvg/vue3-notification";

const props = defineProps<{
  id: string;
  account: Account;
}>();

const notification = useNotification();

const editAccount = ref<Account>(props.account);
const isEditMode = ref(false);
const newName = ref(editAccount.value.name);
const errorName = ref<string>();

const emit = defineEmits<{
  (e: "delete-account", accountId?: number): void;
}>();

const confirm = useConfirm();

async function deleteAccount() {
  const result = await confirm(`Vēlaties izdzēst kontu "${editAccount.value.name}"`, props.id);

  if (result) {
    emit("delete-account", props.account.accountId);
  }
}

async function iconChanged(icon: IconName) {
  editAccount.value.icon = icon;

  await api.saveAccountIcon(editAccount.value.accountId, icon);

  notification.notify({
    title: "Ikona mainīta",
    text: `Ikona saglabāta.`,
    duration: 4000,
    type: "success",
  });
}

function validateName(): boolean {
  if (!newName.value) {
    errorName.value = "Konta nosaukums ir obligāts.";
    return false;
  } else if ((newName.value?.length ?? 0) > 15) {
    errorName.value = "Konta nosaukums nedrīkst pārsniegt 15 rakstzīmes.";
    return false;
  } else {
    errorName.value = undefined;
    return true;
  }
}

async function iconNameSaved() {
  if (!validateName()) {
    return;
  }

  isEditMode.value = false;

  editAccount.value.name = newName.value;
  await api.saveAccountName(editAccount.value.accountId, newName.value);

  notification.notify({
    title: "Nosaukums mainīts",
    text: `Nosaukums saglabāts.`,
    duration: 4000,
    type: "success",
  });
}

watch(
  () => newName.value,
  () => {
    validateName();
  },
);
</script>

<template>
  <ModalWindow :id="id" title="Konta rediģēšana">
    <template #body>
      <div class="d-flex align-items-center h-100">
        <SelectIconDropdown
          :icon-name="editAccount.icon ?? ''"
          @select-icon="(icon) => iconChanged(icon)"
        />
        <div class="row flex-grow-1">
          <div v-if="isEditMode" class="col d-flex align-items-center justify-content-center">
            <div>
              <input
                v-model="newName"
                type="text"
                class="form-control form-control-sm me-2"
                :class="{ 'is-invalid': errorName }"
                style="width: 30rem"
              />
              <div v-if="errorName" class="text-danger mt-1">
                {{ errorName }}
              </div>
            </div>
            <button class="p-0" @click="iconNameSaved()">
              <FaIcon icon-name="floppy-disk" size="lg" />
            </button>
            <button
              class="p-0 ms-4"
              @click="
                isEditMode = false;
                newName = editAccount.name;
                errorName = undefined;
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
          <button v-if="editAccount.showDeleteButton" class="p-0" @click="deleteAccount">
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

<style scoped lang="scss">
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

.is-invalid {
  border-color: red;
  box-shadow: 0 0 0 0.2rem rgba(255, 0, 0, 0.25);
}
</style>
