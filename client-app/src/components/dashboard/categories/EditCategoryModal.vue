<script setup lang="ts">
import { api } from "@/api/auto-generated-client";
import { inject, ref } from "vue";
import SelectIconDropdown from "@/components/global/SelectIconDropdown.vue";
import FaIcon from "@/components/global/FaIcon.vue";
import ModalWindow, { editCategoryModalId } from "@/components/global/ModalWindow.vue";
import { categoriesKey, editCategoryKey } from "@/utils/keys";

const { category, close, openConfirmModal } = inject(editCategoryKey)!;
const { categories, fetchCategories } = inject(categoriesKey)!;

const isEditMode = ref(false);
const newName = ref(category.value?.name ?? "");

async function deleteCategory() {
  const result = await openConfirmModal(`Vēlaties izdzēst kategoriju "${category.value!.name}?"`);

  if (result) {
    categories.value = categories.value.filter((c) => c.categoryId !== category.value!.categoryId);

    setTimeout(async () => {
      await api.deleteCategory(category.value!.categoryId);
      await fetchCategories();
    }, 0);

    close();
  }
}

async function iconChanged(icon: string) {
  category.value!.icon = icon;
  await api.saveCategoryIcon(category.value!.categoryId, icon);
}

async function nameChanged() {
  isEditMode.value = false;
  category.value!.name = newName.value;
  await api.saveCategoryName(category.value!.categoryId, newName.value);
}
</script>

<template>
  <ModalWindow :id="editCategoryModalId" :height="6" title="Kategorijas rediģēšana">
    <template #body>
      <div class="d-flex align-items-center h-100">
        <SelectIconDropdown
          :icon-name="category!.icon"
          @select-icon="(icon) => iconChanged(icon)"
        />
        <div class="row flex-grow-1">
          <div v-if="isEditMode" class="col d-flex align-items-center justify-content-center">
            <input
              v-model="newName"
              type="text"
              class="form-control form-control-sm me-4"
              style="width: 30rem"
            />
            <button class="p-0" @click="nameChanged()">
              <FaIcon icon-name="floppy-disk" size="lg" />
            </button>
            <button
              class="p-0 ms-4"
              @click="
                isEditMode = false;
                newName = category!.name ?? '';
              "
            >
              <FaIcon icon-name="xmark" size="lg" />
            </button>
          </div>
          <div v-else class="col d-flex align-items-center justify-content-center">
            <div class="p-0 me-5">{{ category!.name }}</div>
            <button class="p-0" @click="isEditMode = true">
              <FaIcon icon-name="pen" size="lg" />
            </button>
          </div>
        </div>
        <div class="d-flex justify-content-between align-items-center">
          <button v-if="category!.showDeleteButton" class="p-0" @click="deleteCategory">
            <FaIcon icon-name="trash" size="lg" />
          </button>
          <button
            type="button"
            class="btn-close ms-3 me-3"
            data-bs-dismiss="modal"
            aria-label="Close"
          ></button>
        </div>
      </div>
    </template>
  </ModalWindow>
</template>
