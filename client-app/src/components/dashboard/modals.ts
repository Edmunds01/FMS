import type { Category, CategoryType, Transaction } from "@/api/auto-generated-client";
import {
  addCategoryKey,
  addEditTransactionKey,
  editCategoryKey,
  profileKey,
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
  userProfileModalId,
} from "../global/ModalWindow.vue";

import { useConfirm } from "@/utils/confirm";
import { isConfirmModal } from "../global/ConfirmAction.vue";

const confirm = useConfirm();

export function useProfileModal() {
  const isOpened = ref(false);

  function openProfile() {
    isOpened.value = true;

    const onModalHidden = () => {
      closeProfile();
    };

    openModal(userProfileModalId, onModalHidden);
  }

  function closeProfile() {
    closeModal(userProfileModalId);

    setTimeout(() => {
      isOpened.value = false;
    }, 0);
  }

  provide(profileKey, {
    isOpened,
    open: openProfile,
    close: closeProfile,
  });

  return openProfile;
}

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

  function openTransactionList(categoryRaw: Category) {
    category.value = categoryRaw;

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
    boolForWatch,
    fetchData,
    open: openTransactionList,
    close: closeTransactionList,
  });
}

export function useAddEditTransactionListModal() {
  const category = ref<Category | undefined>();
  const transaction = ref<Transaction | undefined>();

  const reopenFunctionToCall = ref<(categoryRaw: Category) => void | undefined>();

  function openTransactionList(
    categoryRaw: Category,
    reopenFunction?: (categoryRaw: Category) => void,
  ) {
    category.value = categoryRaw;

    const onModalHidden = () => {
      closeTransactionList();
      if (reopenFunction) {
        reopenFunction(categoryRaw);
      }
    };

    openModal(transactionAddEditModalId, onModalHidden);
  }

  function openEditTransactionList(
    categoryRaw: Category,
    transactionRaw: Transaction,
    reopenFunction: (categoryRaw: Category) => void,
  ) {
    category.value = categoryRaw;
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
        reopenFunctionToCall.value(category.value!);
        reopenFunctionToCall.value = undefined;
      }

      category.value = undefined;
      transaction.value = undefined;
    }, 0);
  }

  async function confirmDelete(text: string) {
    return await openConfirmModal(text, transactionAddEditModalId);
  }

  provide(addEditTransactionKey, {
    category,
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
