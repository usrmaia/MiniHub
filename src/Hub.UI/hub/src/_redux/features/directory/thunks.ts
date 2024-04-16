import { createAsyncThunk } from "@reduxjs/toolkit";

import { Axios } from "@/_http/axios";
import { directory, queryResult, directoryFilter } from "@/_types";

const DIRECTORY = "/Directory";

export const getDirectories = createAsyncThunk(
  "directory/getDirectories",
  async (filter?: directoryFilter): Promise<queryResult<directory>> => {
    const res = await Axios.get<queryResult<directory>>(DIRECTORY, { params: filter });
    return res.data as queryResult<directory>;
  }
);