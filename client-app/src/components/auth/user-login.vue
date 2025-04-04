<script setup lang="ts">
import { api } from "@/api/auto-generated-client";
import { ref } from "vue";
import { useRouter } from "vue-router";

const router = useRouter();

const username = ref("");
const password = ref("");

const handleLogin = async () => {
  try {
    const token = await api.login({ username: username.value, password: password.value });
    localStorage.setItem("token", token.token ?? "");
    router.push("/");
  } catch {
    username.value = "";
    password.value = "";
  }
};
</script>

<template>
  <div class="login">
    <title>Ieiet</title>

    <div class="border p-4 w-25">
      <h1 class="border-bottom">Ieiet</h1>
      <form class="container" @submit.prevent="handleLogin">
        <div class="row mb-1 mt-4">
          <label for="username" class="form-label col-3">E-pasts</label>
          <input
            id="username"
            v-model="username"
            type="email"
            class="form-control col w-auto"
            placeholder="example@example.com"
            autocomplete="username"
          />
        </div>
        <div class="row mb-1">
          <label for="password" class="form-label col-3">Parole</label>
          <input
            id="password"
            v-model="password"
            class="form-control col"
            type="password"
            autocomplete="current-password"
          />
        </div>
        <div class="row text-center">
          <div class="col-3"></div>
          <div class="col"></div>
          <a href="#" class="recover link-primary ms-2 col">Atjaunot paroli</a>
        </div>
        <div class="row">
          <div class="col-3"></div>
          <button type="submit" class="btn btn-primary col">Ieiet</button>
          <a type="button" href="register" class="btn btn-primary ms-2 col"
            >Registrēties</a
          >
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
}
</style>
