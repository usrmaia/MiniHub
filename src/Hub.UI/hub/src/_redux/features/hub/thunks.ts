import { createAsyncThunk } from "@reduxjs/toolkit";

import { Axios } from "@/_http/axios";
import { items, itemsFilter } from "@/_types";

const HUB = "/Hub";

export const getItems = createAsyncThunk(
  "hub/getItems",
  async (filter?: itemsFilter): Promise<items> => {
    const res = await Axios.get<items>(HUB, { params: filter });
    return res.data as items;
  }
);