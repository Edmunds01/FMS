<script setup lang="ts">
import { api, type Category, type NewCategory } from "@/api/auto-generated-client";
import { inject, ref, watch } from "vue";
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
const errorName = ref<string>();

const { close } = inject(addCategoryKey)!;

function validateCategoryName(): boolean {
  if (!newCategory.value.name) {
    errorName.value = "Kategorijas nosaukums ir obligāts.";
    return false;
  } else if ((newCategory.value.name?.length ?? 0) > 15) {
    errorName.value = "Kategorijas nosaukums nedrīkst pārsniegt 15 rakstzīmes.";
    return false;
  } else {
    errorName.value = undefined;
    return true;
  }
}

async function save() {
  if (!validateCategoryName()) {
    return;
  }

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

watch(
  () => newCategory.value.name,
  () => {
    validateCategoryName();
  },
);
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
            <div>
              <input
                v-model="newCategory.name"
                placeholder="Kategorijas nosaukums"
                type="text"
                class="form-control form-control-sm me-4"
                :class="{ 'is-invalid': errorName }"
                style="width: 30rem"
              />
              <div v-if="errorName" class="text-danger mt-1">
                {{ errorName }}
              </div>
            </div>
          </div>
        </div>
        <SaveOrCloseInModal @save="save" />
      </div>
    </template>
  </ModalWindow>
</template>

<style scoped>
.is-invalid {
  border-color: red;
  box-shadow: 0 0 0 0.2rem rgba(255, 0, 0, 0.25);
}

.text-danger {
  font-size: 0.875rem;
}
</style>
