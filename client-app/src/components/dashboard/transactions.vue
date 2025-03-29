<script setup lang="ts">
import TransactionCategoryButton from "./category-buttons/transaction-category-button.vue";
import AddTransactionCategoryButton from "./category-buttons/add-transaction-category-button.vue";
import { MapCategoryType, type Category, type CategoryType } from "@/api/categories";

defineProps<{
  transactionType: CategoryType;
  transactionSum: number;
  categories: Category[];
}>();
</script>

<template>
  <div class="container-fluid transaction-container">
    <div class="row sticky-header second-row-height border-bottom border-end text-center">
      <div class="col text-center full-center-text fs-5">
        <div>{{ MapCategoryType(transactionType) }}</div>
        <div>{{ transactionSum.toEurFormat() }}</div>
      </div>
    </div>
    <div class="row flex-grow-1 border-end">
      <div class="col">
        <div class="transaction-list d-flex flex-wrap">
          <TransactionCategoryButton
            v-for="category in categories"
            :key="category.id"
            :category="category"
            class="category-width user-select-none"
          />
          <AddTransactionCategoryButton
            :type="transactionType"
            class="category-width user-select-none"
          />
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped lang="scss">
.transaction-container {
  display: flex;
  flex-direction: column;
  height: 100vh;
  overflow: hidden;
}

.sticky-header {
  position: sticky;
  top: 0;
  z-index: 10;
  background-color: white;
  flex-shrink: 0;
}

.row.flex-grow-1 {
  flex-grow: 1;
  overflow-y: auto;
}

.transaction-list {
  display: flex;
  flex-wrap: wrap;
  gap: 10px;
}

.category-width {
  margin-bottom: 10px;
  flex: 0 0 calc(100% - 10px);

  @media (min-width: 1200px) {
    flex: 0 0 calc(100% / 2 - 10px);
  }

  @media (min-width: 1500px) {
    flex: 0 0 calc(100% / 3 - 10px);
  }

  @media (min-width: 1920px) {
    flex: 0 0 calc(100% / 4 - 10px);
  }

  @media (min-width: 2560px) {
    flex: 0 0 calc(100% / 5 - 10px);
  }

  @media (min-width: 3840px) {
    flex: 0 0 calc(100% / 6 - 10px);
  }
}
</style>
