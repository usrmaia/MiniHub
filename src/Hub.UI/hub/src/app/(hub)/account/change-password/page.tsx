'use client'

import { Button, Grid, TextField, Typography } from "@mui/material";
import { useForm } from "react-hook-form";
import { useDispatch } from "react-redux";

import env from "@/env";
import { updatePassword } from "@/_redux/features/user/thunks";
import { AppDispatch } from "@/_redux/store";
import { updatedPassword } from "@/_types";

export default function ChangePassword() {
  const dispatch = useDispatch<AppDispatch>();
  const { register, handleSubmit, watch, formState: { errors } } = useForm<updatedPassword & { confirmPassword: string }>({
    defaultValues: {
      oldPassword: '',
      newPassword: '',
      confirmPassword: '',
    }
  });

  if (env.NODE_ENV !== 'production')
    console.debug(watch())

  const onSubmit = (data: updatedPassword & { confirmPassword: string }) => {
    dispatch(updatePassword({ oldPassword: data.oldPassword, newPassword: data.newPassword, }));
  };

  const Form = () => (
    <Grid container component="form" onSubmit={handleSubmit(onSubmit)} spacing={2}>
      <Grid item xs={12} md={4}>
        <TextField
          fullWidth
          label="Old Password"
          type="password"
          {...register('oldPassword', { required: true })}
        />
      </Grid>
      <Grid item xs={12} md={4}>
        <TextField
          fullWidth
          label="New Password"
          type="password"
          {...register('newPassword', {
            required: true,
            validate: (value) =>
              value !== watch('oldPassword', '') || 'New password must be different from the old password'
          })}
          error={!!errors.newPassword}
          helperText={errors.newPassword?.message}
        />
      </Grid>
      <Grid item xs={12} md={4}>
        <TextField
          fullWidth
          label="Confirm New Password"
          type="password"
          {...register("confirmPassword", {
            required: true,
            validate: (value) =>
              value === watch("newPassword", "") || "Passwords do not match"
          })}
          error={!!errors.confirmPassword}
          helperText={errors.confirmPassword?.message}
        />
      </Grid>
      <Grid item xs={12}>
        <Button
          type="submit"
          variant="contained"
          color="primary"
          fullWidth
        >
          Update
        </Button>
      </Grid>
    </Grid>
  );

  return (
    <>
      <Typography variant="h5" mb={2}>Change Password</Typography>
      <Form />
    </>
  );
}