<script setup lang="ts">
import DateSelect from '../components/date-select.vue';

const startDate = new Date();
const endDate = new Date(2026, 9, 18);
</script>

<template>
  <div class="dashboard-container">
    <title>Dashboard</title>

    <div class="accounts">
      <h2>Accounts</h2>
    </div>
    <div class="main-content">
      <div class="date-select">
        <DateSelect :start-date="startDate" :end-date="endDate" />
      </div>
      <div class="transactions">
        <div class="expense">
          <div>Expense</div>
        </div>
        <div class="income">Income</div>
        <div class="stats">Stats</div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.dashboard-container {
  display: flex;
  height: 100vh;
}

.accounts {
  width: 15%;
  height: 100%;
  background-color: #b9d2fa;
}

.main-content {
  width: 85%;
  display: flex;
  flex-direction: column;
}

.date-select {
  height: 7%;
  background-color: #cfe9c7;
}

.transactions {
  display: flex;
  flex-grow: 1;
  background-color: #f0f0f0;
}

.income {
  width: 30%;
  background-color: #e0f7fa;
}

.expense {
  width: 30%;
  background-color: #ffecb3;
}

.stats {
  width: 40%;
  background-color: #c8e6c9;
}
</style>
