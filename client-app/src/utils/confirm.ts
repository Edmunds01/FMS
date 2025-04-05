import { createVNode, render } from "vue";
import ConfirmDialog from "@/components/global/confirm-action.vue";

// eslint-disable-next-line @typescript-eslint/no-explicit-any
let instance: any = null;

export function useConfirm(): (
  title: string,
  message: string,
  reopenModalId: string,
  doNotReopenOnSuccessRaw: boolean,
) => Promise<boolean> {
  if (!instance) {
    const container = document.createElement("div");
    document.body.appendChild(container);

    const vnode = createVNode(ConfirmDialog);
    render(vnode, container);

    instance = vnode.component?.proxy;
  }

  return (
    title: string,
    message: string,
    reopenModalId: string,
    doNotReopenOnSuccessRaw: boolean = false,
  ) => {
    return instance.open(title, message, reopenModalId, doNotReopenOnSuccessRaw);
  };
}
