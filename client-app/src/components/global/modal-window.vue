<script setup lang="ts">
defineProps<{
  id: string;
  width?: number;
  height?: number;
}>();
</script>

<template>
  <div :id="id" class="modal" tabindex="-1">
    <div class="modal-dialog modal-lg modal-dialog-centered">
      <div class="modal-content">
        <div v-if="$slots.header" class="modal-header">
          <slot name="header" />
        </div>
        <div v-if="$slots.body" class="modal-body p-0">
          <slot name="body" />
        </div>
        <div v-if="$slots.footer" class="modal-footer">
          <slot name="footer" />
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped></style>
