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
import { ref } from "vue";
import { useRouter } from "vue-router";
import FaIcon from "@/components/global/FaIcon.vue";

const notification = useNotification();

const router = useRouter();

const passwordError = ref("");
const confirmPasswordError = ref("");

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
  validatePassword(password.value);
  validateConfirmPassword();
  if (!passwordError.value && !confirmPasswordError.value) {
    await handleRegister();
  }
};

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
          <label for="username" class="form-label col-4">E-pasts</label>
          <div class="col">
            <input
              id="username"
              v-model="username"
              type="email"
              class="form-control"
              placeholder="example@example.com"
              autocomplete="username"
            />
          </div>
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
  width: 26%;
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
</style>
