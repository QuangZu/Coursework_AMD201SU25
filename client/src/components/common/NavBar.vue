<template>
  <nav class="bg-white shadow-lg">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
      <div class="flex justify-between items-center h-16">
        <!-- Logo -->
        <div class="flex-shrink-0">
          <RouterLink to="/" class="flex items-center">
            <img src="../../assets/link_icon.png" alt="Logo" class="h-8 w-auto" />
            <span class="ml-2 text-xl font-bold text-gray-800">ShortLink</span>
          </RouterLink>
        </div>

        <!-- Navigation Links -->
        <div class="hidden md:block">
          <div class="ml-10 flex items-baseline space-x-4">
            <RouterLink
              to="/"
              class="text-gray-700 hover:text-blue-600 px-3 py-2 rounded-md text-sm font-medium transition-colors duration-200"
              active-class="text-blue-600 bg-blue-50"
            >
              Home
            </RouterLink>
            <RouterLink
              to="/about"
              class="text-gray-700 hover:text-blue-600 px-3 py-2 rounded-md text-sm font-medium transition-colors duration-200"
              active-class="text-blue-600 bg-blue-50"
            >
              About
            </RouterLink>
            <RouterLink
              to="/history"
              class="text-gray-700 hover:text-blue-600 px-3 py-2 rounded-md text-sm font-medium transition-colors duration-200"
              active-class="text-blue-600 bg-blue-50"
              v-if="user"
            >
              History
            </RouterLink>
          </div>
        </div>

        <!-- User Menu / Login -->
        <div class="hidden md:block">
          <!-- Logged In User -->
          <div v-if="user" class="flex items-center space-x-4">
            <div class="flex items-center space-x-2">
              <div class="w-8 h-8 bg-blue-600 rounded-full flex items-center justify-center">
                <span class="text-white text-sm font-medium">{{ userInitials }}</span>
              </div>
              <span class="text-gray-700 text-sm font-medium">{{ user.username }}</span>
            </div>
            <button
              @click="logout"
              class="text-gray-500 hover:text-red-600 px-3 py-2 rounded-md text-sm font-medium transition-colors duration-200"
            >
              Logout
            </button>
          </div>
          
          <!-- Not Logged In -->
          <RouterLink
            v-else
            to="/login"
            class="bg-blue-600 text-white hover:bg-blue-700 px-4 py-2 rounded-md text-sm font-medium transition-colors duration-200"
          >
            Login/Register
          </RouterLink>
        </div>

        <!-- Mobile menu button -->
        <div class="md:hidden">
          <button
            @click="mobileMenuOpen = !mobileMenuOpen"
            class="text-gray-700 hover:text-blue-600 focus:outline-none focus:text-blue-600"
          >
            <svg class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16" />
            </svg>
          </button>
        </div>
      </div>

      <!-- Mobile menu -->
      <div v-show="mobileMenuOpen" class="md:hidden">
        <div class="px-2 pt-2 pb-3 space-y-1 sm:px-3">
          <RouterLink
            to="/"
            class="text-gray-700 hover:text-blue-600 block px-3 py-2 rounded-md text-base font-medium"
            active-class="text-blue-600 bg-blue-50"
            @click="mobileMenuOpen = false"
          >
            Home
          </RouterLink>
          <RouterLink
            to="/about"
            class="text-gray-700 hover:text-blue-600 block px-3 py-2 rounded-md text-base font-medium"
            active-class="text-blue-600 bg-blue-50"
            @click="mobileMenuOpen = false"
          >
            About
          </RouterLink>
          
          <!-- Mobile User Menu -->
          <div v-if="user" class="border-t border-gray-200 pt-4 mt-4">
            <div class="flex items-center space-x-3 px-3 py-2">
              <div class="w-8 h-8 bg-blue-600 rounded-full flex items-center justify-center">
                <span class="text-white text-sm font-medium">{{ userInitials }}</span>
              </div>
              <span class="text-gray-700 text-base font-medium">{{ user.username }}</span>
            </div>
            <button
              @click="logout"
              class="text-red-600 hover:text-red-700 block px-3 py-2 rounded-md text-base font-medium w-full text-left"
            >
              Logout
            </button>
          </div>
          
          <!-- Mobile Login -->
          <RouterLink
            v-else
            to="/login"
            class="bg-blue-600 text-white hover:bg-blue-700 block px-3 py-2 rounded-md text-base font-medium"
            @click="mobileMenuOpen = false"
          >
            Login/Register
          </RouterLink>
        </div>
      </div>
    </div>
  </nav>
</template>

<script>
export default {
  name: 'NavBar',
  props: {
    user: {
      type: Object,
      default: null
    }
  },
  data() {
    return {
      mobileMenuOpen: false
    }
  },
  computed: {
    userInitials() {
      if (!this.user || !this.user.username) return '?'
      return this.user.username
        .split(' ')
        .map(name => name.charAt(0))
        .join('')
        .toUpperCase()
        .substring(0, 2)
    }
  },
  methods: {
    logout() {
      localStorage.removeItem('user')
      localStorage.removeItem('token')
      this.$emit('user-logged-out')
      this.mobileMenuOpen = false
      
      // Redirect to home page
      if (this.$route.path !== '/') {
        this.$router.push('/')
      }
    }
  }
}
</script>