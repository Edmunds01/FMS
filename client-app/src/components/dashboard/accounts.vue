<script setup lang="ts">
const accounts = [
  {
    name: 'KontName',
    balance: 100.1,
  },
  {
    name: 'KontName2',
    balance: 200.14,
  },
  {
    name: 'KontName3 really long name',
    balance: 300.15,
  },
  {
    name: 'KontName4',
    balance: 400.16,
  },
  {
    name: 'KontName5',
    balance: 500.17,
  },
  {
    name: 'KontName6',
    balance: 600.18,
  },
  {
    name: 'KontName7',
    balance: 700.19,
  },
  {
    name: 'KontName8',
    balance: 800.2,
  },
  {
    name: 'KontName9',
    balance: 900.21,
  },
]

function handleClick(account: { name: string; balance: number }) {
  alert(account.name)
}

function createNewAccount() {
  alert('Create new account')
}

const total = accounts.reduce((acc, account) => acc + account.balance, 0)
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
      <button @click="handleClick(account)" class="col">
        <div class="full-center-text text-ellipsis" :title="account.name">{{ account.name }}</div>
        <div class="full-center-text">{{ account.balance.toEurFormat() }}</div>
      </button>
    </div>
    <div class="row no-gutters border border-end-0 border-top-0">
      <button @click="createNewAccount()" class="col text-center">
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

.no-gutters {
  margin-right: 0;
  margin-left: 0;

  > .col,
  > [class*='col-'] {
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
</style>
