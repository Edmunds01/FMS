import type { Account, Category, CategoryType, Transaction } from "@/api/auto-generated-client";
import { type InjectionKey, type Ref } from "vue";

export const selectedDatesKey = Symbol() as InjectionKey<{
  startDate: Ref<Date>;
  endDate: Ref<Date>;
}>;

export const accountsKey = Symbol() as InjectionKey<{
  accounts: Ref<Account[]>;
  fetchAccounts: () => Promise<void>;
}>;

export const accountsKeyTest = Symbol() as InjectionKey<{
  accounts: Ref<Account[]>;
  fetchAccounts: () => Promise<void>;
}>;

export const categoriesKey = Symbol() as InjectionKey<{
  categories: Ref<Category[]>;
  fetchCategories: () => Promise<void>;
}>;

/**
 * [Implementation](../components/dashboard/modals.ts)
 */
export const addCategoryKey = Symbol() as InjectionKey<{
  isOpened: Ref<boolean>;
  categoryType: Ref<CategoryType | undefined>;
  open: (categoryTypeRaw: CategoryType) => void;
  close: () => void;
}>;

/**
 * [Implementation](../components/dashboard/modals.ts)
 */
export const editCategoryKey = Symbol() as InjectionKey<{
  category: Ref<Category | undefined>;
  open: (categoryRaw: Category) => void;
  close: () => void;
  openConfirmModal: (text: string) => Promise<boolean>;
}>;

/**
 * [Implementation](../components/dashboard/modals.ts)
 */
export const transactionListKey = Symbol() as InjectionKey<{
  category: Ref<Category | undefined>;
  boolForWatch: Ref<boolean>;
  fetchData: () => void;
  open: (categoryRaw: Category) => void;
  close: () => void;
}>;

/**
 * [Implementation](../components/dashboard/modals.ts)
 */
export const addEditTransactionKey = Symbol() as InjectionKey<{
  category: Ref<Category | undefined>;
  transaction: Ref<Transaction | undefined>;
  openConfirmModal: (text: string) => Promise<boolean>;
  openAdd: (categoryRaw: Category, reopenFunction?: (categoryRaw: Category) => void) => void;
  openEdit: (
    categoryRaw: Category,
    transactionRaw: Transaction,
    reopenFunction: (categoryRaw: Category) => void,
  ) => void;
  close: () => void;
}>;
