import { createApp } from 'vue'
import { createRouter, createWebHistory } from 'vue-router'
import App from './App.vue'
import './assets/tailwind.css'

// Define routes
const routes = [
  {
    path: '/',
    name: 'Home',
    component: () => import('./components/HomePage.vue')
  },
  {
    path: '/about',
    name: 'About',
    component: () => import('./components/AboutPage.vue')
  },
  {
    path: '/login',
    name: 'Login',
    component: () => import('./components/LoginPage.vue')
  }
]

// Create router instance
const router = createRouter({
  history: createWebHistory(),
  routes
})

// Create and mount the app
const app = createApp(App)
app.use(router)
app.mount('#app')
