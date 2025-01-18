import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router'
import HomeView from '../views/HomeView.vue'

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    name: 'home',
    component: HomeView,
  },
  {
    path: '/about',
    name: 'about',
    component: () => import('../views/AboutView.vue'),
  },
  {
    path: '/login',
    name: 'login',
    component: () => import('../views/auth/LoginView.vue'),
  },
  {
    path: '/register',
    name: 'register',
    component: () => import('../views/auth/RegisterView.vue'),
  },
]

for (const route of routes) {
  if (route.name !== 'login' && route.name !== 'register') {
    route.meta = {
      requiresAuth: true,
    }
  }
}

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: routes,
})

router.beforeEach((to, from, next) => {
  const token = localStorage.getItem('token')
  console.log('to', to)
  console.log('from', from)
  if (to.meta.requiresAuth && !token) {
    next('/login')
  } else {
    next()
  }
})

export default router
