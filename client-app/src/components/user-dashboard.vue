<script setup lang="ts">
import { categories } from "@/api/categories";
import DateSelect from "../components/date-select.vue";
import Accounts from "./dashboard/accounts.vue";
import Transactions from "./dashboard/transactions.vue";

const startDate = new Date();
const endDate = new Date(2026, 9, 18);

const expense = categories.filter((category) => category.type === "expense");
const income = categories.filter((category) => category.type === "income");
</script>

<template>
  <div class="container-fluid vh-100 text-center">
    <div class="row vh-100">
      <div class="col-2 p-0 border-end">
        <Accounts />
      </div>
      <div class="col p-0">
        <div class="container-fluid text-center">
          <div class="row first-row-height">
            <div class="col p-0 border-bottom">
              <DateSelect :start-date="startDate" :end-date="endDate" />
            </div>
          </div>
          <div class="row vh-100">
            <div class="col-3 p-0">
              <Transactions
                transaction-type="Izdevumi"
                :transaction-sum="200"
                :categories="expense"
              />
            </div>
            <div class="col-3 p-0">
              <Transactions
                transaction-type="Ienākumi"
                :transaction-sum="200"
                :categories="income"
              />
            </div>
            <div class="col p-0">Stats</div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.bg-primary {
  background-color: #b9d2fa !important;
}

.bg-success {
  background-color: #cfe9c7 !important;
}

.bg-info {
  background-color: #e0f7fa !important;
}

.bg-warning {
  background-color: #ffecb3 !important;
}

.bg-success {
  background-color: #c8e6c9 !important;
}
</style>
