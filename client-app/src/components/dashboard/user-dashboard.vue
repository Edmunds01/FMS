<script setup lang="ts">
import DateSelect from "./date-select.vue";
import Accounts from "./accounts/accounts.vue";
import Transactions from "./transactions.vue";
import { api, CategoryType, type Category, type NewCategory } from "@/api/auto-generated-client";
import { computed, onMounted, ref } from "vue";
import FaIcon from "@/components/global/fa-icon.vue";

const startDate = ref(new Date());
const endDate = ref(new Date(2025, 9, 1));

const categories = ref<Category[]>([]);

async function fetchCategories() {
  categories.value = await api.getCategories();
}

function addCategory(newCategory: NewCategory) {
  categories.value.push({
    ...newCategory,
    sumOfTransactions: 0,
  } as Category);

  setTimeout(async () => {
    await api.addCategory(newCategory);
    await fetchCategories();
  }, 0);
}

function deleteCategory(categoryId: number) {
  categories.value = categories.value.filter((category) => category.categoryId !== categoryId);
}

const expense = computed(() =>
  categories.value.filter((category) => category.type === CategoryType.Expense),
);
const income = computed(() =>
  categories.value.filter((category) => category.type === CategoryType.Income),
);

onMounted(async () => {
  await fetchCategories();
});
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
            <div class="col-1 p-0 border-bottom d-flex align-items-center justify-content-end">
              <button title="Iziet" class="btn" @click="$router.push('/logout')">
                <FaIcon icon-name="right-from-bracket" size="2x" class="me-3" />
              </button>
            </div>
          </div>
          <div class="row vh-100">
            <div class="col-3 p-0 bg-expense">
              <Transactions
                :transaction-type="CategoryType.Expense"
                :categories="expense"
                @add-category="addCategory"
                @delete-category="deleteCategory"
              />
            </div>
            <div class="col-3 p-0 bg-income">
              <Transactions
                :transaction-type="CategoryType.Income"
                :categories="income"
                @add-category="addCategory"
                @delete-category="deleteCategory"
              />
            </div>
            <div class="col p-0 bf-neutral">Stats</div>
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
