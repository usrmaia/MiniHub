import { Box, Container } from "@mui/material";
import { AccountCircle, Delete, List, Settings } from "@mui/icons-material";

import { AuthWrapper } from "../auth-wrapper";
import { AppAppBar, ISideBarProps, SideBar } from "@/_components";

const buttonList: ISideBarProps[][] = [
  [
    { text: "Hub", to: "/hub", icon: <List /> },
    { text: "Recycle Bin", to: "/recycle-bin", icon: <Delete /> },
  ],
  [
    { text: "My Account", to: "/my-account", icon: <AccountCircle /> },
    { text: "Settings", to: "/settings", icon: <Settings /> }
  ],
];

const drawerWidth = 240;

export default function Hub({ children }: Readonly<{ children: React.ReactNode }>) {
  return (
    <AuthWrapper>
      <Box display="flex">
        <SideBar buttonList={buttonList} drawerWidth={drawerWidth} />
        <Box display="flex" flexDirection="column" sx={{ width: { xs: "100%", sm: `calc(100% - ${drawerWidth}px)` }, ml: { sm: `${drawerWidth}px` } }}>
          <AppAppBar />
          <Container maxWidth="xl" sx={{ my: 2 }}>
            {children}
          </Container>
        </Box>
      </Box>
    </AuthWrapper>
  );
}
