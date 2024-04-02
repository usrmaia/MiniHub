'use client'

import { useContext } from 'react';
import { AppBar, Avatar, Container, Grid, IconButton, Toolbar } from '@mui/material';
import { NotificationAdd } from '@mui/icons-material';
import Image from 'next/image';

import { ToggleThemeIcon } from './toggleThemeIcon';
import { Search } from './search';
import { colors, ThemeContext } from '@/_theme';

export const AppAppBar = () => {
  const { themeName } = useContext(ThemeContext);

  return (
    <AppBar position="sticky" color='transparent' elevation={0} sx={{ bgcolor: `${themeName === 'light' ? colors.white : colors.black}` }}>
      <Toolbar>
        <Container disableGutters maxWidth='xl'>
          <Grid container display='flex'>
            <Grid item xs={5}>
              <Search />
            </Grid>
            <Grid item xs={4} />
            <Grid item xs={3} display='flex' justifyContent='flex-end' alignContent='center'>
              <IconButton color='inherit'>
                <Image src="/icons/languages/english.png" alt="English" width={48} height={48} style={{ width: '24px', height: '24px' }} />
              </IconButton>
              <IconButton color='inherit'>
                <NotificationAdd />
              </IconButton>
              <ToggleThemeIcon />
              <IconButton color='inherit'>
                <Avatar sx={{ width: 24, height: 24 }}>G</Avatar>
              </IconButton>
            </ Grid>
          </Grid>
        </Container>
      </Toolbar>
    </AppBar>
  );
}