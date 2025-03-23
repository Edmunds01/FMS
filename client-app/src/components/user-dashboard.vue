<script setup lang="ts">
import DateSelect from '../components/date-select.vue';
import Accounts from './dashboard/accounts.vue';

const startDate = new Date();
const endDate = new Date(2026, 9, 18);
</script>

<template>
  <div class="container-fluid vh-100 text-center">
    <div class="row vh-100">
      <div class="col-2">
        <Accounts />
      </div>
      <div class="col">
        <div class="container-fluid vh-100 text-center">
          <div class="row h-10">
            <div class="col p-0">
              <DateSelect :startDate="startDate" :endDate="endDate" />
            </div>
          </div>
          <div class="row">
            <div class="col-3">
              Expenses
            </div>
            <div class="col-3">
              Income
            </div>
            <div class="col">
              Stats
            </div>
          </div>
        </div>
      </div>
    </div>

  </div>

</template>

<style scoped>
.h-10 {
  height: 10%;
}

/* Custom styles (if needed) */
.bg-primary {
  background-color: #b9d2fa !important;
}

.bg-success {
  background-color: #cfe9c7 !important;
}

.bg-info {
  background-color: #e0f7fa !important;
}

.bg-warning {
  background-color: #ffecb3 !important;
}

.bg-success {
  background-color: #c8e6c9 !important;
}
</style>
