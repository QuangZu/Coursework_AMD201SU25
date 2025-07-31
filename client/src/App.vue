<template>
  <div id="app">
    <NavBar :user="currentUser" @user-logged-in="handleUserLogin" @user-logged-out="handleUserLogout" />
    <router-view @user-logged-in="handleUserLogin" />
  </div>
</template>

<script>
import NavBar from './components/common/NavBar.vue'

export default {
  name: 'App',
  components: {
    NavBar
  },
  data() {
    return {
      currentUser: null
    }
  },
  mounted() {
    this.loadUser()
  },
  methods: {
    loadUser() {
      const userStr = localStorage.getItem('user')
      if (userStr) {
        try {
          this.currentUser = JSON.parse(userStr)
        } catch (error) {
          console.error('Error parsing user data:', error)
          localStorage.removeItem('user')
          localStorage.removeItem('token')
        }
      }
    },
    handleUserLogin(user) {
      this.currentUser = user
    },
    handleUserLogout() {
      this.currentUser = null
    }
  }
}
</script>
