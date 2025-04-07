import type { Category, CategoryType, Transaction } from "@/api/auto-generated-client";
import {
  addCategoryKey,
  addEditTransactionKey,
  editCategoryKey,
  transactionListKey,
} from "@/utils/keys";
import { provide, ref } from "vue";
import {
  addCategoryModalId,
  closeModal,
  editCategoryModalId,
  openModal,
  transactionAddModalId as transactionAddEditModalId,
  transactionListModalId,
} from "../global/ModalWindow.vue";

import { useConfirm } from "@/utils/confirm";
import { isConfirmModal } from "../global/ConfirmAction.vue";

const confirm = useConfirm();

export function useAddCategoryModal() {
  const isOpened = ref(false);
  const categoryType = ref<CategoryType | undefined>();

  function openAddCategory(categoryTypeRaw: CategoryType) {
    isOpened.value = true;
    categoryType.value = categoryTypeRaw;

    const onModalHidden = () => {
      closeAddCategory();
    };

    openModal(addCategoryModalId, onModalHidden);
  }

  function closeAddCategory() {
    console.log("Close add category");
    closeModal(addCategoryModalId);

    setTimeout(() => {
      isOpened.value = false;
      categoryType.value = undefined;
    }, 0);
  }

  provide(addCategoryKey, {
    isOpened,
    categoryType,
    open: openAddCategory,
    close: closeAddCategory,
  });
}

export function useEditCategoryModal() {
  const category = ref<Category | undefined>();

  function openEditCategory(categoryRaw: Category) {
    console.log("Open Edit Category");

    category.value = categoryRaw;

    const onModalHidden = () => {
      closeEditCategory();
    };

    openModal(editCategoryModalId, onModalHidden);
  }

  function closeEditCategory() {
    console.log("Close edit category");
    closeModal(editCategoryModalId);

    setTimeout(() => {
      if (!isConfirmModal) {
        category.value = undefined;
      }
    }, 0);
  }

  async function confirmDelete(text: string) {
    return await openConfirmModal(text, editCategoryModalId);
  }

  provide(editCategoryKey, {
    category,
    open: openEditCategory,
    close: closeEditCategory,
    openConfirmModal: confirmDelete,
  });
}

export function useTransactionListModal() {
  const categoryType = ref<CategoryType | undefined>();
  const category = ref<Category | undefined>();

  function openTransactionList(categoryRaw: Category, categoryTypeRaw: CategoryType) {
    console.log("Open Transaction List");

    category.value = categoryRaw;
    categoryType.value = categoryTypeRaw;

    openModal(transactionListModalId, closeTransactionList);
  }

  function closeTransactionList() {
    console.log("Close transaction list");
    closeModal(transactionListModalId);

    setTimeout(() => {
      categoryType.value = undefined;
      category.value = undefined;
    }, 0);
  }

  provide(transactionListKey, {
    category,
    categoryType,
    open: openTransactionList,
    close: closeTransactionList,
  });
}

export function useAddTransactionListModal() {
  const categoryType = ref<CategoryType | undefined>();
  const category = ref<Category | undefined>();
  const transaction = ref<Transaction | undefined>();

  function openTransactionList(
    categoryRaw: Category,
    categoryTypeRaw: CategoryType,
    transactionRaw?: Transaction,
  ) {
    console.log("Open Add Transaction");

    category.value = categoryRaw;
    categoryType.value = categoryTypeRaw;
    transaction.value = transactionRaw;

    openModal(transactionAddEditModalId, closeTransactionList);
  }

  function closeTransactionList() {
    console.log("Close Add Transaction");
    closeModal(transactionAddEditModalId);

    setTimeout(() => {
      categoryType.value = undefined;
      category.value = undefined;
      transaction.value = undefined;
    }, 0);
  }

  provide(addEditTransactionKey, {
    category,
    categoryType,
    transaction,
    open: openTransactionList,
    close: closeTransactionList,
  });
}

async function openConfirmModal(text: string, modelId: string) {
  return await confirm(text, modelId);
}
