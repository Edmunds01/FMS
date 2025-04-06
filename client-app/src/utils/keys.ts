import type { Account, Category } from "@/api/auto-generated-client";
import { type InjectionKey, type Ref } from "vue";

export const accountsKey = Symbol() as InjectionKey<
{
    accounts: Ref<Account[]>;
    fetchAccounts: () => Promise<void>;
}>;
export const categoriesKey = Symbol() as InjectionKey<Ref<Category[]>>;

