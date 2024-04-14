import { createSlice } from "@reduxjs/toolkit";

interface initialStateProps {
  open: { [key: string]: boolean }
  status: "idle" | "loading" | "succeeded" | "failed";
  error: string | null;
}

const initialState: initialStateProps = {
  open: {},
  status: "idle",
  error: null,
};

const handleModalSlice = createSlice({
  name: "handleModal",
  initialState,
  reducers: {
    handleModalOpen: (state, action: { payload: string }) => {
      state.open[action.payload] = true;
    },
    handleModalClose: (state, action: { payload: string }) => {
      state.open[action.payload] = false;
    },
  },
  selectors: {
    selectOpen: state => state.open,
    selectStatus: state => state.status,
    selectError: state => state.error,
  }
});

export const { handleModalClose, handleModalOpen } = handleModalSlice.actions;
export const selectOpen = handleModalSlice.selectors.selectOpen;
export const selectStatus = handleModalSlice.selectors.selectStatus;
export const selectError = handleModalSlice.selectors.selectError;

export default handleModalSlice.reducer;