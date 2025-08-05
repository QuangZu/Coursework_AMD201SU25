<template>
  <div class="min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100 flex items-center justify-center py-12 px-4 sm:px-6 lg:px-8">
    <div class="max-w-md w-full space-y-8">
      <div>
        <h2 class="mt-6 text-center text-3xl font-extrabold text-gray-900">
          Sign in to your account
        </h2>
        <p class="mt-2 text-center text-sm text-gray-600">
          Or
          <a href="#" @click="showRegister = !showRegister" class="font-medium text-indigo-600 hover:text-indigo-500">
            {{ showRegister ? 'sign in to existing account' : 'create a new account' }}
          </a>
        </p>
      </div>
      
      <form class="mt-8 space-y-6" @submit.prevent="handleSubmit">
        <div class="rounded-md shadow-sm -space-y-px">
          <div>
            <label for="username" class="sr-only">Username</label>
            <input
              id="username"
              v-model="form.username"
              name="username"
              type="text"
              required
              class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-t-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
              placeholder="Username"
            />
          </div>
          <div v-if="showRegister">
            <label for="email" class="sr-only">Email address</label>
            <input
              id="email"
              v-model="form.email"
              name="email"
              type="email"
              required
              class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
              placeholder="Email address"
            />
          </div>
          <div>
            <label for="password" class="sr-only">Password</label>
            <input
              id="password"
              v-model="form.password"
              name="password"
              type="password"
              required
              class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-b-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
              placeholder="Password"
            />
          </div>
        </div>

        <div v-if="error" class="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded">
          {{ error }}
        </div>

        <div v-if="success" class="bg-green-50 border border-green-200 text-green-700 px-4 py-3 rounded">
          {{ success }}
        </div>

        <div>
          <button
            type="submit"
            :disabled="isLoading"
            class="group relative w-full flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 disabled:opacity-50"
          >
            <span v-if="isLoading" class="absolute left-0 inset-y-0 flex items-center pl-3">
              <svg class="animate-spin h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
              </svg>
            </span>
            {{ showRegister ? 'Register' : 'Sign in' }}
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script>
import { authAPI } from '../utils/api'

export default {
  name: 'LoginPage',
  data() {
    return {
      showRegister: false,
      isLoading: false,
      error: '',
      success: '',
      form: {
        username: '',
        email: '',
        password: ''
      }
    }
  },
  methods: {
    async handleSubmit() {
      this.isLoading = true
      this.error = ''
      this.success = ''

      try {
        let response
        if (this.showRegister) {
          response = await authAPI.register(this.form)
        } else {
          response = await authAPI.login(this.form)
        }

        const data = response.data

        // Store token and user info
        localStorage.setItem('token', data.token)
        localStorage.setItem('user', JSON.stringify(data.user))
        
        this.success = this.showRegister ? 'Registration successful! Redirecting...' : 'Login successful! Redirecting...'
        
        // Emit login event to parent components
        this.$emit('user-logged-in', data.user)
        
        // Redirect to home page after a short delay
        setTimeout(() => {
          this.$router.push('/')
        }, 1500)

      } catch (error) {
        this.error = error.response?.data?.error || error.message || 'An error occurred'
      } finally {
        this.isLoading = false
      }
    }
  }
}
</script>
