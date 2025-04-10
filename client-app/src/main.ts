import * as bootstrap from "bootstrap";

import "./assets/main.scss";
import "./utils/format";

import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";

import Notifications from "@kyvg/vue3-notification";

import VueApexCharts from "vue3-apexcharts";

import { library } from "@fortawesome/fontawesome-svg-core";

import { fas } from "@fortawesome/free-solid-svg-icons";

library.add(fas);

const app = createApp(App);

app.use(router);
app.use(Notifications);
app.use(VueApexCharts);

app.provide("bootstrap", bootstrap);
app.mount("#app");
