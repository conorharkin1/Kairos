import { createApp } from 'vue'
import App from './App.vue'
import router from './router'

// Icon library
import { Icon } from '@iconify/vue'

// Tooltip library
import FloatingVue from 'floating-vue'
import 'floating-vue/dist/style.css'

const app = createApp(App)

app.use(router)
app.component('Icon', Icon)
app.use(FloatingVue)

app.mount('#app')
