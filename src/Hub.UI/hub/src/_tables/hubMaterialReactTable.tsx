"use client";

import { MRT_ColumnFiltersState, MRT_PaginationState, MRT_ShowHideColumnsButton, MRT_TableInstance, MRT_TableState, MRT_ToggleDensePaddingButton, MRT_ToggleFiltersButton, MRT_ToggleFullScreenButton, MRT_ToggleGlobalFilterButton, type MRT_ColumnDef, type MRT_RowData, type MRT_SortingState, type MRT_TableOptions } from "material-react-table";
import { Dispatch, SetStateAction } from "react";
import { useDispatch } from "react-redux";

import { useDefaultMaterialReactTable } from "./defaultMaterialReactTable";
import { AppDispatch } from "@/_redux/store";
import { Box, Button, FormControl, IconButton, InputLabel, ListItemIcon, MenuItem, Select } from "@mui/material";
import Link from "next/link";
import { Add, ClearAll, Delete, Edit, Face, FileDownload, Share } from "@mui/icons-material";
import { Loading } from "@/_components";

interface Props<TData extends MRT_RowData> extends MRT_TableOptions<TData> {
  columns: MRT_ColumnDef<TData>[];
  data: TData[];
  setGlobalFilter?: Dispatch<SetStateAction<string>>;
  setColumnFilters?: Dispatch<SetStateAction<MRT_ColumnFiltersState>>;
  setSorting?: Dispatch<SetStateAction<MRT_SortingState>>;
  setPagination?: Dispatch<SetStateAction<MRT_PaginationState>>;
  rowCount?: number;
  initialSatate?: Partial<MRT_TableState<TData>>;
  state?: Partial<MRT_TableState<TData>>;
  onSubmit?: () => void;
  isLoading?: () => boolean;
  title: string;
  toUpload?: string;
  handleDelete?: (id: string) => void;
}

export const useHubMaterialReactTable = <TData extends MRT_RowData>(
  { columns, data, ...props }: Props<TData>
) => {
  const dispatch = useDispatch<AppDispatch>();

  const renderTopToolbarCustomActions = ({ table }: { table: MRT_TableInstance<TData> }) => (
    <Box display="flex" gap={1}>
      <FormControl key={"input-type"} size="small" sx={{ minWidth: 150 }}>
        <InputLabel id="input-type">Type</InputLabel>
        <Select>
          <MenuItem value="docs">Docs</MenuItem>
          <MenuItem value="sheets">Sheets</MenuItem>
          <MenuItem value="slides">Slides</MenuItem>
          <MenuItem value="forms">Forms</MenuItem>
          <MenuItem value="images">Images</MenuItem>
          <MenuItem value="pdfs">PDFs</MenuItem>
          <MenuItem value="videos">Videos</MenuItem>
          <MenuItem value="folders">Folders</MenuItem>
          <MenuItem value="audios">Audios</MenuItem>
          <MenuItem value="zips">Zips</MenuItem>
          <MenuItem value="others">Others</MenuItem>
        </Select>
      </FormControl>
      <FormControl key={"input-peoples"} size="small" sx={{ minWidth: 200 }}>
        <InputLabel id="input-peoples">Peoples</InputLabel>
        <Select>
          <MenuItem value="people-1">
            <ListItemIcon sx={{ color: "inherit" }}>
              <Face />
            </ListItemIcon>
            {"People 1"}
          </MenuItem>
        </Select>
      </FormControl>
    </Box>
  );

  const renderToolbarInternalActions = ({ table }: { table: MRT_TableInstance<TData> }) => [
    <MRT_ToggleGlobalFilterButton key="globalFilter" table={table} />,
    // <IconButton key="share" size="medium" aria-label="teste" onClick={handleShare}><Share /></IconButton>,
    // <IconButton key="clear-filters" size="medium" onClick={handleClearFilters}><ClearAll /></IconButton>,
    <MRT_ShowHideColumnsButton key="showHideColumns" table={table} />,
    <MRT_ToggleDensePaddingButton key="toggleDensePadding" table={table} />,
  ];

  return useDefaultMaterialReactTable({
    columns,
    data,

    enablePagination: false,

    renderTopToolbarCustomActions: renderTopToolbarCustomActions,
    renderToolbarInternalActions: renderToolbarInternalActions,

    initialState: {
      showColumnFilters: false,
    },

    ...props,
  });
};