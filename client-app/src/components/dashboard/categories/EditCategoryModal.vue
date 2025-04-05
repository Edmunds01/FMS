<script setup lang="ts">
import { api, type Category } from "@/api/auto-generated-client";
import { ref } from "vue";
import SelectIconDropdown from "@/components/global/SelectIconDropdown.vue";
import ModalWindow from "@/components/global/ModalWindow.vue";
import FaIcon from "@/components/global/FaIcon.vue";
import { useConfirm } from "@/utils/confirm";

const props = defineProps<{
  id: string;
  category: Category;
}>();

const category = ref(props.category);
const isEditMode = ref(false);
const newName = ref(category.value.name);

const emit = defineEmits<{
  (e: "delete-category", categoryId: number): void;
}>();

const confirm = useConfirm();

async function deleteCategory() {
  const result = await confirm(
    "Apsitpriniet",
    `Vēlaties izdzēst kategoriju "${category.value.name}"`,
    props.id,
    true,
  );

  if (result) {
    emit("delete-category", category.value.categoryId);
  }
}

async function iconChanged(icon: string) {
  category.value.icon = icon;
  await api.saveCategoryIcon(category.value.categoryId, icon);
}

async function nameChanged() {
  isEditMode.value = false;
  category.value.name = newName.value;
  await api.saveCategoryName(category.value.categoryId, newName.value);
}
</script>

<template>
  <ModalWindow :id="id" :height="6">
    <template #body>
      <div class="d-flex align-items-center h-100">
        <SelectIconDropdown
          :icon-name="category.icon ?? ''"
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
                newName = category.name;
              "
            >
              <FaIcon icon-name="xmark" size="lg" />
            </button>
          </div>
          <div v-else class="col d-flex align-items-center justify-content-center">
            <div class="p-0 me-5">{{ category.name }}</div>
            <button class="p-0" @click="isEditMode = true">
              <FaIcon icon-name="pen" size="lg" />
            </button>
          </div>
        </div>
        <div class="d-flex justify-content-between align-items-center">
          <button v-if="category.showDeleteButton" class="p-0" @click="deleteCategory">
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

<style scoped lang="scss"></style>
