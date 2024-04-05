import { createAsyncThunk } from "@reduxjs/toolkit";

import { Axios } from "@/_http/axios";
import { role } from "@/_types";

const QUERY_URL = "/Role";

export const getAllRoles = createAsyncThunk(
  "role/getAllRoles",
  async (): Promise<role[]> => {
    const res = await Axios.get<role[]>(QUERY_URL);
    return res.data as role[];
  }
)