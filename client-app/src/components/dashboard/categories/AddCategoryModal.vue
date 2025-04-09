<script setup lang="ts">
import { api, type Category, type NewCategory } from "@/api/auto-generated-client";
import { inject, ref } from "vue";
import SelectIconDropdown from "@/components/global/SelectIconDropdown.vue";
import SaveOrCloseInModal from "@/components/global/SaveOrCloseInModal.vue";
import ModalWindow, { addCategoryModalId } from "@/components/global/ModalWindow.vue";
import { categoriesKey, addCategoryKey } from "@/utils/keys";
import { useNotification } from "@kyvg/vue3-notification";

const notification = useNotification();
const { categories, fetchCategories } = inject(categoriesKey)!;
const { categoryType } = inject(addCategoryKey)!;

const defaultCategory = {
  name: "",
  icon: "wallet",
  type: categoryType.value,
} as NewCategory;

const newCategory = ref({ ...defaultCategory });

const { close } = inject(addCategoryKey)!;

// TODO: do not allow to save with empty name
async function save() {
  categories.value.push({
    ...newCategory.value,
    sumOfTransactions: 0,
    showDeleteButton: true,
  } as Category);

  setTimeout(async () => {
    await api.addCategory(newCategory.value);
    await fetchCategories();

    notification.notify({
      title: "Jauna kategorija pievienota",
      text: `Jauna kategorija "${newCategory.value.name}" ir pievienota.`,
      duration: 4000,
      type: "success",
    });
    newCategory.value = { ...defaultCategory };
  }, 0);

  close();
}
</script>

<template>
  <ModalWindow :id="addCategoryModalId" :height="6" title="Jaunas kategorijas pievienošana">
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
        <SaveOrCloseInModal @save="save" />
      </div>
    </template>
  </ModalWindow>
</template>
