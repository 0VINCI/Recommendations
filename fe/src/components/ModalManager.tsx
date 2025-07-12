import { useApp } from "../context/useApp";
import { ChangePasswordModal } from "./ChangePasswordModal";
import { RemindPasswordModal } from "./RemindPasswordModal";

export function ModalManager() {
  const { state, dispatch } = useApp();

  const closeChangePasswordModal = () => {
    dispatch({ type: "CLOSE_CHANGE_PASSWORD_MODAL" });
  };

  const closeRemindPasswordModal = () => {
    dispatch({ type: "CLOSE_REMIND_PASSWORD_MODAL" });
  };

  return (
    <>
      <ChangePasswordModal
        isOpen={state.isChangePasswordModalOpen}
        onClose={closeChangePasswordModal}
      />
      <RemindPasswordModal
        isOpen={state.isRemindPasswordModalOpen}
        onClose={closeRemindPasswordModal}
      />
    </>
  );
}
