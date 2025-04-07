<script setup lang="ts">
import CategoryButton from "@/components/dashboard/categories/CategoryButton.vue";
import AddCategoryButton from "@/components/dashboard/categories/AddCategoryButton.vue";
import { closeModal, openModal } from "@/components/global/ModalWindow.vue";
import { computed, ref } from "vue";
import { icons } from "@/components/global/FaIcon.vue";
import {
  api,
  CategoryType,
  type Category,
  type NewCategory,
  type Transaction,
} from "@/api/auto-generated-client";
import AddCategoryModal from "@/components/dashboard/categories/AddCategoryModal.vue";
import EditCategoryModal from "@/components/dashboard/categories/EditCategoryModal.vue";
import { isConfirmModal } from "@/components/global/ConfirmAction.vue";
import TransactionListModal from "../transactions/TransactionListModal.vue";
import TransactionAddModal from "../transactions/TransactionAddModal.vue";

const addCategoryModalId = "addCategoryModal";
const editCategoryModalId = "editCategoryModal";
const transactionListModalId = "transactionListModal";
const transactionAddModalId = "transactionAddModal";

const props = defineProps<{
  transactionType: CategoryType;
  categories: Category[];
}>();

const emit = defineEmits<{
  (e: "add-category", category: NewCategory): void;
  (e: "delete-category", categoryId: number): void;
}>();

const newCategory = ref<NewCategory>({
  name: "",
  icon: icons[0],
  type: CategoryType.Expense,
});

const selectedCategory = ref<Category | null>(null);
const selectedTransactionCategory = ref<Category | null>(null);
const selectedTransaction = ref<Transaction | null>(null);

const isAddCategory = ref(false);
const isAddTransaction = ref(false);
const openedFromTransactionList = ref(false);
const needToReload = ref(false);

function showAddCategoryModal() {
  isAddCategory.value = true;

  const onModalHidden = () => {
    isAddCategory.value = false;
    newCategory.value.name = "";
    newCategory.value.icon = icons[0];
  };

  openModal(addCategoryModalId, onModalHidden);
}

function showEditCategoryModal(category: Category) {
  selectedCategory.value = category;

  const onModalHidden = () => {
    if (!isConfirmModal) {
      selectedCategory.value = null;
    }
  };

  openModal(editCategoryModalId, onModalHidden);
}

function showTransactionListModal(category: Category) {
  selectedTransactionCategory.value = category;

  const onModalHidden = () => {
    if (!isAddTransaction.value && !selectedTransaction.value) {
      selectedTransactionCategory.value = null;
    }
  };

  openModal(transactionListModalId, onModalHidden);
}

function showTransactionAddModal(category: Category | null = null) {
  if (category) {
    selectedTransactionCategory.value = category;
  } else {
    openedFromTransactionList.value = true;
  }

  isAddTransaction.value = true;
  closeModal(transactionListModalId, true);

  const onModalHidden = () => {
    if (!openedFromTransactionList.value) {
      selectedTransactionCategory.value = null;
    }

    openedFromTransactionList.value = false;
    isAddTransaction.value = false;
  };

  openModal(transactionAddModalId, onModalHidden);
}

function showTransactionEditModal(transaction: Transaction) {
  selectedTransaction.value = transaction;
  openedFromTransactionList.value = true;

  closeModal(transactionListModalId, true);

  const onModalHidden = () => {
    if (!openedFromTransactionList.value) {
      selectedTransactionCategory.value = null;
      selectedTransaction.value = null;
    }

    openedFromTransactionList.value = false;
  };

  openModal(transactionAddModalId, onModalHidden);
}

function addTransaction(transaction: Transaction) {
  setTimeout(async () => {
    await api.upsertTransaction(transaction);
    needToReload.value = !needToReload.value;
  }, 0);

  closeModal(transactionAddModalId);

  if (openedFromTransactionList.value) {
    showTransactionListModal(selectedTransactionCategory.value!);
  }
}

const transactionSum = computed(() => {
  return (
    props.categories.reduce((sum, category) => {
      return sum + category.sumOfTransactions;
    }, 0) ?? 0
  );
});

async function addCategory() {
  const newCategoryData = {
    ...newCategory.value,
    type: props.transactionType,
  };

  newCategory.value.name = "";
  newCategory.value.icon = icons[0];
  emit("add-category", newCategoryData);
  closeModal(addCategoryModalId);
}

async function deleteCategory(categoryId: number) {
  closeModal(editCategoryModalId);

  emit("delete-category", categoryId);

  api.deleteCategory(categoryId);
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
          <CategoryButton
            v-for="category in categories"
            :key="category.categoryId"
            :category="category"
            class="category-width user-select-none"
            @left-click="showTransactionListModal(category)"
            @right-click="showEditCategoryModal(category)"
            @double-click="showTransactionAddModal(category)"
          />
          <AddCategoryButton
            :type="transactionType"
            class="category-width user-select-none"
            @left-click="showAddCategoryModal"
          />
        </div>
      </div>
    </div>
    <AddCategoryModal
      v-if="isAddCategory"
      :id="addCategoryModalId"
      :new-category="newCategory"
      @add-category="addCategory"
      @delete-category="deleteCategory"
    />
    <EditCategoryModal
      v-if="selectedCategory"
      :id="editCategoryModalId"
      :category="selectedCategory"
      @delete-category="deleteCategory"
    />
    <TransactionListModal
      v-if="selectedTransactionCategory"
      :id="transactionListModalId"
      :category="selectedTransactionCategory"
      :transaction-type
      :need-to-reload
      @add-transaction="showTransactionAddModal"
      @edit-transaction="showTransactionEditModal"
    />
    <TransactionAddModal
      v-if="isAddTransaction || selectedTransaction"
      :id="transactionAddModalId"
      :from-category="selectedTransactionCategory!"
      :transaction="selectedTransaction"
      @add-transaction="addTransaction"
    />
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
