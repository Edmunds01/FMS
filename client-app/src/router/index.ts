import { api } from "@/api/auto-generated-client";
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

router.beforeEach(async (to, from, next) => {
  console.log("to", to);
  console.log("from", from);

  if((import.meta.env.NOT_REQUIRED_AUTH)) {
    next();
    return;
  }

  if (to.meta.requiresAuth) {
    try {
      await api.validateSession()
      next();
    }
    catch {
      next("/login");
    }
  } else {
    next();
  }
});

export default router;
