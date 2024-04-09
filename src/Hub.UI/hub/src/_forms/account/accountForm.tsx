"use client";

import { Add } from "@mui/icons-material";
import { Button, Chip, FormControl, Grid, InputLabel, ListItemIcon, MenuItem, Select, TextField } from "@mui/material";
import { useEffect } from "react";
import { useForm } from "react-hook-form";
import { useDispatch, useSelector } from "react-redux";

import env from "@/env";
import { selectUser } from "@/_redux/features/auth/slice";
import { selectRoles } from "@/_redux/features/role/slice";
import { getAllRoles } from "@/_redux/features/role/thunks";
import { postUser, updateUser } from "@/_redux/features/user/thunks";
import { AppDispatch } from "@/_redux/store";
import { user } from "@/_types";
import { includes } from "@/_utils";

export const AccountForm = ({ user }: { user?: user }) => {
  const currentUser = useSelector(selectUser);
  const allRoles = useSelector(selectRoles);
  const dispatch = useDispatch<AppDispatch>();
  const { register, handleSubmit, watch, setValue, getValues, formState: { errors } } = useForm<user & { confirmPassword: string }>({
    defaultValues: { ...user, roles: user?.roles || ["Colaborador"] }
  });

  useEffect(() => { dispatch(getAllRoles()); }, []);

  if (env.NODE_ENV !== "production")
    console.debug(watch());

  const onSubmit = (data: user) =>
    !user ? dispatch(postUser(data)) :
      dispatch(updateUser({ oldUser: user, newUser: data }));

  const handleRoleAdd = (role: string) => {
    if (getValues().roles.includes(role)) return;
    setValue("roles", [...getValues().roles, role]);
  };

  const handleRoleDelete = (role: string) =>
    setValue("roles", getValues().roles.filter(r => r !== role));

  const RolesControlItems = () => (
    <FormControl
      disabled={!includes(currentUser ? currentUser.roles : [], ["Desenvolvedor", "Administrador", "Supervisor"])}
      sx={{ width: "100%" }}
    >
      <InputLabel id="input-modulo">Add Roles</InputLabel>
      <Select>
        <MenuItem value="" disabled>Add Roles</MenuItem>
        {allRoles && allRoles.map(role =>
          <MenuItem key={role.id} value={role.name} onClick={() => handleRoleAdd(role.name)}>
            <ListItemIcon sx={{ color: "inherit" }}>
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

  return (
    <Grid container component="form" onSubmit={handleSubmit(onSubmit)} spacing={2}>
      <Grid item xs={12} md={4}>
        <TextField
          fullWidth
          label="Username"
          {...register("userName", { required: true })}
        />
      </Grid>
      <Grid item xs={12} md={4}>
        <TextField
          fullWidth
          label="Email"
          {...register("email", { required: true })}
        />
      </Grid>
      <Grid item xs={12} md={4}>
        <TextField
          fullWidth
          label="Phone"
          {...register("phoneNumber", { required: true })}
        />
      </Grid>
      {!user && (
        <>
          <Grid item xs={12} md={4}>
            <TextField
              fullWidth
              label="Password"
              type="password"
              {...register("password", { required: true })}
            />
          </Grid>
          <Grid item xs={12} md={4}>
            <TextField
              fullWidth
              label="Confirm Password"
              type="password"
              {...register("confirmPassword", {
                required: true,
                validate: value =>
                  value === watch("password", "") || "The passwords do not match.",
              })}
              error={!!errors.confirmPassword}
              helperText={errors.confirmPassword?.message}
            />
          </Grid>
          <Grid item xs={12} md={4} />
        </>
      )}
      <Grid item xs={12} md={4}>
        <RolesControlItems />
      </Grid>
      <Grid item xs={12} md={8} alignContent="center" alignItems="center" gap={1}>
        <UserChips />
      </Grid>
      <Grid item xs={12}>
        <Button
          type="submit"
          variant="contained"
          color="primary"
          fullWidth
        >
          {user ? "Update" : "Create"}
        </Button>
      </Grid>
    </Grid>
  );
};