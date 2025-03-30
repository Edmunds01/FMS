<script setup lang="ts">
import { categories } from "@/api/categories";
import DateSelect from "./date-select.vue";
import Accounts from "./accounts/accounts.vue";
import Transactions from "./transactions.vue";

const startDate = new Date();
const endDate = new Date(2026, 9, 18);

const expense = categories.filter((category) => category.type === "expense");
const income = categories.filter((category) => category.type === "income");
</script>

<template>
  <div class="container-fluid vh-100 text-center">
    <div class="row vh-100">
      <div class="col-2 p-0 border-end bf-neutral">
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
            <div class="col-3 p-0 bg-expense">
              <Transactions
                transaction-type="expense"
                :transaction-sum="200"
                :categories="expense"
              />
            </div>
            <div class="col-3 p-0 bg-income">
              <Transactions transaction-type="income" :transaction-sum="200" :categories="income" />
            </div>
            <div class="col p-0">Stats</div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.bg-income {
  background-color: #ecf5ea !important;
}

.bg-expense {
  background-color: #f8e9e9 !important;
}

.bf-neutral {
  background-color: #e4eef7 !important;
}
</style>
