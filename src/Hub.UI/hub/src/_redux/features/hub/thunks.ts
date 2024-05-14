import { createAsyncThunk } from "@reduxjs/toolkit";

import { Axios } from "@/_http/axios";
import { file, items, itemsFilter } from "@/_types";

const HUB = "/Hub";
const HUB_DOWNLOAD = "/Storage/download";

export const getItems = createAsyncThunk(
  "hub/getItems",
  async (filter?: itemsFilter): Promise<items> => {
    const res = await Axios.get<items>(HUB, { params: filter });
    return res.data as items;
  }
);

export const downloadFile = createAsyncThunk(
  "hub/downloadFile",
  async (file: file): Promise<void> => {
    const res = await Axios.get(HUB_DOWNLOAD + `/${file.id}`, { responseType: "blob" });
    const url = window.URL.createObjectURL(new Blob([res.data]));

    const link = document.createElement('a');
    link.href = url;
    link.setAttribute('download', `${file.name}`);
    document.body.appendChild(link);
    link.click();

    window.URL.revokeObjectURL(url);
  }
);

