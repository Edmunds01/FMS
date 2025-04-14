<script setup lang="ts">
import { ref } from "vue";
import FaIcon from "@/components/global/FaIcon.vue";
import { api } from "@/api/auto-generated-client";
import { useNotification } from "@kyvg/vue3-notification";
import router from "@/router";

const notification = useNotification();
const username = ref("");
const errorEmail = ref<string>();

function validateEmail(): boolean {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  if (!username.value) {
    errorEmail.value = "E-pasta adrese ir obligāta.";
    return false;
  } else if (!emailRegex.test(username.value)) {
    errorEmail.value = "Lūdzu ievadiet derīgu e-pasta adresi.";
    return false;
  } else {
    errorEmail.value = undefined;
    return true;
  }
}

async function sendEmail() {
  if (!validateEmail()) {
    return;
  }
  await api.recover(username.value);

  notification.notify({
    title: "Paroles atjaunošana",
    text: "Ja lietotājs ar šādu e-pastu eksistē, e-pasts ir nosūtīts.",
    duration: 5000,
    type: "success",
  });

  router.push("/login");
}
</script>

<template>
  <div class="recover">
    <title>Atjaunot paroli</title>

    <div class="border p-4 recover-form">
      <div class="border-bottom d-flex">
        <h1 class="flex-grow-1">Atjaunot paroli</h1>
        <a
          href="login"
          class="text-reset text-decoration-none d-flex align-items-center justify-content-center"
        >
          <FaIcon icon="fa-solid fa-xmark" class="ms-auto" size="2xl" />
        </a>
      </div>
      <div class="container">
        <div class="row">
          Ievadiet savu e-pastu un mēs atsutīsim jums saiti uz paroles atjaunošanu.
        </div>
        <div class="row mb-1 mt-2">
          <label class="form-label col-2">E-pasts</label>
          <div class="col">
            <input
              v-model="username"
              type="email"
              class="form-control"
              :class="{ 'is-invalid': errorEmail }"
              placeholder="example@example.com"
              autocomplete="username"
            />
            <div v-if="errorEmail" class="text-danger mt-1">
              {{ errorEmail }}
            </div>
          </div>
        </div>
        <div class="row">
          <div class="col-2"></div>
          <div class="col">
            <button class="btn btn-primary" @click="sendEmail">Atjaunot paroli</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.recover {
  height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
}

.recover-form {
  width: 40%;
}

.is-invalid {
  border-color: red;
  box-shadow: 0 0 0 0.2rem rgba(255, 0, 0, 0.25);
}

.text-danger {
  font-size: 0.875rem;
}
</style>
