'use client'

import { useContext, useEffect } from 'react';
import { Avatar as AvatarMUI, IconButton } from '@mui/material';
import { useSelector } from 'react-redux';

import { selectUser } from '@/_redux/features/auth/slice';
import { colors, ThemeContext } from '@/_theme';

export const Avatar = () => {
  const user = useSelector(selectUser);

  useEffect(() => {
    console.log('Avatar', user);
  }, [user]);

  const { themeName } = useContext(ThemeContext);

  return (
    <IconButton color='inherit'>
      <AvatarMUI sx={{ width: 24, height: 24, bgcolor: `${themeName === 'light' ? colors.black : colors.white}` }}>
        {user?.userName ? user.userName[0].toUpperCase() : ''}
      </AvatarMUI>
    </IconButton >
  );
}
