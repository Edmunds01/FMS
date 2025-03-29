<script setup lang="ts">
import { accounts } from "@/api/accounts";
import FaIcon from "@/components/global/fa-icon.vue";
import Modal from "bootstrap/js/dist/modal";
import { onMounted, onUnmounted, ref } from "vue";
import NewAccountCreationModal from "./new-account-creation-modal.vue";
import AccountEditModal from "./Account-edit-modal.vue";

function addAccount(modelId: "accountModal" | "accountEditModal") {
  const modal = document.getElementById(modelId);
  if (modal) {
    const modalInstance = Modal.getInstance(modal) ?? new Modal(modal);
    if (modalInstance) {
      modalInstance.show();
    }
  }
}

const screenWidth = ref(window.innerWidth);

function trimName(name: string) {
  const maxLength = screenWidth.value > 1200 ? 17 : screenWidth.value > 768 ? 12 : 5;

  return name.length > maxLength ? name.slice(0, maxLength) + "..." : name;
}

const total = accounts.reduce((acc, account) => acc + account.balance, 0);

function updateScreenWidth() {
  screenWidth.value = window.innerWidth;
}

onMounted(() => {
  window.addEventListener("resize", updateScreenWidth);
});

onUnmounted(() => {
  window.removeEventListener("resize", updateScreenWidth);
});
</script>

<template>
  <div class="account-div p-0">
    <div class="row no-gutters border border-end-0 first-row-height">
      <div class="col text-center full-center-text fs-1">Konti</div>
    </div>
    <div class="row no-gutters border-start border-bottom second-row-height">
      <div class="col text-center fs-5">
        <div>Kopā</div>
        <div>{{ total.toEurFormat() }}</div>
      </div>
    </div>
    <div
      v-for="account in accounts"
      :key="account.name"
      class="row no-gutters border border-end-0 border-top-0 dashed-bottom-border"
    >
      <button class="col d-flex" @click="addAccount('accountEditModal')">
        <div class="account-details">
          <div class="full-center-text text-ellipsis fs-5" :title="account.name">
            {{ trimName(account.name) }}
          </div>
          <div class="full-center-text">{{ account.balance.toEurFormat() }}</div>
        </div>
        <div class="icon full-center-text">
          <FaIcon :icon-name="account.icon" size="lg" />
        </div>
      </button>
    </div>
    <div class="row no-gutters border border-end-0 border-top-0">
      <button class="col text-center add-account" @click="addAccount('accountModal')">
        <div>+ Pievienot</div>
      </button>
    </div>
    <NewAccountCreationModal />
    <AccountEditModal />
  </div>
</template>

<style scoped>
.btn-primary {
  font-size: 1rem;
  padding: 0.5rem 1rem;
  border-radius: 0.25rem;
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
