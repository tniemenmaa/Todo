<template>
    <div id="app">
          <Tasks/>
    </div>
</template>

<script>
import Tasks from './components/Tasks.vue'

export default {
  name: 'App',
  components: {
      Tasks,
  }
}
</script>

<style>
@font-face {
  font-family: 'Nunito';
  src: url('./assets/fonts/Nunito-Regular.ttf') format("truetype");
}
#app {
  font-family: Nunito, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 10px;
}
</style>
