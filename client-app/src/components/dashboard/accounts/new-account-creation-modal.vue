<script setup lang="ts">
import { type Account } from "@/api/accounts";
import ModalWindow from "@/components/global/modal-window.vue";
import { ref } from "vue";
import IconDropdown from "./icon-dropdown.vue";

const newAccount = ref<Account>({
  icon: "sack-dollar",
  name: "",
  balance: 0,
});

function saveAccount() {
  console.log("Account saved:", newAccount.value);
}
</script>

<template>
  <ModalWindow id="accountModal">
    <template #body>
      <div class="d-flex align-items-center">
        <IconDropdown
          :icon-name="newAccount.icon"
          @select-icon="(icon) => (newAccount.icon = icon)"
        />
        <div class="flex-grow-1 pe-3 account-details">
          <div class="row mt-4 mb-2">
            <label
              for="colFormLabelSm"
              class="col-4 col-form-label col-form-label-sm align-items-start"
              >Konta nosaukums:</label
            >
            <div class="col">
              <input
                id="colFormLabelSm"
                type="text"
                class="form-control form-control-sm"
                placeholder="Konta nosaukums"
              />
            </div>
          </div>
          <div class="row pb-4">
            <label for="colFormLabelSm" class="col-4 col-form-label col-form-label-sm"
              >Konta sakotnēja summa:
            </label>
            <div class="col">
              <input
                id="colFormLabelSm"
                type="text"
                class="form-control form-control-sm"
                :placeholder="(0).toEurFormat()"
              />
            </div>
          </div>
        </div>
        <div class="d-flex justify-content-between align-items-center mt-3">
          <button class="btn btn-primary px-4" @click="saveAccount">Saglabāt</button>
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

.account-details {
  margin-top: -2rem;
  margin-bottom: -2rem;
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
