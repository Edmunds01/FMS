<script setup lang="ts">
import { api } from "@/api/auto-generated-client";
import ModalWindow, { userProfileModalId } from "@/components/global/ModalWindow.vue";
import { inject, onMounted, ref, watch } from "vue";
import { validatePassword } from "@/components/auth/TheRegister.vue";
import { useNotification } from "@kyvg/vue3-notification";
import { profileKey } from "@/utils/keys";
import FaIcon from "@/components/global/FaIcon.vue";

const notification = useNotification();
const email = ref("");
const { close } = inject(profileKey)!;

const showOldPassword = ref(false);
const showPassword = ref(false);
const showConfirmPassword = ref(false);

const oldPassword = ref("");
const newPassword = ref("");
const confirmPassword = ref("");

const oldPasswordError = ref("");
const newPasswordError = ref("");
const confirmPasswordError = ref("");

const validateConfirmPassword = () => {
  if (confirmPassword.value !== newPassword.value) {
    confirmPasswordError.value = "Paroles nesakrīt.";
  } else {
    confirmPasswordError.value = "";
  }
};

const validateForm = () => {
  oldPasswordError.value = oldPassword.value ? "" : "Tekošā parole ir obligāta.";
  newPasswordError.value = validatePassword(newPassword.value);
  validateConfirmPassword();
  return !oldPasswordError.value && !newPasswordError.value && !confirmPasswordError.value;
};

watch(
  () => newPassword.value,
  () => {
    if (newPasswordError.value) {
      newPasswordError.value = validatePassword(newPassword.value);
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

const handleSave = async () => {
  if (validateForm()) {
    try {
      await api.changePassword(oldPassword.value, newPassword.value);
      notification.notify({
        title: "Parole",
        text: "Parole veiksmīgi mainīta.",
        duration: 4000,
        type: "success",
      });
      close();
    } catch {
      notification.notify({
        title: "Kļūda",
        text: "Parole nav pareiza.",
        duration: 4000,
        type: "error",
      });
    }
  }
};

onMounted(async () => {
  email.value = await api.userEmail();
});
</script>

<template>
  <ModalWindow :id="userProfileModalId">
    <template #header>
      <div class="row p-0 m-0 w-100">
        <h1 class="col-11 p-0 m-0 text-start">Profils</h1>
        <button class="col-1 p-0 m-0" @click="close">
          <FaIcon icon="fa-solid fa-xmark" size="2xl" />
        </button>
      </div>
    </template>
    <template #body>
      <div class="row p-0 m-0 mb-4 mt-2">
        <div class="col-3 text-start ms-2">E-pasts</div>
        <div class="col-7 p-0">
          <input type="text" class="form-control" :placeholder="email" disabled />
        </div>
      </div>
      <div class="row p-0 m-0 border-bottom">
        <h1 class="col text-start ms-2">Paroles maiņa</h1>
      </div>
      <div class="row p-0 m-0 mb-4 mt-2">
        <div class="col-3 text-start ms-2">Tekoša parole</div>
        <div class="input-group col-7 p-0">
          <input
            v-model="oldPassword"
            :type="showOldPassword ? 'text' : 'password'"
            autocomplete="current-password"
            class="form-control"
          />
          <button
            type="button"
            class="btn btn-outline-secondary"
            @click="showOldPassword = !showOldPassword"
          >
            {{ showOldPassword ? "Slēpt" : "Rādīt" }}
          </button>
        </div>
        <p v-if="oldPasswordError" class="text-danger">{{ oldPasswordError }}</p>
      </div>
      <div class="row p-0 m-0 mb-4 mt-2">
        <div class="col-3 text-start ms-2">Jauna parole</div>
        <div class="input-group col-7 p-0">
          <input
            v-model="newPassword"
            :type="showPassword ? 'text' : 'password'"
            autocomplete="new-password"
            class="form-control"
          />
          <button
            type="button"
            class="btn btn-outline-secondary"
            @click="showPassword = !showPassword"
          >
            {{ showPassword ? "Slēpt" : "Rādīt" }}
          </button>
        </div>
        <p v-if="newPasswordError" class="text-danger">{{ newPasswordError }}</p>
      </div>
      <div class="row p-0 m-0 mb-4 mt-2">
        <div class="col-3 text-start ms-2">Atkārtojiet paroli</div>
        <div class="input-group col-7 p-0">
          <input
            v-model="confirmPassword"
            :type="showConfirmPassword ? 'text' : 'password'"
            autocomplete="new-password"
            class="form-control"
          />
          <button
            type="button"
            class="btn btn-outline-secondary"
            @click="showConfirmPassword = !showConfirmPassword"
          >
            {{ showConfirmPassword ? "Slēpt" : "Rādīt" }}
          </button>
        </div>
        <p v-if="confirmPasswordError" class="text-danger">{{ confirmPasswordError }}</p>
      </div>
      <div class="row p-0 m-0 mb-4 mt-2">
        <div class="col-3 ms-2"></div>
        <button class="col-3 btn btn-primary" @click="handleSave">Saglabāt</button>
      </div>
    </template>
  </ModalWindow>
</template>

<style scoped lang="scss">
.input-group {
  width: 58.33%;
}
.text-danger {
  color: red;
  font-size: 0.8rem;
}
</style>
