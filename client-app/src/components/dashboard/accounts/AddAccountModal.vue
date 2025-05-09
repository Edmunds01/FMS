<script setup lang="ts">
import ModalWindow from "@/components/global/ModalWindow.vue";
import { ref, watch } from "vue";
import SelectIconDropdown from "@/components/global/SelectIconDropdown.vue";
import { type NewAccount } from "@/api/auto-generated-client";
import SaveOrCloseInModal from "@/components/global/SaveOrCloseInModal.vue";

defineProps<{
  id: string;
}>();

const defaultAccount = {
  icon: "sack-dollar",
  name: "",
  balance: 0,
};

const newAccount = ref<NewAccount>({ ...defaultAccount });
const errorName = ref<string>();
const errorBalance = ref<string>();

const validateForm = () => {
  let isValid = true;

  if (!newAccount.value.name) {
    errorName.value = "Konta nosaukums ir obligāts.";
    isValid = false;
  } else if ((newAccount.value.name?.length ?? 0) > 15) {
    errorName.value = "Konta nosaukums nedrīkst pārsniegt 15 rakstzīmes.";
    isValid = false;
  } else {
    errorName.value = undefined;
  }

  if (newAccount.value.balance > 10_000_000) {
    errorBalance.value = "Sākotnējā summa nedrīkst pārsniegt 10 000 000.";
    isValid = false;
  } else if (newAccount.value.balance < 0) {
    errorBalance.value = "Sākotnējā summa nedrīkst būt negatīva.";
    isValid = false;
  } else {
    errorBalance.value = undefined;
  }

  return isValid;
};

const addNewAccount = () => {
  if (validateForm()) {
    emit("save-account", newAccount.value);
    clearData();
  }
};

watch(
  () => [newAccount.value.name, newAccount.value.balance],
  () => {
    validateForm();
  },
);

function clearData() {
  newAccount.value = { ...defaultAccount };
  setTimeout(() => {
    errorName.value = undefined;
    errorBalance.value = undefined;
  }, 0);
}

const emit = defineEmits<{
  (e: "save-account", accountId?: NewAccount): void;
}>();
</script>

<template>
  <ModalWindow :id="id" :height="10" title="Jauna konta pievienošana">
    <template #body>
      <div class="d-flex align-items-center h-100">
        <SelectIconDropdown
          :icon-name="newAccount.icon"
          @select-icon="(icon) => (newAccount.icon = icon)"
        />
        <div class="flex-grow-1 pe-3 account-details">
          <div class="mb-1 row">
            <label class="col-5 col-form-label text-start align-bottom"> Konta nosaukums: </label>
            <div class="col-7">
              <div v-if="errorName" class="text-danger mt-1 error">
                {{ errorName }}
              </div>
              <input
                v-model="newAccount.name"
                type="text"
                class="form-control"
                :class="{ 'is-invalid': errorName }"
              />
            </div>
          </div>
          <div class="row">
            <label class="col-5 col-form-label text-start align-bottom">
              Konta sakotnēja summa:
            </label>
            <div class="col-7">
              <div v-if="errorBalance && !errorName" class="text-danger mt-1 error">
                {{ errorBalance }}
              </div>
              <input
                v-model="newAccount.balance"
                type="number"
                class="form-control"
                :class="{ 'is-invalid': errorBalance }"
              />
            </div>
          </div>
        </div>
        <SaveOrCloseInModal @save="addNewAccount" />
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
