import type { IconName } from "@/components/global/fa-icon.vue";

export type Account = {
  name: string;
  balance: number;
  icon: IconName;
};

export const accounts: Account[] = [
  {
    name: "KontName",
    balance: 100.1,
    icon: "money-bills",
  },
  {
    name: "KontName2",
    balance: 200.14,
    icon: "sack-dollar",
  },
  {
    name: "KontName3 long name",
    balance: 300.15,
    icon: "money-bill",
  },
  {
    name: "KontName4",
    balance: 400.16,
    icon: "credit-card",
  },
  {
    name: "KontName5",
    balance: 500.17,
    icon: "calendar",
  },
  {
    name: "KontName6",
    balance: 600.18,
    icon: "xmark",
  },
  {
    name: "KontName7",
    balance: 700.19,
    icon: "wand-magic",
  },
  {
    name: "KontName8",
    balance: 800.2,
    icon: "envelope",
  },
  {
    name: "KontName9 really really long name for testing purpo",
    // name: "KontName9 really",
    balance: 900.21,
    icon: "tree",
  },
];
