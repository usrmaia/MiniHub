"use client";

import { Provider } from "react-redux";

import { store } from "@/_redux/store";
import { SnackbarProvider } from "@/_contexts";

export default function Providers({ children }: Readonly<{ children: React.ReactNode }>) {
  return (
    <>
      <SnackbarProvider>
        <Provider store={store}>
          {children}
        </Provider>
      </SnackbarProvider>
    </>
  );
}
