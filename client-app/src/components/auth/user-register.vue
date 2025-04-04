<script setup lang="ts">
// TODO: Add unsuccessful register alert
// TODO: Change redirection link

import { api } from "@/api/auto-generated-client";
import { ref } from "vue";
import { useRouter } from "vue-router";

const router = useRouter();

const passwordError = ref("");
const confirmPasswordError = ref("");

const username = ref("");
const password = ref("");
const confirmPassword = ref("");

const validatePassword = () => {
  const minLength = 10;
  const hasNumber = /\d/;
  const hasUpperCase = /[A-Z]/;
  const hasLowerCase = /[a-z]/;
  const hasSpecialChar = /[!@#$%^&*(),.?":{}|<>]/;

  if (password.value.length < minLength) {
    passwordError.value = `PParolei jābūt vismaz ${minLength} simbolu garam.`;
  } else if (!hasNumber.test(password.value) && !hasSpecialChar.test(password.value)) {
    passwordError.value = "Parolei jābūt vismaz vienam ciparam vai speciālam simbolam.";
  } else if (!hasUpperCase.test(password.value)) {
    passwordError.value = "Parolei jābūt vismaz vienam lielajam burtam.";
  } else if (!hasLowerCase.test(password.value)) {
    passwordError.value = "Parolei jābūt vismaz vienam mazajam burtam.";
  } else {
    passwordError.value = "";
  }
};

const validateConfirmPassword = () => {
  if (confirmPassword.value !== password.value) {
    confirmPasswordError.value = "Paroles nesakrīt.";
  } else {
    confirmPasswordError.value = "";
  }
};

const validateForm = async () => {
  validatePassword();
  validateConfirmPassword();
  if (!passwordError.value && !confirmPasswordError.value) {
    await handleRegister();
  }
};

const handleRegister = async () => {
  try {
    const token = await api.register({
      username: username.value,
      password: password.value,
    });
    localStorage.setItem("token", token.token ?? "");
    router.push("/login");
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
          <fa-icon icon="fa-solid fa-xmark" class="ms-auto" size="2xl" />
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
