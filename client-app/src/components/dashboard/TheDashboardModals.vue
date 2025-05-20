<script setup lang="ts">
import { inject } from "vue";
import {
  addCategoryKey,
  addEditTransactionKey,
  editCategoryKey,
  transactionListKey,
  profileKey,
} from "@/utils/keys";

import AddCategoryModal from "@/components/dashboard/categories/AddCategoryModal.vue";
import EditCategoryModal from "@/components/dashboard/categories/EditCategoryModal.vue";
import TransactionListModal from "@/components/dashboard/transactions/TheTransactionListModal.vue";
import TransactionAddEditModal from "@/components/dashboard/transactions/TransactionAddEditModal.vue";
import TheProfileModal from "@/components/dashboard/TheProfileModal.vue";

const { isOpened: isAddCategoryOpen } = inject(addCategoryKey)!;
const { category: isEditCategoryOpen } = inject(editCategoryKey)!;
const { category: isTransactionListOpen } = inject(transactionListKey)!;
const { category: isAddTransactionOpen } = inject(addEditTransactionKey)!;
const { isOpened: isProfileModalOpen } = inject(profileKey)!;
</script>

<template>
  <div>
    <TheProfileModal v-if="isProfileModalOpen" />
    <AddCategoryModal v-if="isAddCategoryOpen" />
    <EditCategoryModal v-if="!!isEditCategoryOpen" />
    <TransactionListModal v-if="!!isTransactionListOpen" />
    <TransactionAddEditModal v-if="!!isAddTransactionOpen" />
  </div>
</template>
