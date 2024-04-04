import { ToastContainer as ToastContainerRT } from 'react-toastify';

export const ToastContainer = () => {
  return (
    <ToastContainerRT
      position="bottom-right"
      autoClose={5000}
      hideProgressBar={false}
      newestOnTop={false}
      closeOnClick
      rtl={false}
      pauseOnFocusLoss
      draggable
      pauseOnHover
      theme="light"
    />
  );
};