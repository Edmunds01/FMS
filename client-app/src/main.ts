import * as bootstrap from 'bootstrap'

import './assets/main.css'

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'

import { library } from '@fortawesome/fontawesome-svg-core'

import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'

import { faXmark } from '@fortawesome/free-solid-svg-icons'

library.add(faXmark)

const app = createApp(App)

app.component('font-awesome-icon', FontAwesomeIcon)

app.use(router)

app.provide('bootstrap', bootstrap)
app.mount('#app')
