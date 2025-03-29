<script setup lang="ts">
import { accounts } from "@/api/accounts";
import FaIcon from "../global/fa-icon.vue";

function handleClick(account: { name: string; balance: number }) {
  alert(account.name);
}

function createNewAccount() {
  alert("Create new account");
}

const total = accounts.reduce((acc, account) => acc + account.balance, 0);
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
      <button class="col d-flex" @click="handleClick(account)">
        <div class="account-details">
          <div class="full-center-text text-ellipsis" :title="account.name">
            {{ account.name }}
          </div>
          <div class="full-center-text">{{ account.balance.toEurFormat() }}</div>
        </div>
        <div class="icon full-center-text">
          <FaIcon :icon-name="account.icon" size="lg" />
        </div>
      </button>
    </div>
    <div class="row no-gutters border border-end-0 border-top-0">
      <button class="col text-center" @click="createNewAccount()">
        <div>+ Pievienot</div>
      </button>
    </div>
  </div>
</template>

<style scoped>
.account-div {
  max-height: 100vh;
  overflow-y: auto;
}

.account-details {
  flex: 1;
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
