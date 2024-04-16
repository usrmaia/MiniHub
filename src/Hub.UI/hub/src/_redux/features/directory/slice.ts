import { createSlice } from "@reduxjs/toolkit";

import { getDirectories } from "./thunks";
import { directory, directoryFilter } from "@/_types";

interface initialStateProps {
  directories: directory[] | null;
  totalCount: number | null;
  status: "idle" | "loading" | "succeeded" | "failed";
  error: string | null;
}

const initialState: initialStateProps = {
  directories: null,
  totalCount: null,
  status: "idle",
  error: null,
};

const directorySlice = createSlice({
  name: "directory",
  initialState,
  reducers: {
  },
  extraReducers: builder => {
    builder.addCase(getDirectories.pending, (state) => {
      state.status = "loading";
    });
    builder.addCase(getDirectories.fulfilled, (state, action) => {
      state.directories = action.payload.items;
      state.totalCount = action.payload.totalCount;
      state.status = "succeeded";
    });
    builder.addCase(getDirectories.rejected, (state, action) => {
      state.status = "failed";
      state.error = action.error.message || null;
    });
  },
  selectors: {
    selectDirectories: (state) => state.directories,
    selectTotalCount: (state) => state.totalCount,
    selectStatus: (state) => state.status,
    selectError: (state) => state.error,
  }
});

export const { } = directorySlice.actions;
export const { selectDirectories, selectError, selectStatus, selectTotalCount } = directorySlice.selectors;

export default directorySlice.reducer;