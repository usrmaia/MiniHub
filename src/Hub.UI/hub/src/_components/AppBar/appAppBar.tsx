'use client'

import { useContext, useEffect } from 'react';
import { AppBar, Container, Grid, IconButton, Toolbar } from '@mui/material';
import { NotificationAdd } from '@mui/icons-material';
import Image from 'next/image';
import { useSelector } from 'react-redux';

import { Avatar } from './avatar';
import { ToggleThemeIcon } from './toggleThemeIcon';
import { Search } from './search';
import { selectUser } from '@/_redux/features/auth/slice';
import { colors, ThemeContext } from '@/_theme';

export const AppAppBar = () => {
  let user = useSelector(selectUser);
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
              <Avatar />
            </ Grid>
          </Grid>
        </Container>
      </Toolbar>
    </AppBar>
  );
}