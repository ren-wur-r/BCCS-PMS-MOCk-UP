import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import { createPinia } from "pinia";
import piniaPluginPersistedstate from "pinia-plugin-persistedstate";
import { createVuetify } from "vuetify";
import * as components from "vuetify/components";
import * as directives from "vuetify/directives";
import "@/styles/main.css";
import '@vuepic/vue-datepicker/dist/main.css'


const pinia = createPinia();
pinia.use(piniaPluginPersistedstate);
const vuetify = createVuetify({
  components,
  directives,
});

const app = createApp(App);
app.use(router);
app.use(pinia);
app.use(vuetify);
app.mount("#app");
