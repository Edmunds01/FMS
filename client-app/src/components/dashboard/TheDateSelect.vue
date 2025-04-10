<!-- eslint-disable @typescript-eslint/no-unused-vars -->
<script lang="ts">
export function formatLatvianDate(date: Date) {
  return format(date, "d. LLLL", { locale: lv }).replace(
    /(\d+\.\s*)([a-z])/,
    (_, p1, p2) => p1 + p2.toUpperCase(),
  );
}
</script>
<script setup lang="ts">
import { format } from "date-fns";
import { lv } from "date-fns/locale";
import VueDatePicker, { type RangeConfig } from "@vuepic/vue-datepicker";
import "@vuepic/vue-datepicker/dist/main.css";
import { inject, ref, watch } from "vue";
import { selectedDatesKey } from "@/utils/keys";

const dates = inject(selectedDatesKey)!;

const date = ref([dates.startDate.value, dates.endDate.value]);

watch(date, ([newStartDate, newEndDate]) => {
  dates.startDate.value = newStartDate;
  dates.endDate.value = newEndDate;
});

watch([dates.startDate, dates.endDate], ([newStartDate, newEndDate]) => {
  date.value = [newStartDate, newEndDate];
});

function formatDate(dateRange: Date[]) {
  const startDate = dateRange[0];
  const endDate = dateRange[1];

  return `${formatLatvianDate(startDate)}` + (endDate ? ` - ${formatLatvianDate(endDate)}` : "");
}

const rangeConfig: RangeConfig = {
  maxRange: 366,
};
</script>

<template>
  <div class="date-container">
    <vue-date-picker
      v-model="date"
      :enable-time-picker="false"
      :format="formatDate"
      :range="rangeConfig"
      :clearable="false"
      :ui="{ input: 'date-select' }"
      locale="lv"
      multi-calendars
    >
      <template #input-icon> </template>
    </vue-date-picker>
  </div>
</template>

<style scoped lang="scss">
.date-container {
  margin-left: 2rem;
  align-items: center;
  display: flex;
  height: 100%;
  width: 50%;
}
</style>

<style>
.dp__input {
  border-radius: 0px;
  background: none !important;
  border: 0 !important;
}

.date-select {
  font-size: 2rem !important;
}
</style>
