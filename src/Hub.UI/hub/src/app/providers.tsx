import { Provider } from "react-redux";

import { AppThemeProvider } from "@/_theme";
import { store } from "@/_redux/store";

export default function Providers({ children }: Readonly<{ children: React.ReactNode }>) {
  return (
    <AppThemeProvider>
      <Provider store={store}>
        {children}
      </Provider>
    </AppThemeProvider>
  );
}
