import { createApp } from 'vue'
import './style.css'
import App from './App.vue'
import router from "./Router";
import { pinia } from './Stores/index.ts'

createApp(App)
    .use(pinia)
    .use(router)
    .mount('#app')
