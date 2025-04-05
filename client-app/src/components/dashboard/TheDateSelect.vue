<script setup lang="ts">
import { format } from "date-fns";
import { lv } from "date-fns/locale";
import VueDatePicker, { type RangeConfig } from "@vuepic/vue-datepicker";
import "@vuepic/vue-datepicker/dist/main.css";
import { ref } from "vue";

const props = defineProps<{
  startDate: Date;
  endDate: Date;
}>();

const date = ref([props.startDate, props.endDate]);
const formatLatvianDate = (date: Date) =>
  format(date, "d. LLLL", { locale: lv }).replace(
    /(\d+\.\s*)([a-z])/,
    (_, p1, p2) => p1 + p2.toUpperCase(),
  );

function formatDate(dateRange: Date[]) {
  const startDate = dateRange[0];
  const endDate = dateRange[1];

  return `${formatLatvianDate(startDate)} - ${formatLatvianDate(endDate)}`;
}

const rangeConfig: RangeConfig = {
  maxRange: 366,
};
</script>

<template>
  <div class="date-container">
    <vue-date-picker
      v-model="date"
      locale="lv"
      multi-calendars
      :enable-time-picker="false"
      :format="formatDate"
      style="width: auto; background-color: black"
      :range="rangeConfig"
      :clearable="false"
    />
  </div>
</template>

<style scoped>
.date-container {
  margin-left: 2rem;
  align-items: center;
  display: flex;
  height: 100%;
}
</style>

<style>
.dp__input {
  border-radius: 0px;
}
</style>
