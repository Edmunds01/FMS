<script setup lang="ts">
import FaIcon from "@/components/global/fa-icon.vue";
import { computed, onMounted, onUnmounted, ref } from "vue";
import NewAccountCreationModal from "./new-account-creation-modal.vue";
import AccountEditModal from "./account-edit-modal.vue";
import { api, type Account, type NewAccount } from "@/api/auto-generated-client";
import { closeModal, openModal } from "@/components/global/modal-window.vue";

const selectedAccount = ref<Account | null>(null);
const accounts = ref<Account[]>([]);

onMounted(async () => {
  await fetchAccounts();
});

const newAccountModalId = "accountModal";
const accountEditModalId = "accountEditModal";

async function fetchAccounts() {
  accounts.value = await api.accounts();
}

async function saveAccount(account?: NewAccount) {
  closeModal(newAccountModalId);
  selectedAccount.value = null;

  if (account) {
    await api.createNewAccount(account);
    await fetchAccounts();
  }
}

async function deleteAccount(accountId?: number) {
  closeModal(accountEditModalId);
  selectedAccount.value = null;
  await api.deleteAccount(accountId);

  accounts.value = accounts.value.filter((account) => account.accountId !== accountId);
}

function openAccountModal(modelId: string, account?: Account) {
  if (account) {
    selectedAccount.value = account;
  }

  const onModalHidden = () => {
    selectedAccount.value = null;
  };

  openModal(modelId, onModalHidden);
}

// #region AccountNames
const screenWidth = ref(window.innerWidth);

function trimName(name: string) {
  const maxLength = screenWidth.value > 1200 ? 17 : screenWidth.value > 768 ? 12 : 5;

  return name.length > maxLength ? name.slice(0, maxLength) + "..." : name;
}

const total = computed(() => {
  return accounts.value.reduce((acc, account) => acc + account.balance, 0);
});

function updateScreenWidth() {
  screenWidth.value = window.innerWidth;
}

onMounted(() => {
  window.addEventListener("resize", updateScreenWidth);
});

onUnmounted(() => {
  window.removeEventListener("resize", updateScreenWidth);
});
// #endregion
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
      :key="account.accountId"
      class="row no-gutters border border-end-0 border-top-0 dashed-bottom-border"
    >
      <button class="col d-flex" @click="openAccountModal(accountEditModalId, account)">
        <div class="account-details">
          <div class="full-center-text text-ellipsis fs-5" :title="account.name">
            {{ trimName(account.name ?? "") }}
          </div>
          <div class="full-center-text">{{ account.balance.toEurFormat() }}</div>
        </div>
        <div class="icon full-center-text">
          <FaIcon :icon-name="account.icon" size="lg" />
        </div>
      </button>
    </div>
    <div class="row no-gutters border border-end-0 border-top-0">
      <button class="col text-center add-account" @click="openAccountModal(newAccountModalId)">
        <div>+ Pievienot</div>
      </button>
    </div>
    <NewAccountCreationModal :id="newAccountModalId" @save-account="saveAccount" />
    <AccountEditModal
      v-if="selectedAccount"
      :id="accountEditModalId"
      :account="selectedAccount"
      @delete-account="deleteAccount"
    />
  </div>
</template>

<style scoped>
.account-div {
  max-height: 100vh;
  overflow-y: auto;
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
