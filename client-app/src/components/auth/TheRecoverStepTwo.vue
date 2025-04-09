<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useRoute } from "vue-router";
import { validatePassword } from "./TheRegister.vue";
import { api } from "@/api/auto-generated-client";
import { useNotification } from "@kyvg/vue3-notification";
import router from "@/router";
import FaIcon from "@/components/global/FaIcon.vue";

const route = useRoute();

const password = ref("");
const confirmPassword = ref("");
const passwordError = ref("");
const confirmPasswordError = ref("");

const notification = useNotification();

async function savePassword() {
  validateConfirmPassword();
  passwordError.value = validatePassword(password.value);

  if (confirmPasswordError.value || passwordError.value) {
    return;
  }

  await api.recoverChangePassword(route.query.token as string, password.value);
  notification.notify({
    title: "Parole",
    text: "Parole veiksmīgi mainīta.",
    duration: 4000,
    type: "success",
  });

  router.push("/login");
}

function validateConfirmPassword() {
  if (confirmPassword.value !== password.value) {
    confirmPasswordError.value = "Paroles nesakrīt.";
  } else {
    confirmPasswordError.value = "";
  }
}

onMounted(() => {
  console.log(route.query);
  console.log(route.query.value);
});
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
      <form class="container">
        <div class="row">
          <div class="col mb-2">Jūsu e-pasts: {{ route.query.email }}</div>
        </div>
        <div class="row mb-1">
          <label for="password" class="form-label col-4">Parole</label>
          <div class="col">
            <input
              id="password"
              v-model="password"
              class="form-control"
              type="password"
              autocomplete="current-password"
            />
            <p v-if="passwordError" class="error">{{ passwordError }}</p>
          </div>
        </div>
        <div class="row mb-1">
          <label for="password" class="form-label col-4">Atkārtojiet paroli</label>
          <div class="col">
            <input
              id="password2"
              v-model="confirmPassword"
              class="form-control"
              type="password"
              autocomplete="current-password"
            />
            <p v-if="confirmPasswordError" class="error">{{ confirmPasswordError }}</p>
          </div>
        </div>
        <div class="row">
          <div class="col-4"></div>
          <div class="col">
            <button class="btn btn-primary" @click.prevent="savePassword">Atjaunot paroli</button>
          </div>
        </div>
      </form>
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
  width: 20%;
}

.cross {
  width: 1.3rem;
  height: 1.3rem;
}

.error {
  color: red;
  font-size: 0.7rem;
  margin-bottom: 0rem;
}

.is-invalid {
  border-color: red;
  box-shadow: 0 0 0 0.2rem rgba(255, 0, 0, 0.25);
}

.text-danger {
  font-size: 0.875rem;
}
</style>
