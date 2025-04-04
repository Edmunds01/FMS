 
import { createRouter, createWebHistory, type RouteRecordRaw } from "vue-router";

const routes: RouteRecordRaw[] = [
  {
    path: "/dashboard",
    name: "dashboard",
    component: import("../views/DashboardView.vue"),
  },
  {
    path: "/dashboard2",
    name: "dashboard2",
    component: import("@/views/DashboardView.vue"),
  },
  {
    path: "/login2",
    name: "login2",
    component: import("../views/auth/LoginView.vue"),
  },
  {
    path: "/login",
    name: "login",
    component: () => import("../views/auth/LoginView.vue"),
  },
  {
    path: "/register",
    name: "register",
    component: () => import("../views/auth/RegisterView.vue"),
  },
  {
    path: "/da",
    name: "da",
    component: () => import("../views/auth/DashboardView.vue"),
  },
  {
    path: "/das",
    name: "das",
    component: () => import("@/views/auth/DashboardView.vue"),
  },
];

// for (const route of routes) {
//   if (route.name !== "login" && route.name !== "register") {
//     route.meta = {
//       requiresAuth: true,
//     };
//   }
// }

const router = createRouter({
  history: createWebHistory(),
  routes: routes,
});

// router.beforeEach((to, from, next) => {
//   const token = localStorage.getItem("token") ?? "some-token-value";
//   console.log("to", to);
//   console.log("from", from);
  
//   next();
// });

export default router;
