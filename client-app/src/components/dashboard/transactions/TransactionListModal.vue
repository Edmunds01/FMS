<script setup lang="ts">
import { api, CategoryType, type Category, type Transaction } from "@/api/auto-generated-client";
import ModalWindow from "@/components/global/ModalWindow.vue";
import { computed, onMounted, ref } from "vue";
import FaIcon from "@/components/global/FaIcon.vue";

const props = defineProps<{
  id: string;
  category: Category;
  transactionType: CategoryType;
}>();

const transactions = ref<Transaction[]>();
const transactionClass = computed(() => props.transactionType === CategoryType.Expense ? "table-cell-expense" : "table-cell-income");

async function fetchTransactions() {
  transactions.value = await api.categoryTransactions(props.category.categoryId);
}

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
            <button title="Pievienot tranzakciju" class="add-button">
              <FaIcon icon-name="plus" size="sm" class="add-icon" />
            </button>
          </div>
        </div>
        <div class="table-responsive d-block">
          <table
            class="table table-bordered text-center table-striped table-bordered table-hover"
          >
            <thead>
              <tr>
                <th class="position-sticky">Summa</th>
                <th class="position-sticky">Kategorija</th>
                <th class="position-sticky">Datums</th>
              </tr>
            </thead>
            <tbody v-if="transactions && transactions.length > 0">
              <tr v-for="transaction in transactions" :key="transaction.transactionId">
                <td :class="transactionClass">{{ transaction.amount.toEurFormat() }}</td>
                <td :class="transactionClass">{{ transaction.comment }}</td>
                <td :class="transactionClass">{{ transaction.createdDateTime }}</td>
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
</style>
