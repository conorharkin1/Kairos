import { createApp } from 'vue'
import App from './App.vue'
import router from './router'

// Icon library
import { Icon } from '@iconify/vue'

// Tooltip library
import FloatingVue from 'floating-vue'
import 'floating-vue/dist/style.css'

// Toast notification library
import Toast from "vue-toastification";

const app = createApp(App)

const toastOptions = {
    transition: "Vue-Toastification__fade",
    maxToasts: 10,
    newestOnTop: true,
    timeout: 2000,
    draggable: false,
    pauseOnHover: false,
    closeOnClick: true,
    hideProgressBar: true,
}

app.use(router)
// Icons
app.component('Icon', Icon)
// Toast notifications
app.use(Toast, toastOptions);
// Tooltips
app.use(FloatingVue)

app.mount('#app')
