<script setup lang="ts">
import TheDateSelect from "./TheDateSelect.vue";
import TheAccounts from "./accounts/TheAccounts.vue";
import UserCategories from "./categories/UserCategories.vue";
import { api, CategoryType, type Account, type Category } from "@/api/auto-generated-client";
import { computed, onMounted, provide, ref, watch } from "vue";
import FaIcon from "@/components/global/FaIcon.vue";
import { accountsKey, categoriesKey, selectedDatesKey } from "@/utils/keys";
import TheDashboardModals from "./TheDashboardModals.vue";
import {
  useAddCategoryModal,
  useAddEditTransactionListModal,
  useEditCategoryModal,
  useTransactionListModal,
} from "./modals";
import { useNotification } from "@kyvg/vue3-notification";
import router from "@/router";
import ApexCharts from "vue3-apexcharts";
import type { ApexOptions } from "apexcharts";

const notification = useNotification();

const now = new Date();
const startDate = ref(new Date(Date.UTC(now.getUTCFullYear(), now.getUTCMonth(), 1)));
const endDate = ref(new Date(Date.UTC(now.getUTCFullYear(), now.getUTCMonth() + 1, 0)));

const categories = ref<Category[]>([]);
const accounts = ref<Account[]>([]);

async function fetchCategories() {
  categories.value = await api.categories(startDate.value, endDate.value);
}

async function fetchAccounts() {
  accounts.value = await api.accounts();
}

const expense = computed(() =>
  categories.value.filter((category) => category.type === CategoryType.Expense),
);
const income = computed(() =>
  categories.value.filter((category) => category.type === CategoryType.Income),
);

function logout() {
  notification.notify({
    title: "Iziet",
    text: "Jūs esat izrakstījies.",
    duration: 4000,
    type: "success",
  });

  router.push("/logout");
}

provide(selectedDatesKey, { startDate, endDate });
provide(accountsKey, { accounts, fetchAccounts });
provide(categoriesKey, { categories, fetchCategories });

useAddCategoryModal();
useEditCategoryModal();
useTransactionListModal();
useAddEditTransactionListModal();

onMounted(() => {
  fetchCategories();
  fetchAccounts();
});

watch([startDate, endDate], () => {
  fetchCategories();
});

interface Opts {
  seriesIndex: number;
}

const options = {
  chart: {
    type: "pie",
  },
  labels: ["Izdevumi", "Ienakumi", "Starpība"],
  colors: ["#FF4560", "#00E396", "#008FFB"],
  legend: {
    position: "bottom",
  },
  dataLabels: {
    enabled: true,
    formatter: (val: number, opts?: Opts) => {
      switch (opts?.seriesIndex) {
        case 0:
          return ((expenseSum.value / totalSum.value) * 100).toFixed(0) + " %";
        case 1:
          return ((incomeSum.value / totalSum.value) * 100).toFixed(0) + " %";
        case 2:
          return `Starpība: ${Number((incomeSum.value - expenseSum.value).toFixed(2)).toEurFormat()}`;
      }

      return val.toEurFormat();
    },
  },
} as ApexOptions;

const totalSum = computed(() => expenseSum.value + incomeSum.value);

const expenseSum = computed(() =>
  categories.value
    .filter((category) => category.type === CategoryType.Expense)
    .reduce((sum, category) => sum + category.sumOfTransactions, 0),
);
const incomeSum = computed(() =>
  categories.value
    .filter((category) => category.type === CategoryType.Income)
    .reduce((sum, category) => sum + category.sumOfTransactions, 0),
);
const difference = computed(() => Number(Math.abs(incomeSum.value - expenseSum.value).toFixed(2)));

const series = computed(() => [expenseSum.value, incomeSum.value, difference.value]);
</script>

<template>
  <div class="container-fluid vh-100 text-center">
    <TheDashboardModals />
    <title>Vadības panelis</title>
    <div class="row vh-100">
      <div class="col-2 p-0 border-end bf-neutral">
        <TheAccounts />
      </div>
      <div class="col p-0">
        <div class="container-fluid text-center">
          <div class="row first-row-height">
            <div class="col p-0 border-top border-bottom">
              <TheDateSelect :start-date="startDate" :end-date="endDate" />
            </div>
            <div
              class="col-1 p-0 border-bottom border-top d-flex align-items-center justify-content-end"
            >
              <button title="Iziet" class="btn" @click="logout">
                <FaIcon icon-name="right-from-bracket" size="2x" class="me-3" />
              </button>
            </div>
          </div>
          <div class="row categories-columns">
            <div class="col-3 p-0 bg-expense categories-columns">
              <UserCategories :category-type="CategoryType.Expense" :categories="expense" />
            </div>
            <div class="col-3 p-0 bg-income categories-columns">
              <UserCategories :category-type="CategoryType.Income" :categories="income" />
            </div>
            <div class="col p-0 bf-neutral categories-columns">
              <div class="row sticky-header second-row-height border-bottom border-end text-center">
                <div class="col text-center full-center-text fs-3">
                  <div>Statistika</div>
                </div>
              </div>
              <ApexCharts width="99%" :series :options="options"></ApexCharts>
            </div>
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

.categories-columns {
  height: calc(100vh - 5rem);
}
</style>
