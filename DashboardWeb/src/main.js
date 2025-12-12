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
import 'vue-select/dist/vue-select.css';

// Vue select
import vSelect from 'vue-select'

const app = createApp(App)

const toastOptions = {
    transition: "Vue-Toastification__fade",
    maxToasts: 10,
    newestOnTop: true,
    timeout: 3500,
    draggable: false,
    pauseOnHover: false,
    closeOnClick: true,
    hideProgressBar: true,
}

app.use(router)

// Icons
app.component('Icon', Icon)

// Tooltips
app.use(FloatingVue)

// Toast notifications
app.use(Toast, toastOptions);

// Vue select
app.component('v-select', vSelect)

app.mount('#app')
