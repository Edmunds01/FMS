<script setup lang="ts">
import { api } from "@/api/auto-generated-client";
import { useNotification } from "@kyvg/vue3-notification";
import { onMounted, ref } from "vue";
import { useRouter } from "vue-router";

const notification = useNotification();
const router = useRouter();

const username = ref("");
const password = ref("");
const showPassword = ref(false);

const loading = ref(false);

const handleLogin = async () => {
  if (loading.value) return;

  try {
    loading.value = true;
    await api.login({ username: username.value, password: password.value });
    router.push("/");
  } catch {
    loading.value = false;
    username.value = "";
    password.value = "";
    notification.notify({
      title: "Kļūda",
      text: "Parole vai e-pasts ir nepareizs",
      type: "error",
      duration: 3000,
    });
  }
};

onMounted(() => {
  api.logout();
});
</script>

<template>
  <div class="login">
    <title>Ieiet</title>

    <div class="border p-4 login-form">
      <h1 class="border-bottom">Ieiet</h1>
      <form class="container" @submit.prevent="handleLogin">
        <div class="row mb-1 mt-4">
          <label class="form-label col-3">E-pasts</label>
          <input
            v-model="username"
            type="email"
            class="form-control col w-auto"
            placeholder="example@example.com"
            autocomplete="username"
          />
        </div>
        <div class="row mb-1">
          <label class="form-label col-3">Parole</label>
          <div class="input-group col p-0">
            <input
              v-model="password"
              :type="showPassword ? 'text' : 'password'"
              class="form-control"
              autocomplete="password"
            />
            <button
              type="button"
              class="btn btn-outline-secondary"
              @click="showPassword = !showPassword"
            >
              {{ showPassword ? "Slēpt" : "Rādīt" }}
            </button>
          </div>
        </div>
        <div class="row text-center">
          <div class="col-3"></div>
          <div class="col"></div>
          <a class="recover link-primary ms-2 col" @click.prevent="$router.push('/recover')">
            Atjaunot paroli
          </a>
        </div>
        <div class="row">
          <div class="col-3"></div>
          <button type="submit" class="btn btn-primary col">Ieiet</button>
          <a type="button" href="register" class="btn btn-primary ms-2 col"> Registrēties </a>
        </div>
      </form>
    </div>
  </div>
</template>

<style scoped>
.login {
  height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
}

.recover {
  font-size: 0.8rem;
  cursor: pointer;
}

.login-form {
  width: 40%;
}

.input-group {
  display: flex;
  align-items: center;
}
</style>
