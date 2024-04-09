import { AuthWrapper } from "@app/auth-wrapper";

export default function Hub({ children }: Readonly<{ children: React.ReactNode }>) {
  return (
    <AuthWrapper authorizedRoles={["Desenvolvedor", "Administrador", "Supervisor"]}>
      {children}
    </AuthWrapper>
  );
}
