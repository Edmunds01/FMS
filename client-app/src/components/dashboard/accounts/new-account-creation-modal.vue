<script setup lang="ts">
import ModalWindow from "@/components/global/modal-window.vue";
import { ref, watch } from "vue";
import IconDropdown from "./icon-dropdown.vue";
import { type Account } from "@/api/auto-generated-client";

const newAccount = ref<Account>({
  accountId: undefined,
  icon: "sack-dollar",
  name: "",
  balance: 0,
  showDeleteButton: undefined,
});
const error = ref<string>();

const validateForm = () => {
  error.value = !newAccount.value.name ? "Name is required" : undefined;
  return !error.value;
};

const saveAccount = () => {
  if (validateForm()) {
    emit("save-account", newAccount.value);
  }
};

watch(
  () => newAccount.value.name,
  () => {
    validateForm();
  },
);

const emit = defineEmits<{
  (e: "save-account", accountId?: Account): void;
}>();
</script>

<template>
  <ModalWindow id="accountModal" :height="10">
    <template #body>
      <div class="d-flex align-items-center h-100">
        <IconDropdown
          :icon-name="newAccount.icon"
          @select-icon="(icon) => (newAccount.icon = icon)"
        />
        <div class="flex-grow-1 pe-3 account-details">
          <div class="mb-1 row">
            <label for="staticEmail" class="col-5 col-form-label text-start align-bottom"
              >Konta nosaukums:</label
            >
            <div class="col-7">
              <div v-if="error" class="text-danger mt-1 error">
                {{ error }}
              </div>
              <input
                v-model="newAccount.name"
                type="text"
                class="form-control"
                :class="{ 'is-invalid': error }"
              />
            </div>
          </div>
          <div class="row">
            <label for="inputPassword" class="col-5 col-form-label text-start align-bottom"
              >Konta sakotnēja summa:</label
            >
            <div class="col-7">
              <input v-model="newAccount.balance" type="number" class="form-control" />
            </div>
          </div>
        </div>
        <div class="d-flex align-items-center">
          <button class="btn btn-primary px-4" @click="saveAccount">Saglabāt</button>
          <button
            type="button"
            class="btn-close ms-3 me-3"
            data-bs-dismiss="modal"
            aria-label="Close"
            @click="error = undefined"
          ></button>
        </div>
      </div>
    </template>
  </ModalWindow>
</template>

<style scoped>
.align-bottom {
  align-self: flex-end;
}

.error {
  position: absolute;
  top: 0;
  margin-left: 0.5rem;
}

.is-invalid {
  border-color: red;
  box-shadow: 0 0 0 0.2rem rgba(255, 0, 0, 0.25);
}

.btn-primary {
  font-size: 1rem;
  padding: 0.5rem 1rem;
  border-radius: 0.25rem;
  height: 2rem;
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
  border-radius: 50%;
}

.account-details {
  flex: 1;
  margin-left: 1rem;
}
</style>
