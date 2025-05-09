<script lang="ts">
export function validatePassword(passwordRaw: string) {
  const minLength = 10;
  const hasNumber = /\d/;
  const hasUpperCase = /[A-Z]/;
  const hasLowerCase = /[a-z]/;
  const hasSpecialChar = /[!@#$%^&*(),.?":{}|<>]/;

  let passwordError = "";

  if (passwordRaw.length < minLength) {
    passwordError = `Parolei jābūt vismaz ${minLength} simbolu garam.`;
  } else if (!hasNumber.test(passwordRaw) && !hasSpecialChar.test(passwordRaw)) {
    passwordError = "Parolei jābūt vismaz vienam ciparam vai speciālam simbolam.";
  } else if (!hasUpperCase.test(passwordRaw)) {
    passwordError = "Parolei jābūt vismaz vienam lielajam burtam.";
  } else if (!hasLowerCase.test(passwordRaw)) {
    passwordError = "Parolei jābūt vismaz vienam mazajam burtam.";
  } else {
    passwordError = "";
  }

  return passwordError;
}
</script>

<script setup lang="ts">
// TODO: Add unsuccessful register alert
// TODO: Change redirection link

import { api } from "@/api/auto-generated-client";
import { useNotification } from "@kyvg/vue3-notification";
import { ref, watch } from "vue";
import { useRouter } from "vue-router";
import FaIcon from "@/components/global/FaIcon.vue";

const notification = useNotification();

const router = useRouter();

const passwordError = ref("");
const confirmPasswordError = ref("");

const showPassword = ref(false);
const showConfirmPassword = ref(false);

const username = ref("");
const password = ref("");
const confirmPassword = ref("");

const validateConfirmPassword = () => {
  if (confirmPassword.value !== password.value) {
    confirmPasswordError.value = "Paroles nesakrīt.";
  } else {
    confirmPasswordError.value = "";
  }
};

const validateForm = async () => {
  passwordError.value = validatePassword(password.value);
  validateConfirmPassword();
  if (!passwordError.value && !confirmPasswordError.value) {
    await handleRegister();
  }
};

watch(
  () => password.value,
  () => {
    if (passwordError.value) {
      passwordError.value = validatePassword(password.value);
    }
  },
);

watch(
  () => confirmPassword.value,
  () => {
    if (confirmPasswordError.value) {
      validateConfirmPassword();
    }
  },
);

const handleRegister = async () => {
  try {
    await api.register({
      username: username.value,
      password: password.value,
    });
    notification.notify({
      title: "Reģistrācija",
      text: "Reģistrācija veiksmīga.",
      duration: 4000,
      type: "success",
    });
    router.push("/");
  } catch {
    notification.notify({
      title: "Reģistrācija",
      text: "Lietotājs ar šādu e-pastu jau eksistē.",
      duration: 4000,
      type: "error",
    });

    username.value = "";
    password.value = "";
    confirmPassword.value = "";
  }
};
</script>

<template>
  <div class="register">
    <title>Reģistrēties</title>

    <div class="border p-4 register-form">
      <div class="border-bottom d-flex">
        <h1 class="flex-grow-1">Reģistrēties</h1>
        <a
          href="login"
          class="text-reset text-decoration-none d-flex align-items-center justify-content-center"
        >
          <FaIcon icon="fa-solid fa-xmark" class="ms-auto" size="2xl" />
        </a>
      </div>
      <form class="container" @submit.prevent="validateForm">
        <div class="row mb-1 mt-2">
          <label class="form-label col-4">E-pasts</label>
          <div class="col">
            <input
              v-model="username"
              type="email"
              class="form-control"
              placeholder="example@example.com"
              autocomplete="username"
            />
          </div>
        </div>
        <div class="row mb-1">
          <label class="form-label col-4">Parole</label>
          <div class="col input-group">
            <input
              v-model="password"
              class="form-control"
              :type="showPassword ? 'text' : 'password'"
              autocomplete="current-password"
            />
            <button
              type="button"
              class="btn btn-outline-secondary"
              @click="showPassword = !showPassword"
            >
              {{ showPassword ? "Slēpt" : "Rādīt" }}
            </button>
          </div>
          <div class="row">
            <p v-if="passwordError" class="col-4"></p>
            <p v-if="passwordError" class="error col">{{ passwordError }}</p>
          </div>
        </div>
        <div class="row mb-1">
          <label class="form-label col-4">Atkārtojiet paroli</label>
          <div class="col input-group">
            <input
              v-model="confirmPassword"
              class="form-control"
              :type="showConfirmPassword ? 'text' : 'password'"
              autocomplete="current-password"
            />
            <button
              type="button"
              class="btn btn-outline-secondary"
              @click="showConfirmPassword = !showConfirmPassword"
            >
              {{ showConfirmPassword ? "Slēpt" : "Rādīt" }}
            </button>
          </div>
        </div>
        <div class="row">
          <p v-if="confirmPasswordError" class="col-4"></p>
          <p v-if="confirmPasswordError" class="error col">{{ confirmPasswordError }}</p>
        </div>
        <div class="row">
          <div class="col-4"></div>
          <div class="col">
            <button type="submit" class="btn btn-primary">Registrēties</button>
          </div>
        </div>
      </form>
    </div>
  </div>
</template>

<style scoped>
.register {
  height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
}

.register-form {
  width: 40%;
}

.error {
  color: red;
  font-size: 0.7rem;
  margin-bottom: 0rem;
}
</style>
