<script setup lang="ts">
import { api, CategoryType, type Category, type Transaction } from "@/api/auto-generated-client";
import ModalWindow from "@/components/global/ModalWindow.vue";
import { computed, onMounted, ref, watch } from "vue";
import FaIcon from "@/components/global/FaIcon.vue";
import { formatLatvianDate } from "../TheDateSelect.vue";

const props = defineProps<{
  id: string;
  category: Category;
  transactionType: CategoryType;
  needToReload: boolean;
}>();

watch(
  () => props.needToReload,
  () => fetchTransactions(),
);

defineEmits<{
  (e: "add-transaction"): void;
  (e: "edit-transaction", transaction: Transaction): void;
}>();

const transactions = ref<Transaction[]>();

type TransactionSortMode = {
  mode: 0 | 1 | 2;
  order: "asc" | "desc";
};

const sortMode = ref<TransactionSortMode>({ mode: 2, order: "desc" });
const transactionClass = computed(() =>
  props.transactionType === CategoryType.Expense ? "table-cell-expense" : "table-cell-income",
);

async function fetchTransactions() {
  transactions.value = await api.categoryTransactions(props.category.categoryId);
  sortByDate(false);
}

function sortByDate(isAscending: boolean) {
  if (!transactions.value) return;
  transactions.value.sort((a, b) => {
    const dateA = new Date(a.createdDateTime);
    dateA.setHours(0, 0, 0, 0);
    const dateB = new Date(b.createdDateTime);
    dateB.setHours(0, 0, 0, 0);

    const dateComparison = isAscending
      ? dateA.getTime() - dateB.getTime()
      : dateB.getTime() - dateA.getTime();

    return dateComparison || b.amount - a.amount;
  });
}

watch(
  () => sortMode.value,
  () => {
    if (!transactions.value) return;

    const { mode, order } = sortMode.value;
    const isAscending = order === "asc";

    switch (mode) {
      case 0:
        transactions.value.sort((a, b) =>
          isAscending ? a.amount - b.amount : b.amount - a.amount,
        );
        break;

      case 1:
        transactions.value.sort((a, b) => {
          const commentA = a.comment ?? "";
          const commentB = b.comment ?? "";
          return isAscending ? commentA.localeCompare(commentB) : commentB.localeCompare(commentA);
        });
        break;

      case 2:
        sortByDate(isAscending);
        break;
    }
  },
  { immediate: true },
);

onMounted(async () => {
  await fetchTransactions();
});
</script>

<template>
  <ModalWindow :id remove-bottom-border-radius>
    <template #body>
      <div>
        <div class="align-items-center position-relative">
          <div class="text-center h1 category-name">
            {{ category.name }}
            <button
              title="Pievienot tranzakciju"
              class="add-button"
              @click="$emit('add-transaction')"
            >
              <FaIcon icon-name="plus" size="sm" class="add-icon" />
            </button>
          </div>
        </div>
        <div class="table-responsive d-block">
          <table class="table table-bordered text-center table-striped table-bordered table-hover">
            <thead>
              <tr>
                <th
                  class="position-sticky user-select-none"
                  @click="
                    sortMode = {
                      mode: 0,
                      order: sortMode.order === 'desc' ? 'asc' : 'desc',
                    }
                  "
                >
                  Summa
                </th>
                <th
                  class="position-sticky user-select-none"
                  @click="
                    sortMode = {
                      mode: 1,
                      order: sortMode.order === 'desc' ? 'asc' : 'desc',
                    }
                  "
                >
                  Kategorija
                </th>
                <th
                  class="position-sticky user-select-none"
                  @click="
                    sortMode = {
                      mode: 2,
                      order: sortMode.order === 'desc' ? 'asc' : 'desc',
                    }
                  "
                >
                  Datums
                </th>
              </tr>
            </thead>
            <tbody v-if="transactions && transactions.length > 0">
              <tr v-for="transaction in transactions" :key="transaction.transactionId">
                <td
                  :class="transactionClass"
                  class="clickable"
                  @click="$emit('edit-transaction', transaction)"
                >
                  {{ transaction.amount.toEurFormat() }}
                </td>
                <td
                  :class="transactionClass"
                  class="clickable"
                  @click="$emit('edit-transaction', transaction)"
                >
                  {{ transaction.comment }}
                </td>
                <td
                  :class="transactionClass"
                  class="clickable"
                  @click="$emit('edit-transaction', transaction)"
                >
                  {{ formatLatvianDate(transaction.createdDateTime) }}
                </td>
              </tr>
            </tbody>
            <tbody v-else>
              <tr>
                <td colspan="3">Tranzakciju pagaidām nav</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </template>
  </ModalWindow>
</template>

<style scoped lang="scss">
.d-block {
  max-height: 70vh;
  padding: 0.1rem;
}

.table {
  width: 100%;
  border-collapse: separate;
  table-layout: fixed;
  border-spacing: 0;
  margin: 0;
}

th {
  position: sticky;
  top: 0;
  background-color: #e4eef7;
  z-index: 1;
  text-align: center;
  padding: 8px;
  border: 2px solid;
  border-color: #dae3eb;
}

td {
  text-align: center;
  padding: 8px;
  border: 1px solid #dee2e6;
  word-wrap: break-word;
  white-space: nowrap;
}

.table-cell-income {
  background: linear-gradient(to bottom, #e1f4da, #eaf8ea, #ddf4da);
}

.table-cell-expense {
  background: linear-gradient(to bottom, #f4dada, #f6e4e4, #f4dada);
}

.add-button {
  position: absolute;
  top: 50%;
  right: 15%;
  transform: translateY(-42%);
}

.add-icon {
  border: 4px solid;
  border-radius: 50%;
  padding: 0.35rem;
}

.category-name {
  padding: 1rem 10rem;
  margin: 0;
}

.clickable {
  cursor: pointer;
}
</style>
