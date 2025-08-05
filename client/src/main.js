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
  },
  {
    path: '/history',
    name: 'History',
    component: () => import('./components/History.vue')
  },
  {
    path: '/:shortCode',
    name: 'Redirect',
    beforeEnter: (to, from, next) => {
      // Check if this looks like a short URL (6 character alphanumeric)
      const shortUrlPattern = /^[a-zA-Z0-9]{6}$/
      if (shortUrlPattern.test(to.params.shortCode)) {
        // Use the same logic as HomePage - redirect to backend
        const backendUrl = `http://localhost:8000/${to.params.shortCode}`
        window.location.href = backendUrl
      } else {
        // If not a short URL, continue to normal routing
        next()
      }
    }
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
