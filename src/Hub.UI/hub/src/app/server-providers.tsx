import { AppThemeProvider } from "@/_theme";

export default function Providers({ children }: Readonly<{ children: React.ReactNode }>) {
  return (
    <AppThemeProvider>
      {children}
    </AppThemeProvider>
  );
}
