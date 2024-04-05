'use client'

import { useRouter } from 'next/navigation'
import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';

import { logoutUser, selectAuthToken, selectStatus, selectUser } from '@/_redux/features/auth/slice';
import { Loading } from '@/_components';

export const AuthWrapper = ({ children }: Readonly<{ children: React.ReactNode }>) => {
  const dispatch = useDispatch();
  const { push } = useRouter();
  const user = useSelector(selectUser);
  const token = useSelector(selectAuthToken);
  const status = useSelector(selectStatus);

  useEffect(() => {
    // TODO: check if token is expired and token-userName mismatch
    if (token?.accessToken && user?.userName)
      return;

    push('/signin');
    dispatch(logoutUser());
  }, [token, user]);

  return status === 'loading' ? <Loading /> : children;
};
