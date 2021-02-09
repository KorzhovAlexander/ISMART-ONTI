import Vue from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import axios from 'axios';
import appInfo from './app-info'

import 'devextreme/dist/css/dx.common.css';
import './themes/generated/theme.base.css';

import moment from 'moment';

Vue.prototype.$appInfo = appInfo;
Vue.prototype.$moment = moment;
axios.defaults.withCredentials = true

Vue.config.productionTip = false

/**
 * Русификация DevExtreme компонентов.
 */
import {locale, loadMessages} from 'devextreme/localization';
import ruMessages from 'devextreme/localization/messages/ru.json';
import config from 'devextreme/core/config';

loadMessages(ruMessages);
locale('ru');


/**
 * Глобальная настройка DevExtreme компонентов.
 */
config({
    forceIsoDateParsing: true
});

window.Vue = Vue;

new Vue({
    router,
    store,
    render: h => h(App)
}).$mount('#app')
