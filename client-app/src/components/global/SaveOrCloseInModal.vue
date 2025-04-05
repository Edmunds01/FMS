<script setup lang="ts">
defineEmits<{
  (e: "save"): void;
}>();
</script>

<template>
  <div class="d-flex align-items-center">
    <button class="btn btn-primary px-4" @click="$emit('save')">Saglabāt</button>
    <button
      type="button"
      class="btn-close ms-3 me-3"
      data-bs-dismiss="modal"
      aria-label="Close"
    ></button>
  </div>
</template>

<style scoped>
.btn-close:hover {
  background-color: #f0f0f0;
  border-radius: 50%;
}

.btn-close {
  font-size: 1.25rem;
  width: 2rem;
  height: 2rem;
  display: flex;
  align-items: center;
  justify-content: center;
  background-color: transparent;
  border: none;
  cursor: pointer;
}
</style>
