'use client'

import { Box, Button, Checkbox, FormControlLabel, Link, TextField, Typography } from '@mui/material';
import { SubmitHandler, useForm } from 'react-hook-form';
import { useDispatch } from 'react-redux';
import { useRouter } from 'next/navigation';

import { loginUser } from '@/_redux/features/auth/thunks';
import { AppDispatch } from '@/_redux/store';

type Inputs = {
  username: string
  password: string
}

export default function SingIn() {
  const { push } = useRouter();
  const dispatch = useDispatch<AppDispatch>();

  const { register, handleSubmit } = useForm<Inputs>();

  const onSubmit: SubmitHandler<Inputs> = data => {
    dispatch(loginUser({ username: data.username, password: data.password }));
    push('/hub');
  }

  const Links = () => (
    <Box display="flex" justifyContent="space-between">
      <Link href="identity" variant="body2">
        Forgot password?
      </Link>
      <Link href="signup" variant="body2">
        Don't have an account? Sign Up
      </Link>
    </Box>
  );

  return (
    <Box component="form" onSubmit={handleSubmit(onSubmit)} width='-webkit-fill-available'>
      <Typography variant="h5" align='center'>
        Sign in
      </Typography>
      <TextField
        margin='normal'
        fullWidth
        label="Username"
        autoFocus
        {...register("username", { required: true })}
        autoComplete="username"
      />
      <TextField
        margin='normal'
        fullWidth
        label="Password"
        type="password"
        {...register("password", { required: true })}
        autoComplete="current-password"
      />
      <FormControlLabel
        control={<Checkbox value="remember" />}
        label="Remember me"
      />
      <Button type="submit" fullWidth variant="contained" sx={{ my: 2 }}>
        Sign In
      </Button>
      <Links />
    </Box>
  );
}