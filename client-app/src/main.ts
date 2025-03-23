import * as bootstrap from 'bootstrap'

import './assets/main.css'
import './utils/format'

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'

import { library } from '@fortawesome/fontawesome-svg-core'

import FaIcon from './components/global/fa-icon.vue'

import { faXmark, faCalendar } from '@fortawesome/free-solid-svg-icons'

library.add(faXmark, faCalendar)

const app = createApp(App)

app.component('faIcon', FaIcon)

app.use(router)

app.provide('bootstrap', bootstrap)
app.mount('#app')
