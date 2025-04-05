import * as bootstrap from "bootstrap";

import "./assets/main.scss";
import "./utils/format";

import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";

import { library } from "@fortawesome/fontawesome-svg-core";

import { fas } from "@fortawesome/free-solid-svg-icons";

library.add(fas);

const app = createApp(App);

app.use(router);

app.provide("bootstrap", bootstrap);
app.mount("#app");
