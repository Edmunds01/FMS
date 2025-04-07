import type { Account, Category, CategoryType, Transaction } from "@/api/auto-generated-client";
import { type InjectionKey, type Ref } from "vue";

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
  categoryType: Ref<CategoryType | undefined>;
  boolForWatch: Ref<boolean>;
  fetchData: () => void;
  open: (categoryRaw: Category, categoryTypeRaw: CategoryType) => void;
  close: () => void;
}>;

/**
 * [Implementation](../components/dashboard/modals.ts)
 */
export const addEditTransactionKey = Symbol() as InjectionKey<{
  category: Ref<Category | undefined>;
  categoryType: Ref<CategoryType | undefined>;
  transaction: Ref<Transaction | undefined>;
  openAdd: (
    categoryRaw: Category,
    categoryTypeRaw: CategoryType,
    reopenFunction?: (categoryRaw: Category, categoryTypeRaw: CategoryType) => void,
  ) => void;
  openEdit: (
    categoryRaw: Category,
    categoryTypeRaw: CategoryType,
    transactionRaw: Transaction,
    reopenFunction: (categoryRaw: Category, categoryTypeRaw: CategoryType) => void,
  ) => void;
  close: () => void;
}>;
