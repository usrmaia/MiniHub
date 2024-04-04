import type { Metadata } from "next";
import { Inter } from "next/font/google";

import ServerProviders from "./server-providers";
import ClientProviders from "./client-providers";

const inter = Inter({ subsets: ["latin"] });

export const metadata: Metadata = {
  title: "Mini Hub",
  description: "O sistema de gerenciamento de arquivos para empresas e projetos. Com ele, é possível criar, editar, compartilhar e excluir arquivos, além de gerenciar usuários e permissões. Tudo isso na rede interna da sua empresa ou projeto.",
};

export default function RootLayout({ children }: Readonly<{ children: React.ReactNode }>) {
  return (
    <html lang="pt-BR">
      <ServerProviders>
        <ClientProviders>
          <body className={inter.className}>
            {children}
          </body>
        </ClientProviders>
      </ServerProviders>
    </html>
  );
}
