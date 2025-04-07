<script setup lang="ts">
import CategoryButton from "@/components/dashboard/categories/CategoryButton.vue";
import AddCategoryButton from "@/components/dashboard/categories/AddCategoryButton.vue";
import { computed, inject } from "vue";
import { CategoryType, type Category } from "@/api/auto-generated-client";
import {
  addCategoryKey,
  addEditTransactionKey,
  editCategoryKey,
  transactionListKey,
} from "@/utils/keys";

const props = defineProps<{
  categoryType: CategoryType;
  categories: Category[];
}>();

const transactionSum = computed(() => {
  return (
    props.categories.reduce((sum, category) => {
      return sum + category.sumOfTransactions;
    }, 0) ?? 0
  );
});

function mapCategoryTypeName(category: CategoryType): string {
  switch (category) {
    case CategoryType.Expense:
      return "Izdevumi";
    default:
      return "Ienākumi";
  }
}

const { open: openAddCategory } = inject(addCategoryKey)!;
const { open: openEditCategory } = inject(editCategoryKey)!;
const { open: openTransactionList } = inject(transactionListKey)!;
const { open: openAddTransaction } = inject(addEditTransactionKey)!;
</script>

<template>
  <div class="container-fluid transaction-container">
    <div class="row sticky-header second-row-height border-bottom border-end text-center">
      <div class="col text-center full-center-text fs-5">
        <div>{{ mapCategoryTypeName(categoryType) }}</div>
        <div>{{ transactionSum.toEurFormat() }}</div>
      </div>
    </div>
    <div class="row flex-grow-1 border-end">
      <div class="col">
        <div class="transaction-list d-flex flex-wrap">
          <CategoryButton
            v-for="category in categories"
            :key="category.categoryId"
            :category="category"
            class="category-width user-select-none"
            @left-click="(c) => openTransactionList(c, categoryType)"
            @right-click="openEditCategory"
            @double-click="
              console.log('double click');
              openAddTransaction(category, categoryType);
            "
          />
          <AddCategoryButton
            :type="categoryType"
            class="category-width user-select-none"
            @left-click="openAddCategory(categoryType)"
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

  @media (min-width: 1490px) {
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
