import type { IconName } from "@/components/global/fa-icon.vue";

export type Category = {
  name: string;
  type: "expense" | "income";
  icon: IconName;
  sum: number;
  id: string;
};

export const categories: Category[] = [
  {
    name: "Food",
    type: "expense",
    icon: "wand-magic",
    sum: 2123.1233,
    id: "1",
  },
  {
    name: "Transport",
    type: "expense",
    icon: "envelope",
    sum: 213.12,
    id: "2",
  },
  {
    name: "Entertainment",
    type: "expense",
    icon: "cart-shopping",
    sum: 0,
    id: "3",
  },
  {
    name: "Salary",
    type: "income",
    icon: "tree",
    sum: 231.12,
    id: "4",
  },
  {
    name: "Investment",
    type: "income",
    icon: "gear",
    sum: 912.12,
    id: "5",
  },
  {
    name: "Other",
    type: "income",
    icon: "calendar",
    sum: 0,
    id: "6",
  },
  {
    name: "Savings",
    type: "expense",
    icon: "paperclip",
    sum: 0,
    id: "7",
  },
  {
    name: "Health",
    type: "expense",
    icon: "cart-shopping",
    sum: 142.12,
    id: "8",
  },
  {
    name: "Education",
    type: "expense",
    icon: "calendar",
    sum: 912.17,
    id: "9",
  },
];
