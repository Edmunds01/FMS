 
import { createRouter, createWebHistory, type RouteRecordRaw } from "vue-router";

const routes: RouteRecordRaw[] = [
  {
    path: "/",
    name: "default",
    component: () => import("@/views/DashboardView.vue"),
  },
  {
    path: "/dashboard",
    name: "dashboard",
    component: () => import("@/views/DashboardView.vue"),
  },
  {
    path: "/login",
    name: "login",
    component: () => import("@/views/auth/LoginView.vue"),
  },
  {
    path: "/register",
    name: "register",
    component: () => import("@/views/auth/RegisterView.vue"),
  }
];

for (const route of routes) {
  if (route.name !== "login" && route.name !== "register") {
    route.meta = {
      requiresAuth: true,
    };
  }
}

const router = createRouter({
  history: createWebHistory(),
  routes: routes,
});

router.beforeEach((to, from, next) => {
  const token = localStorage.getItem("token");
  console.log("to", to);
  console.log("from", from);

  if(!(import.meta.env.REQUIRED_AUTH)) {
    next();
    return;
  }

  if (to.meta.requiresAuth && !token) {
    next("/login");
  } else {
    next();
  }
});

export default router;
