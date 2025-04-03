<script setup lang="ts">
import TransactionCategoryButton from "./category-buttons/transaction-category-button.vue";
import AddTransactionCategoryButton from "./category-buttons/add-transaction-category-button.vue";
import ModalWindow, { openModal } from "../global/modal-window.vue";
import { computed, ref } from "vue";
import { icons } from "../global/fa-icon.vue";
import IconDropdown from "./accounts/icon-dropdown.vue";
import {
  api,
  CategoryType,
  type Category,
  type NewCategory as TempCategory,
} from "@/api/auto-generated-client";
import SaveOrClose from "../global/save-or-close.vue";

const props = defineProps<{
  transactionType: CategoryType;
  categories: Category[];
}>();

const emit = defineEmits<{
  (e: "add-category"): void;
}>();

const newCategory = ref<TempCategory>({
  name: "",
  icon: icons[0],
  type: CategoryType.Expense,
});

const isAddCategory = ref(false);
const modalId = "addCategory";

function showAddCategoryModal() {
  isAddCategory.value = true;

  const onModalHidden = () => {
    isAddCategory.value = false;
    newCategory.value.name = "";
    newCategory.value.icon = icons[0];
  };

  openModal(modalId, onModalHidden);
}

const transactionSum = computed(() => {
  return (
    props.categories.reduce((sum, category) => {
      return sum + category.sumOfTransactions;
    }, 0) ?? 0
  );
});

async function addNewCategory() {
  const newCategoryData = {
    ...newCategory.value,
    type: props.transactionType,
  };

  await api.addCategory(newCategoryData);

  newCategory.value.name = "";
  newCategory.value.icon = icons[0];
  emit("add-category");
}

function mapCategoryTypeName(category: CategoryType): string {
  switch (category) {
    case CategoryType.Expense:
      return "Izdevumi";
    default:
      return "Ienākumi";
  }
}
</script>

<template>
  <div class="container-fluid transaction-container">
    <div class="row sticky-header second-row-height border-bottom border-end text-center">
      <div class="col text-center full-center-text fs-5">
        <div>{{ mapCategoryTypeName(transactionType) }}</div>
        <div>{{ transactionSum.toEurFormat() }}</div>
      </div>
    </div>
    <div class="row flex-grow-1 border-end">
      <div class="col">
        <div class="transaction-list d-flex flex-wrap">
          <TransactionCategoryButton
            v-for="category in categories"
            :key="category.categoryId"
            :category="category"
            class="category-width user-select-none"
          />
          <AddTransactionCategoryButton
            :type="transactionType"
            class="category-width user-select-none"
            @click="showAddCategoryModal"
          />
        </div>
      </div>
    </div>
    <ModalWindow v-if="isAddCategory" :id="modalId" :height="6">
      <template #body>
        <div class="d-flex align-items-center h-100">
          <IconDropdown
            :icon-name="newCategory.icon"
            @select-icon="(icon) => (newCategory.icon = icon)"
          />
          <div class="row flex-grow-1">
            <div class="col d-flex align-items-center justify-content-center">
              <input
                v-model="newCategory.name"
                placeholder="Kategorijas nosaukums"
                type="text"
                class="form-control form-control-sm me-4"
                style="width: 30rem"
              />
            </div>
          </div>
          <SaveOrClose @save="addNewCategory" />
        </div>
      </template>
    </ModalWindow>
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
