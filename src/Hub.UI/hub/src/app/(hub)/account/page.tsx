'use client'

import { Add } from "@mui/icons-material";
import { Button, Chip, FormControl, Grid, InputLabel, ListItemIcon, MenuItem, Select, TextField, Typography } from "@mui/material";
import { useEffect } from "react";
import { useForm } from "react-hook-form";
import { useDispatch, useSelector } from "react-redux";

import env from "@/env";
import { selectUser } from "@/_redux/features/auth/slice";
import { selectRoles } from "@/_redux/features/role/slice";
import { getAllRoles } from "@/_redux/features/role/thunks";
import { updateUser } from "@/_redux/features/user/thunks";
import { user } from "@/_types";

export default function Account() {
  const currentUser = useSelector(selectUser);
  const roles = useSelector(selectRoles);
  const dispatch = useDispatch();
  const { register, handleSubmit, watch, formState: { errors }, setValue, getValues } = useForm<user>({
    defaultValues: currentUser!,
  });

  useEffect(() => {
    dispatch(getAllRoles());
  }, []);

  if (env.NODE_ENV !== 'production')
    console.debug(watch())

  const onSubmit = (data: user) => {
    dispatch(updateUser({ oldUser: currentUser!, newUser: data }));
  };

  const handleRoleAdd = (role: string) => {
    if (getValues().roles.includes(role)) return;

    setValue("roles", [...getValues().roles, role]);
  };

  const handleRoleDelete = (role: string) => {
    setValue("roles", getValues().roles.filter(r => r !== role));
  };

  const RolesControlItems = () => (
    <FormControl
      disabled={!currentUser?.roles.includes("Administrador") && !currentUser?.roles.includes("Supervisor") && !currentUser?.roles.includes("Desenvolvedor")}
      sx={{ width: "100%" }}
    >
      <InputLabel id="input-modulo">Add Roles</InputLabel>
      <Select>
        <MenuItem value="" disabled>Add Roles</MenuItem>
        {roles && roles.map(role =>
          <MenuItem key={role.id} value={role.name} onClick={() => handleRoleAdd(role.name)}>
            <ListItemIcon sx={{ color: 'inherit' }}>
              <Add fontSize="small" />
            </ListItemIcon>
            {role.name}
          </MenuItem>
        )}
      </Select>
    </FormControl>
  );

  const UserChips = () => (
    <>
      {getValues() && getValues().roles && getValues().roles.map(role =>
        <Chip
          key={role}
          label={role}
          color="primary"
          onDelete={() => handleRoleDelete(role)}
          sx={{ m: 0.5 }}
        />
      )}
    </>
  );

  const Form = () => (
    <Grid container component="form" onSubmit={handleSubmit(onSubmit)} spacing={2}>
      <Grid item xs={12} md={4}>
        <TextField
          fullWidth
          label="Username"
          {...register('userName')}
        />
      </Grid>
      <Grid item xs={12} md={4}>
        <TextField
          fullWidth
          label="Email"
          {...register('email')}
        />
      </Grid>
      <Grid item xs={12} md={4}>
        <TextField
          fullWidth
          label="Phone"
          {...register('phoneNumber')}
        />
      </Grid>
      <Grid item xs={12} md={4}>
        <RolesControlItems />
      </Grid>
      <Grid item xs={12} md={8} alignContent='center' alignItems='center' wrap="nowrap" gap={1}>
        <UserChips />
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
      <Typography variant="h5" mb={2}>Account</Typography>
      <Form />
    </>
  );
}