<script setup lang="ts">
import { ref } from 'vue'
import TransactionCategoryButton from './transaction-category-button.vue'

type TransactionType = 'Izdevumi' | 'Ienākumi'
defineProps<{
  transactionType: TransactionType
}>()

const foo = ref(4)
</script>

<template>
  <div class="container-fluid transaction-container">
    <div class="row sticky-header second-row-height border-bottom border-end text-center">
      <div class="col text-center full-center-text fs-5">
        {{ transactionType }}
      </div>
    </div>
    <div class="row flex-grow-1 border-end">
      <div class="col">
        <div class="transaction-list d-flex flex-wrap">
          <TransactionCategoryButton
            v-for="i in foo"
            :key="i"
            :amount="20 * i"
            :category="'test' + i"
            icon="faCalendar"
            class="test-class user-select-none"
            @click="
              () => {
                foo++
              }
            "
          />
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped lang="scss">
.transaction-container {
  display: flex;
  flex-direction: column;
  height: 100vh;
  overflow: hidden;
}

.sticky-header {
  position: sticky;
  top: 0;
  z-index: 10;
  background-color: white;
  flex-shrink: 0;
}

.row.flex-grow-1 {
  flex-grow: 1;
  overflow-y: auto;
}

.transaction-list {
  display: flex;
  flex-wrap: wrap;
  gap: 10px;
}

.test-class {
  flex: 0 0 calc(33.33% - 10px);
  margin-bottom: 10px;
}
</style>
