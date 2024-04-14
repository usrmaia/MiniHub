import { createAsyncThunk } from "@reduxjs/toolkit";

import { Axios } from "@/_http/axios";
import { queryResult, updatedPassword, user, userFilter, userRole } from "@/_types";

const USER = "/User";
const PUT_USER_PASSWORD = "/User/password";
const POST_USER_ROLE = "/User/add-to-role";
const DELETE_USER_ROLE = "/User/remove-from-role";

export const getUsers = createAsyncThunk(
  "user/getUsers",
  async (filter?: userFilter): Promise<queryResult<user>> => {
    const res = await Axios.get<queryResult<user>>(USER, { params: filter });
    return res.data as queryResult<user>;
  }
);

export const postUser = createAsyncThunk(
  "user/postUser",
  async (user: user): Promise<user> => {
    const res = await Axios.post<user>(USER, user);

    user.roles.filter(r => r !== "Colaborador").forEach(async role => {
      await Axios.post<user>(POST_USER_ROLE, { userId: res.data.id, roleName: role } as userRole);
    });

    res.data.roles = user.roles;
    return res.data as user;
  }
);

export const updateUser = createAsyncThunk(
  "user/updateUser",
  async ({ oldUser, newUser }: { oldUser: user, newUser: user }): Promise<user> => {
    if (oldUser.userName !== newUser.userName || oldUser.email !== newUser.email || oldUser.password !== newUser.password) {
      await Axios.put<user>(USER, newUser);
    }
    if (oldUser.roles !== newUser.roles) {
      const rolesToAdd = newUser.roles.filter(r => !oldUser.roles.includes(r));
      const rolesToDelete = oldUser.roles.filter(r => !newUser.roles.includes(r));

      for (const role of rolesToAdd) {
        await Axios.post<user>(POST_USER_ROLE, { userId: newUser.id, roleName: role } as userRole);
      }

      for (const role of rolesToDelete) {
        await Axios.delete<user>(DELETE_USER_ROLE, { data: { userId: newUser.id, roleName: role } as userRole });
      }
    }

    return newUser;
  }
);

export const updatePassword = createAsyncThunk(
  "user/updatePassword",
  async (password: updatedPassword): Promise<user> => {
    const res = await Axios.put<user>(PUT_USER_PASSWORD, password);
    return res.data as user;
  }
);

export const deleteUser = createAsyncThunk(
  "user/deleteUser",
  async (id: string): Promise<user> => {
    const res = await Axios.delete<user>(`${USER}/${id}`);
    return res.data as user;
  }
);