<script setup lang="ts">
import { ref } from 'vue';
import { login } from '@/api/auth';


const username = ref('');
const password = ref('');

const handleLogin = async () => {
  try {
    console.log(username.value, password.value);
    const token = await login({ username: username.value, password: password.value });
    localStorage.setItem('token', token);
    alert('Login successful!');
  } catch (error) {
    console.error(error);
    alert('Login failed.');
  }
};

</script>


<template>
  <div>
    <h1>Login</h1>
    <form @submit.prevent="handleLogin">
      <div>
        <label for="username">Username</label>
        <input id="username" v-model="username" type="text" />
      </div>
      <div>
        <label for="password">Password</label>
        <input id="password" v-model="password" type="password" />
      </div>
      <button type="submit">Login</button>
    </form>
  </div>
</template>