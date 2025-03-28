<script setup lang="ts">
import TransactionCategoryButton from './transaction-category-button.vue';

type TransactionType = 'Izdevumi' | 'Ienākumi';
const props = defineProps<{
    transactionType: TransactionType;
}>();

</script>

<template>
    <div class="container-fluid vh-100 transaction-container">
        <div class="row sticky-header second-row-height border-bottom border-end text-center">
            <div class="col text-center full-center-text fs-5">
                {{ transactionType }}
            </div>
        </div>
        <div class="row vh-100 border-end">
            <div class="col">
                <div class="transaction-list d-flex flex-wrap">
                    <TransactionCategoryButton v-for="i in 7" :key="i" :amount="20" :category="'test'" icon="faCalendar"
                        class="test-class" />
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped lang="scss">
.transaction-container {
  overflow-y: auto;

  .second-row-height {
    position: sticky;
    top: 0;
    z-index: 10;
    background-color: white;
  }
}

.transaction-list {
  display: flex;
  flex-wrap: wrap;
  gap: 10px;

  .test-class {
    flex: 0 0 calc(33.33% - 10px);
    margin-bottom: 10px;
}
}


</style>
