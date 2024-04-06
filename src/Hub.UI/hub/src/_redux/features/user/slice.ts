import { createSlice } from "@reduxjs/toolkit";

import { updatePassword, updateUser } from "./thunks";
import { user } from "@/_types"
import { getUser, setUser } from "@/_services";

interface initialStateProps {
  user: user | null;
  status: 'idle' | 'loading' | 'succeeded' | 'failed';
  error: string | null;
}

const initialState: initialStateProps = {
  user: null,
  status: 'idle',
  error: null,
};

const userSlice = createSlice({
  name: "user",
  initialState,
  reducers: {
  },
  extraReducers: builder => {
    builder.addCase(updateUser.pending, (state) => {
      state.status = 'loading';
    })
    builder.addCase(updateUser.fulfilled, (state, action) => {
      state.user = action.payload;

      if (action.payload.id == getUser().id)
        setUser(action.payload);

      state.status = 'succeeded';
    })
    builder.addCase(updateUser.rejected, (state, action) => {
      state.status = 'failed';
      state.error = action.error.message || null;
    })
    builder.addCase(updatePassword.pending, (state) => {
      state.status = 'loading';
    })
    builder.addCase(updatePassword.fulfilled, (state, action) => {
      state.user = action.payload;

      state.status = 'succeeded';
    })
    builder.addCase(updatePassword.rejected, (state, action) => {
      state.status = 'failed';
      state.error = action.error.message || null;
    })
  },
  selectors: {
    selectUser: state => state.user,
    selectStatus: state => state.status,
    selectError: state => state.error,
  }
});

export const { } = userSlice.actions;
export const { selectError, selectStatus, selectUser } = userSlice.selectors;

export default userSlice.reducer;