import { createVNode, render } from "vue";
import ConfirmDialog from "@/components/global/ConfirmAction.vue";

// eslint-disable-next-line @typescript-eslint/no-explicit-any
let instance: any = null;

export function useConfirm(): (message: string, reopenModalId: string) => Promise<boolean> {
  if (!instance) {
    const container = document.createElement("div");
    document.body.appendChild(container);

    const vnode = createVNode(ConfirmDialog);
    render(vnode, container);

    instance = vnode.component?.proxy;
  }

  return (message: string, reopenModalId: string) => {
    return instance.open(message, reopenModalId);
  };
}
