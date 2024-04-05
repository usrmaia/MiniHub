import { createAsyncThunk } from "@reduxjs/toolkit";

import { Axios } from "@/_http/axios";
import { user, userRole } from "@/_types";

const PUT_USER = "/User";
const POST_USER_ROLE = "/User/add-to-role";
const DELETE_USER_ROLE = "/User/remove-from-role";

export const updateUser = createAsyncThunk(
  "user/updateUser",
  async ({ oldUser, newUser }: { oldUser: user, newUser: user }): Promise<user> => {
    if (oldUser.userName !== newUser.userName || oldUser.email !== newUser.email || oldUser.password !== newUser.password) {
      await Axios.put<user>(PUT_USER, newUser);
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
)