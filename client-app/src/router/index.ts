import { api } from "@/api/auto-generated-client";
import { createRouter, createWebHistory, type RouteRecordRaw } from "vue-router";

const routes: RouteRecordRaw[] = [
  {
    path: "/",
    name: "default",
    meta: {
      name: "dashboard",
      requiresAuth: true,
    },
    component: () => import("@/views/DashboardView.vue"),
  },
  {
    path: "/dashboard",
    name: "dashboard",
    meta: {
      name: "dashboard",
      requiresAuth: true,
    },
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
  },
  {
    path: "/logout",
    name: "logout",
    component: () => import("@/views/auth/LogoutView.vue"),
  },
  {
    path: "/recover",
    name: "recover",
    component: () => import("@/views/auth/RecoverStepOne.vue"),
  },
  {
    path: "/change-password",
    name: "change-password",
    component: () => import("@/views/auth/RecoverStepTwo.vue"),
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes: routes,
});

router.beforeEach(async (to, from, next) => {
  if (
    (from.name == "login" && to.meta.name == "dashboard") ||
    (from.name == "register" && to.meta.name == "dashboard")
  ) {
    next();
    return;
  }

  if (!to.meta.requiresAuth) {
    next();
    return;
  }

  try {
    const isSessionValid = await api.validateSession();
    if (isSessionValid) {
      next();
      return;
    }

    next("/login");
    // TODO: Add "Service Unavailable" page
  } catch {
    next("/login");
  }
});

export default router;
