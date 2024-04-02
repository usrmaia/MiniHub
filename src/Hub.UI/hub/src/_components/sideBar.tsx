import { Avatar, Box, Button, Divider, Drawer, List, ListItem, ListItemButton, ListItemIcon, ListItemText, Typography } from '@mui/material';
import Image from 'next/image';
import Link from 'next/link';

export interface ISideBarProps {
  text: string;
  to: string;
  icon: React.ReactNode;
}

export const SideBar = ({ buttonList, drawerWidth = 240 }: Readonly<{ buttonList: ISideBarProps[][], drawerWidth: number | undefined }>) => {
  const Logo = () => (
    <Box display="flex" flexDirection='row' justifyContent='left' alignItems='center' gap={1} height={64} p={2}>
      <Avatar src="/logo.png" variant="square" sx={{ width: 32, height: 32 }} />
      <Image src="/logo-rust.svg" alt="Logo" width={90} height={90} style={{ height: 'auto' }} />
    </Box>
  );

  const ButtonList = () => (
    <List>
      {buttonList.map(subList => (
        <>
          {subList.map(({ text, to, icon }) => (
            <ListItem key={text} disablePadding sx={{ display: 'block' }}>
              <Link href={to} style={{ color: 'inherit', textDecoration: 'none' }}>
                <ListItemButton>
                  <ListItemIcon sx={{ color: 'inherit' }}>{icon}</ListItemIcon>
                  <ListItemText primary={text} sx={{ opacity: 1 }} />
                </ListItemButton>
              </Link>
            </ListItem>
          ))}
          <Divider />
        </>
      ))}
    </List>
  );

  const UpgradeToPro = () => (
    <Box display="flex" flexDirection='column' justifyContent='center' alignItems='center' width='100%' height='100%' p={2}>
      <Image src="/upgrade-to-pro-v2.svg" alt="Upgrade to Pro" width={200} height={200} style={{ height: 'auto' }} />
      <Typography variant="h6" m={0} p={0}>
        Get More?
      </Typography>
      <Typography variant="subtitle1">
        From only $4.99/month
      </Typography>
      <Button variant="contained" color="primary" sx={{ mt: 1 }}>
        Upgrade to PRO
      </Button>
    </Box>
  );

  return (
    <Drawer variant="permanent" sx={{ '& .MuiDrawer-paper': { width: drawerWidth, borderRight: 'none' } }}>
      <Logo />
      <ButtonList />
      <UpgradeToPro />
    </Drawer>
  );
}