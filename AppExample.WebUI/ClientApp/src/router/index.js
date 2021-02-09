import Vue from 'vue'
import VueRouter from 'vue-router'
import Home from '../views/Home.vue'
import cart from '../views/cart.vue'
import feedback from '../views/feedback.vue'
import simpleLayout from "../layouts/single-card";
import login from "../views/login-form";
import register from "../views/create-account-form";

import defaultLayout from "../components/static/SideNavOuterToolbar";

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'Home',
    components: {
      layout: defaultLayout,
      content: Home
    }
  },
  {
    path: '/cart',
    name: 'cart',
    components: {
      layout: defaultLayout,
      content: cart
    }
  },
  {
    path: '/feedback',
    name: 'feedback',
    components: {
      layout: defaultLayout,
      content: feedback
    }
  },
  {
    path: "/login-form",
    name: "login-form",
    components: {
      layout: simpleLayout,
      content: login

    },
    props: {
      layout: {
        title: "Войти"
      }
    }
  },
  {
    path: "/create-account",
    name: "create-account",
    meta: { requiresAuth: false },
    components: {
      layout: simpleLayout,
      content: register
    },
    props: {
      layout: {
        title: "Регистрация"
      }
    }
  },
  {
    path: "*",
    redirect: "/"
  },
]

const router = new VueRouter({
  routes
})

export default router
