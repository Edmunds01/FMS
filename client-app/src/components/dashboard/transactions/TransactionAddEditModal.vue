<script setup lang="ts">
import { api, CategoryType, type Transaction } from "@/api/auto-generated-client";
import ModalWindow, {
  transactionAddModalId as transactionAddEditModalId,
} from "@/components/global/ModalWindow.vue";
import { computed, inject, onMounted, ref, watch } from "vue";
import {
  accountsKey,
  addEditTransactionKey,
  categoriesKey,
  transactionListKey,
} from "@/utils/keys";
import VueDatePicker from "@vuepic/vue-datepicker";
import { formatLatvianDate } from "../TheDateSelect.vue";
import FaIcon from "@/components/global/FaIcon.vue";
import { useNotification } from "@kyvg/vue3-notification";

const notification = useNotification();
const { categories, fetchCategories } = inject(categoriesKey)!;
const { accounts, fetchAccounts } = inject(accountsKey)!;
const {
  transaction: editTransaction,
  category,
  close,
  openConfirmModal,
} = inject(addEditTransactionKey)!;
const { boolForWatch } = inject(transactionListKey)!;

const transaction = ref<Transaction>({
  transactionId: editTransaction.value?.transactionId ?? undefined,
  accountId: editTransaction.value?.accountId ?? accounts.value[0].accountId,
  categoryId: editTransaction.value?.categoryId ?? category.value!.categoryId,
  comment: editTransaction.value?.comment ?? "",
  amount: editTransaction.value?.amount ?? 0,
  createdDateTime: editTransaction.value?.createdDateTime ?? new Date(),
});

const amountInput = ref<HTMLElement>();
const amount = ref((0).toEurFormat());
const firstCategoryType = ref<CategoryType>(CategoryType.Expense);

const errorComment = ref<string>();

const selectedCategory = computed(() => {
  return categories?.value.find((category) => category.categoryId === transaction.value.categoryId);
});

const selectedAccount = computed(() => {
  return accounts.value.find((account) => account.accountId === transaction.value.accountId);
});

const isEdit = computed(() => {
  return !!transaction.value.transactionId;
});

const transactionClass = computed(() => getCategoryStyle(selectedCategory.value?.type));

function getCategoryStyle(categoryType?: CategoryType) {
  return categoryType === CategoryType.Expense ? "table-cell-expense" : "table-cell-income";
}

const firstCategories = computed(() => {
  return categories?.value.filter((category) => category.type === firstCategoryType.value);
});

const secondCategories = computed(() => {
  return categories?.value.filter((category) => category.type !== firstCategoryType.value);
});

const errorAmount = ref<string>();

function validateAmount(): boolean {
  const number = amount.value.toNumberFromEurFormat();
  if (number < 0.01) {
    errorAmount.value = "Summai jābūt vismaz 0.01";
    return false;
  } else {
    errorAmount.value = undefined;
    return true;
  }
}

function validateComment(): boolean {
  if ((transaction.value.comment?.length ?? 0) > 50) {
    errorComment.value = "Komentārs nedrīkst pārsniegt 50 rakstzīmes.";
    return false;
  } else {
    errorComment.value = undefined;
    return true;
  }
}

async function save() {
  if (!validateComment() || !validateAmount()) {
    return;
  }

  transaction.value.amount = amount.value.toNumberFromEurFormat();

  updateAccountAndCategory();

  await api.upsertTransaction(transaction.value);
  notification.notify({
    title: isEdit.value ? "Transakcija labota" : "Transakcija pievienota",
    text: `Transakcija ${isEdit.value ? "labota" : "pievienota"} veiksmīgi.`,
    duration: 4000,
    type: "success",
  });

  setTimeout(async () => {
    fetchCategories();
    fetchAccounts();
  }, 0);

  close();
}

function updateAccountAndCategory() {
  const affectedCategory = categories!.value.find(
    (c) => c.categoryId === transaction.value.categoryId,
  )!;
  affectedCategory.sumOfTransactions -= transaction.value.amount;

  const affectedAccount = accounts.value.find((c) => c.accountId === transaction.value.accountId)!;
  if (affectedCategory.type === CategoryType.Expense) {
    affectedAccount.balance -= transaction.value.amount;
  } else {
    affectedAccount.balance += transaction.value.amount;
  }
}

async function deleteTransaction() {
  const result = await openConfirmModal(
    `Vēlaties izdzēst trasakciju uz summu ${transaction.value.amount.toEurFormat()}?`,
  );

  if (result) {
    updateAccountAndCategory();

    setTimeout(async () => {
      await api.deleteTransaction(transaction.value.transactionId!);
      await fetchCategories();
      boolForWatch.value = !boolForWatch.value;
      notification.notify({
        title: "Transakcija izdzēsta",
        text: `Transakcija izdzēsta veiksmīgi.`,
        duration: 4000,
        type: "success",
      });
    }, 0);

    close();
  }
}

onMounted(() => {
  firstCategoryType.value = category.value!.type;
  amount.value = transaction.value.amount.toString();
  onFocusLost();
});

watch(
  () => transaction.value.comment,
  () => {
    validateComment();
  },
);
watch(
  () => amount.value,
  () => {
    validateAmount();
  },
);

// #region Focus
function onFocusLost() {
  amount.value = amount.value
    .replace(",", ".")
    .replace(/(\..*?)\./g, "$1") // Remove all dots except first one
    .replace(/[a-zA-Z]/g, "");

  let number = Number(amount.value);

  number = number > 10_000_000 ? 10_000_000 : number;

  amount.value = (number || 0).toEurFormat();
}

function onFocus() {
  const value = amount.value.toNumberFromEurFormat();

  amount.value = value === 0 ? "" : value.toString();
}

function onKey(e: KeyboardEvent) {
  if (e.key == "Enter") {
    amountInput.value?.blur();
  }
}
// #endregion Focus
</script>

<template>
  <ModalWindow :id="transactionAddEditModalId">
    <template #body>
      <div class="row">
        <div class="col-6">
          <div class="dropdown">
            <button
              class="btn dropdown-toggle drop-down left-drop-down h1"
              type="button"
              data-bs-display="static"
              data-bs-toggle="dropdown"
              aria-expanded="false"
            >
              {{ selectedAccount?.name }}

              <ul class="dropdown-menu style-dropdown-menu dd-overflow">
                <li>
                  <span class="dropdown-header text-center" style="font-size: 1.6rem"> Konts </span>
                </li>
                <li><hr class="dropdown-divider" /></li>
                <li v-for="account in accounts" :key="account.accountId">
                  <a
                    class="dropdown-item text-center"
                    @click="() => (transaction.accountId = account.accountId)"
                  >
                    {{ account.name }}
                  </a>
                </li>
              </ul>
            </button>
          </div>
        </div>
        <div class="col-6">
          <div class="dropdown">
            <button
              class="btn dropdown-toggle drop-down right-drop-down"
              :class="transactionClass"
              type="button"
              data-bs-toggle="dropdown"
              aria-expanded="false"
            >
              {{ selectedCategory?.name }}
              <ul class="dropdown-menu style-dropdown-menu right dd-overflow">
                <li>
                  <span class="dropdown-header text-center" style="font-size: 1.6rem">
                    Kategorija
                  </span>
                </li>
                <li><hr class="dropdown-divider" /></li>
                <li v-for="cat in firstCategories" :key="cat.categoryId">
                  <a
                    class="dropdown-item text-center"
                    :class="getCategoryStyle(cat.type)"
                    @click="() => (transaction.categoryId = cat.categoryId)"
                  >
                    {{ cat.name }}
                  </a>
                </li>
                <li><hr class="dropdown-divider" /></li>
                <li v-for="cat in secondCategories" :key="cat.categoryId">
                  <a
                    class="dropdown-item text-center"
                    :class="getCategoryStyle(cat.type)"
                    @click="() => (transaction.categoryId = cat.categoryId)"
                  >
                    {{ cat.name }}
                  </a>
                </li>
              </ul>
            </button>
          </div>
        </div>
      </div>
      <div class="row">
        <div class="row">
          <div class="col-8">
            <div v-if="errorAmount" class="text-danger mt-1">
              {{ errorAmount }}
            </div>
            <input
              ref="amountInput"
              v-model="amount"
              type="text"
              class="sum-input"
              :class="{ 'is-invalid': errorAmount }"
              @focusout="onFocusLost"
              @focusin="onFocus"
              @keyup="onKey"
            />
          </div>
          <div class="col">
            <vue-date-picker
              v-model="transaction.createdDateTime"
              :enable-time-picker="false"
              :clearable="false"
              :format="formatLatvianDate"
              :ui="{ input: 'date' }"
              auto-apply
              class="mt-4"
              locale="lv"
            />
          </div>
        </div>
        <div class="col-8" :class="{ 'comment-row-height': errorComment }">
          <input
            v-model="transaction.comment"
            placeholder="Komentārs..."
            type="text"
            class="comment-input"
            :class="{ 'is-invalid': errorComment, 'comment-input-height': errorComment }"
            @keyup="
              (event) =>
                (event as KeyboardEvent).key === 'Enter'
                  ? (event.target as HTMLElement).blur()
                  : null
            "
          />
          <div v-if="errorComment" class="text-danger mt-1">
            {{ errorComment }}
          </div>
        </div>
        <div class="col">
          <div class="row">
            <button class="btn btn-primary save col" @click="save">Saglabāt</button>
            <button v-if="isEdit" class="col-2 mt-2" @click="deleteTransaction">
              <FaIcon icon-name="trash" size="xl" class="add-icon" />
            </button>
          </div>
        </div>
      </div>
    </template>
  </ModalWindow>
</template>

<style lang="scss">
.date {
  font-size: 1.4rem !important;
}
</style>

<style scoped lang="scss">
.dd-overflow {
  max-height: 40vh;
  overflow: auto;
}

.row,
.col-6 {
  margin-left: 0;
  margin-right: 0;
  padding-left: 0;
  padding-right: 0;
  background-color: #e4eef7;
}

.table-cell-income {
  background: linear-gradient(to bottom, #e1f4da, #eaf8ea, #ddf4da);
}

.table-cell-expense {
  background: linear-gradient(to bottom, #f4dada, #f6e4e4, #f4dada);
}

.drop-down {
  width: 100%;
  font-size: 2rem;
  padding: 1rem 0;
}

.left-drop-down {
  border-top-right-radius: 0%;
  border-bottom-right-radius: 0%;
  border-bottom-left-radius: 0%;
}

.style-dropdown-menu {
  left: 50% !important;
  transform: translateX(-50%) !important;
  top: 89% !important;
  font-size: 1.5rem;
  min-width: 100%;
  max-width: 100%;

  &.right {
    top: 99% !important;
  }
}

.right-drop-down {
  border-top-left-radius: 0%;
  border-bottom-left-radius: 0%;
  border-bottom-right-radius: 0%;

  width: calc(100% - 3px);
}

.sum-input {
  width: 100%;
  font-size: 2rem;
  padding: 1rem 0;
  border-radius: 0%;
  border-top-left-radius: 0%;
  text-align: center;
  border-bottom-left-radius: 0%;
}

.comment-input {
  @extend .sum-input;

  margin-top: 0.5rem;
  height: 70%;
  font-size: 1.3rem;
  padding: 1rem 0;
  border-radius: 0%;
  border-top-left-radius: 0%;
  text-align: center;
}

.save {
  margin-top: 0.5rem;
  font-size: 1.3rem;
  height: 45%;
  width: 100%;
  padding-left: 0;
  padding-right: 0;
}

.comment-row-height {
  height: 5rem;
}

.comment-input-height {
  height: 50%;
}

.is-invalid {
  border-color: red;
  box-shadow: 0 0 0 0.2rem rgba(255, 0, 0, 0.25);
}

.text-danger {
  font-size: 0.875rem;
}
</style>
