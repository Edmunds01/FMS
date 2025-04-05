<script setup lang="ts">
import type { NewCategory } from "@/api/auto-generated-client";
import { ref } from "vue";
import SelectIconDropdown from "@/components/global/SelectIconDropdown.vue";
import SaveOrCloseInModal from "@/components/global/SaveOrCloseInModal.vue";
import ModalWindow from "@/components/global/ModalWindow.vue";

const props = defineProps<{
  id: string;
  newCategory: NewCategory;
}>();

const newCategory = ref(props.newCategory);

defineEmits<{
  (e: "add-category"): void;
}>();
</script>

<template>
  <ModalWindow :id="id" :height="6">
    <template #body>
      <div class="d-flex align-items-center h-100">
        <SelectIconDropdown
          :icon-name="newCategory.icon"
          @select-icon="(icon) => (newCategory.icon = icon)"
        />
        <div class="row flex-grow-1">
          <div class="col d-flex align-items-center justify-content-center">
            <input
              v-model="newCategory.name"
              placeholder="Kategorijas nosaukums"
              type="text"
              class="form-control form-control-sm me-4"
              style="width: 30rem"
            />
          </div>
        </div>
        <SaveOrCloseInModal @save="$emit('add-category')" />
      </div>
    </template>
  </ModalWindow>
</template>

<style scoped lang="scss"></style>
