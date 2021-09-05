import Vue from 'vue'
import App from './App.vue'

/* Bootstrap-Vue */
import { BootstrapVue, IconsPlugin } from 'bootstrap-vue'
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'
Vue.use(BootstrapVue)
Vue.use(IconsPlugin)

/* Vue Draggable */
import VueDraggable from 'vuedraggable'
Vue.use(VueDraggable)

Vue.config.productionTip = false

new Vue({
  render: h => h(App),
}).$mount('#app')
