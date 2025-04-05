<script setup lang="ts">
import { api } from "@/api/auto-generated-client";
import { onMounted } from "vue";
import { useRouter } from "vue-router";

const router = useRouter();

onMounted(async () => {
  await api.logout();
  router.push("/login");
});
</script>

<template>
  <div>
    <title>Izrakstities</title>
  </div>
</template>
