'use client'

import { Avatar, Box, IconButton } from '@mui/material';
import { MenuOpen } from '@mui/icons-material';
import Image from 'next/image';
import React from 'react';

import { useDispatch } from 'react-redux';
import { handleSideBarClose as Close } from '@/_redux/features/handleSideBar/slice';

export const Header = () => {
  const dispatch = useDispatch();

  const Logo = () => (
    <>
      <Avatar src="/logo.png" variant="square" sx={{ width: 32, height: 32 }} />
      <Image src="/logo-rust.svg" alt="Logo" width={90} height={90} style={{ height: 'auto' }} />
    </>
  );

  const CloseButton = () => (
    <IconButton
      color='inherit'
      onClick={() => dispatch(Close())}
      sx={{ display: { xs: 'flex', sm: 'none' }, ml: 'auto' }}
    >
      <MenuOpen />
    </IconButton>
  );

  return (
    <Box
      display="flex"
      flexDirection='row'
      alignItems='center'
      gap={1} height={64} p={2}
    >
      <Logo />
      <CloseButton />
    </Box>
  );
}