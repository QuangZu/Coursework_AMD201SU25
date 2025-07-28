<template>
  <nav class="bg-white shadow-lg">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
      <div class="flex justify-between items-center h-16">
        <!-- Logo -->
        <div class="flex-shrink-0">
          <RouterLink to="/" class="flex items-center">
            <img src="../../assets/logo.png" alt="Logo" class="h-8 w-auto" />
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
          </div>
        </div>

        <!-- Login/Register -->
        <div class="hidden md:block">
          <RouterLink
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
          <RouterLink
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
  data() {
    return {
      mobileMenuOpen: false
    }
  }
}
</script>