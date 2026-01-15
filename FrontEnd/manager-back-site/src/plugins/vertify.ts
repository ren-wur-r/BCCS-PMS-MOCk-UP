import 'vuetify/styles'
import { createVuetify } from 'vuetify'

// 只引真的會用到的元件與指令
import { VApp, VMain, VSwitch } from 'vuetify/components'
import { Ripple } from 'vuetify/directives'

export const vuetify = createVuetify({
  components: { VApp, VMain, VSwitch },
  directives: { Ripple },
})