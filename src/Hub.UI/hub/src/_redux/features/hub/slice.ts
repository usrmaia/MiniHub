import { createSlice } from "@reduxjs/toolkit";

import { downloadFile, getItems, uploadFile } from "./thunks";
import { directory, file, items } from "@/_types";

interface initialStateProps {
  items: items | null;
  directories: directory[] | null;
  files: file[] | null;
  totalCount: number | null;
  status: "idle" | "loading" | "succeeded" | "failed";
  error: string | null;
}

const initialState: initialStateProps = {
  items: null,
  directories: null,
  files: null,
  totalCount: null,
  status: "idle",
  error: null,
};

const hubSlice = createSlice({
  name: "hub",
  initialState,
  reducers: {
  },
  extraReducers: builder => {
    builder.addCase(downloadFile.pending, (state) => {
      state.status = "loading";
    });
    builder.addCase(downloadFile.fulfilled, (state) => {
      state.status = "succeeded";
    });
    builder.addCase(downloadFile.rejected, (state, action) => {
      state.status = "failed";
      state.error = action.error.message || null;
    });

    builder.addCase(getItems.pending, (state) => {
      state.status = "loading";
    });
    builder.addCase(getItems.fulfilled, (state, action) => {
      state.items = action.payload;
      state.directories = action.payload.directories;
      state.files = action.payload.files;
      state.totalCount = action.payload.totalCount;
      state.status = "succeeded";
    });
    builder.addCase(getItems.rejected, (state, action) => {
      state.status = "failed";
      state.error = action.error.message || null;
    });

    builder.addCase(uploadFile.pending, (state) => {
      state.status = "loading";
    });
    builder.addCase(uploadFile.fulfilled, (state) => {
      state.status = "succeeded";
    });
    builder.addCase(uploadFile.rejected, (state, action) => {
      state.status = "failed";
      state.error = action.error.message || null;
    });
  },
  selectors: {
    selectItems: state => state.items,
    selectDirectories: state => state.directories,
    selectFiles: state => state.files,
    selectTotalCount: state => state.totalCount,
    selectStatus: state => state.status,
    selectError: state => state.error,
  }
});

export const { } = hubSlice.actions;
export const { selectDirectories, selectError, selectFiles, selectItems, selectStatus, selectTotalCount } = hubSlice.selectors;

export default hubSlice.reducer;