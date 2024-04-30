"use client";

import { MRT_ColumnFiltersState, MRT_PaginationState, MRT_ShowHideColumnsButton, MRT_TableInstance, MRT_TableState, MRT_ToggleDensePaddingButton, MRT_ToggleFiltersButton, MRT_ToggleFullScreenButton, MRT_ToggleGlobalFilterButton, type MRT_ColumnDef, type MRT_RowData, type MRT_SortingState, type MRT_TableOptions } from "material-react-table";
import { Dispatch, SetStateAction } from "react";
import { useDispatch } from "react-redux";

import { useDefaultMaterialReactTable } from "./defaultMaterialReactTable";
import { AppDispatch } from "@/_redux/store";
import { Box, Button, IconButton } from "@mui/material";
import Link from "next/link";
import { Add, ClearAll, Delete, Edit, FileDownload, Share } from "@mui/icons-material";

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

    renderTopToolbarCustomActions: () => <></>,
    renderToolbarInternalActions: renderToolbarInternalActions,

    ...props,
  });
};