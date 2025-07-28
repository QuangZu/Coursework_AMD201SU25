<template>
  <div id="app">
    <NavBar />
    <router-view />
  </div>
</template>

<script>
import NavBar from './components/common/NavBar.vue'

export default {
  name: 'App',
  components: {
    NavBar
  }
}
</script>
