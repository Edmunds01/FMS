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
    category.value = categoryRaw;

    const onModalHidden = () => {
      closeEditCategory();
    };

    openModal(editCategoryModalId, onModalHidden);
  }

  function closeEditCategory() {
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
  const boolForWatch = ref(false);

  function openTransactionList(categoryRaw: Category, categoryTypeRaw: CategoryType) {
    category.value = categoryRaw;
    categoryType.value = categoryTypeRaw;

    openModal(transactionListModalId, closeTransactionList);
  }

  function closeTransactionList() {
    closeModal(transactionListModalId);

    setTimeout(() => {
      categoryType.value = undefined;
      category.value = undefined;
    }, 0);
  }

  function fetchData() {
    boolForWatch.value = !boolForWatch.value;
  }

  provide(transactionListKey, {
    category,
    categoryType,
    boolForWatch,
    fetchData,
    open: openTransactionList,
    close: closeTransactionList,
  });
}

export function useAddEditTransactionListModal() {
  const categoryType = ref<CategoryType | undefined>();
  const category = ref<Category | undefined>();
  const transaction = ref<Transaction | undefined>();

  const reopenFunctionToCall =
    ref<(categoryRaw: Category, categoryTypeRaw: CategoryType) => void | undefined>();

  function openTransactionList(
    categoryRaw: Category,
    categoryTypeRaw: CategoryType,
    reopenFunction?: (categoryRaw: Category, categoryTypeRaw: CategoryType) => void,
  ) {
    category.value = categoryRaw;
    categoryType.value = categoryTypeRaw;

    const onModalHidden = () => {
      closeTransactionList();
      if (reopenFunction) {
        reopenFunction(categoryRaw, categoryTypeRaw);
      }
    };

    openModal(transactionAddEditModalId, onModalHidden);
  }

  function openEditTransactionList(
    categoryRaw: Category,
    categoryTypeRaw: CategoryType,
    transactionRaw: Transaction,
    reopenFunction: (categoryRaw: Category, categoryTypeRaw: CategoryType) => void,
  ) {
    category.value = categoryRaw;
    categoryType.value = categoryTypeRaw;
    transaction.value = transactionRaw;
    reopenFunctionToCall.value = reopenFunction;

    const onModalHidden = () => {
      closeTransactionList();
    };

    openModal(transactionAddEditModalId, onModalHidden);
  }

  function closeTransactionList() {
    closeModal(transactionAddEditModalId);

    setTimeout(() => {
      if (isConfirmModal) return;

      if (reopenFunctionToCall.value) {
        reopenFunctionToCall.value(category.value!, categoryType.value!);
        reopenFunctionToCall.value = undefined;
      }

      categoryType.value = undefined;
      category.value = undefined;
      transaction.value = undefined;
    }, 0);
  }

  async function confirmDelete(text: string) {
    return await openConfirmModal(text, transactionAddEditModalId);
  }

  provide(addEditTransactionKey, {
    category,
    categoryType,
    transaction,
    openConfirmModal: confirmDelete,
    openAdd: openTransactionList,
    openEdit: openEditTransactionList,
    close: closeTransactionList,
  });
}

async function openConfirmModal(text: string, modelId: string) {
  return await confirm(text, modelId);
}
