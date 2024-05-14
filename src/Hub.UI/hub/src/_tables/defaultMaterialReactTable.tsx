"use client";

import { MaterialReactTable, MRT_ColumnFiltersState, MRT_PaginationState, MRT_Row, MRT_RowSelectionState, MRT_ShowHideColumnsButton, MRT_SortingState, MRT_TableInstance, MRT_TableState, MRT_ToggleDensePaddingButton, MRT_ToggleFiltersButton, MRT_ToggleFullScreenButton, MRT_ToggleGlobalFilterButton, MRT_VisibilityState, useMaterialReactTable, type MRT_ColumnDef, type MRT_RowData, type MRT_TableOptions, } from "material-react-table";
import { Add, ClearAll, Delete, Edit, FileDownload, Share } from "@mui/icons-material";
import { Box, Button, IconButton } from "@mui/material";
import Link from "next/link";
import { Dispatch, SetStateAction, useContext, useEffect, useState } from "react";

import env from "@/env";
import { useSnackbar } from "@/_contexts";
import { colors, ThemeContext } from "@/_theme";
import { DownloadExportDisplay } from "./components/DownloadExportDisplay/downloadExportDisplay";
import { useDispatch } from "react-redux";
import { handleModalOpen } from "@/_redux/features/handleModal/slice";

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
  enableRowSelection?: boolean;
  onSubmit?: () => void;
  isLoading?: () => boolean;
  title: string;
  toCreate?: string;
  toEdit?: string;
  handleDelete?: (id: string) => void;
}

export const useDefaultMaterialReactTable = <TData extends MRT_RowData>(
  { columns, data, ...props }: Props<TData>
) => {

  const params: URLSearchParams = typeof window === "undefined" ?
    new URLSearchParams() :
    new URLSearchParams(window.location.search);

  const [rowSelection, setRowSelection] = useState<MRT_RowSelectionState>({});

  const dispatch = useDispatch();

  // useEffect(() => { if (env.NODE_ENV !== "production") console.debug("rowSelection", rowSelection); }, [rowSelection]);

  const snackbar = useSnackbar();
  const { themeName } = useContext(ThemeContext);

  const handleShare = () => {
    const url = typeof window === "undefined" ? "" : `${window.location.href}`;

    navigator.clipboard.writeText(url)
      .then(() => snackbar("Link copied to clipboard"));

    if (env.NODE_ENV !== "production")
      console.debug("handleShare", url);
  };

  const handleClearFilters = () => {
    props.setGlobalFilter && props.setGlobalFilter("");
    props.setSorting && props.setSorting([]);
    props.setColumnFilters && props.setColumnFilters([]);
    props.setPagination && props.setPagination({ pageIndex: 0, pageSize: 10 });

    Array.from(params.keys()).forEach(key => params.delete(key));

    props.onSubmit && props.onSubmit();
  };

  const handleDownloadExportRows = () =>
    dispatch(handleModalOpen("download-export-display"));

  const handleDelete = () => {
    if (!Object.keys(rowSelection).length) {
      snackbar("Select a row to delete");
      return;
    }

    const id = Object.keys(rowSelection)[0].toString() ?? "";
    setRowSelection({});
    props.handleDelete && props.handleDelete(id);
    snackbar("Record deleted! Update the page to see the changes.");
  };

  const renderTopToolbarCustomActions = ({ table }: { table: MRT_TableInstance<TData> }) => (
    <Box display="flex" alignItems="center" gap={1}>
      {props.onSubmit && <Button key="run" variant="contained" size="small" onClick={props.onSubmit}>Run</Button>}
      {props.toCreate && <Link href={props.toCreate}><Button key="create" variant="outlined" size="small" endIcon={<Add />} onClick={() => console.debug("create")}>Create</Button></Link>}
      {props.toEdit && <Link href={`${props.toEdit}/${Object.keys(rowSelection)[0] ?? ""}`}><Button key="edit" variant="outlined" size="small" endIcon={<Edit />} onClick={() => console.debug("edit")}>Edit</Button></Link>}
      {props.handleDelete && <Button key="delete" variant="outlined" size="small" endIcon={<Delete />} onClick={handleDelete}>Delete</Button>}
    </Box>
  );

  const renderToolbarInternalActions = ({ table }: { table: MRT_TableInstance<TData> }) => [
    <MRT_ToggleGlobalFilterButton key="globalFilter" table={table} />,
    <IconButton key="share" size="medium" aria-label="teste" onClick={handleShare}><Share /></IconButton>,
    <IconButton key="clear-filters" size="medium" onClick={handleClearFilters}><ClearAll /></IconButton>,
    <IconButton key="export" size="medium" onClick={handleDownloadExportRows}><FileDownload /></IconButton>,
    <MRT_ToggleFiltersButton key="toggleFilters" table={table} />,
    <MRT_ShowHideColumnsButton key="showHideColumns" table={table} />,
    <MRT_ToggleDensePaddingButton key="toggleDensePadding" table={table} />,
    <MRT_ToggleFullScreenButton key="toggleFullScreen" table={table} />,
  ];

  const table = useMaterialReactTable({
    columns,
    data,
    ...props,

    renderTopToolbarCustomActions: props.renderTopToolbarCustomActions ?? renderTopToolbarCustomActions,
    renderToolbarInternalActions: props.renderToolbarInternalActions ?? renderToolbarInternalActions,

    //#region setStates

    onGlobalFilterChange: props.setGlobalFilter,

    manualFiltering: props.manualFiltering ?? true,
    onColumnFiltersChange: props.setColumnFilters,

    manualSorting: props.manualSorting ?? true,
    onSortingChange: props.setSorting,

    enableRowSelection: props.enableRowSelection ?? true,
    getRowId: row => row.id ?? "",
    onRowSelectionChange: setRowSelection,

    manualPagination: props.manualPagination ?? true,
    onPaginationChange: props.setPagination,

    initialState: {
      showColumnFilters: true,
      density: "compact",
      ...props.initialState,
    },

    state: {
      isLoading: props.isLoading ? props.isLoading() : false,
      rowSelection,
      ...props.state,
    },

    //#endregion

    layoutMode: "grid",
    enableColumnResizing: props.enableColumnResizing ?? true,
    positionToolbarAlertBanner: "none",

    rowCount: props.rowCount,

    //#region Styles

    muiTablePaperProps: ({ table }) => ({
      style: {
        zIndex: table.getState().isFullScreen ? 9999 : undefined,
      },
    }),

    muiTopToolbarProps: ({ table }) => ({
      sx: () => ({
        "& .MuiIconButton-root": {
          color: `${themeName === "light" ? colors.black : colors.white}`
        },
      })
    }),

    muiTableProps: ({ table }) => ({
      sx: () => ({
        "& .MuiIconButton-root, .MuiSelect-icon": {
          color: `${themeName === "light" ? colors.black : colors.white}`
        },
      })
    }),

    muiFilterSliderProps: ({ table }) => ({
      sx: () => ({
        "& .MuiIconButton-root, .MuiSelect-icon": {
          color: `${themeName === "light" ? colors.black : colors.white}`
        },
      })
    }),

    muiBottomToolbarProps: ({ table }) => ({
      sx: () => ({
        "& .MuiIconButton-root, .MuiSelect-icon": {
          color: `${themeName === "light" ? colors.black : colors.white}`
        },
      })
    }),

    muiPaginationProps: ({ table }) => ({
      rowsPerPageOptions: [5, 10, 15, 20, 25, 30, 50, 100, 200, 500],
    }),

    //#endregion

  });

  return (
    <>
      <DownloadExportDisplay
        fileName={props.title}
        head={table.getVisibleFlatColumns().map(c => c.columnDef).map(c => ({ id: c.id ?? "", value: c.header?.toString() ?? "" })).filter(c => c.id !== "mrt-row-select")}
        allRows={table.getRowModel().rows.map(r => r.original)}
        selectedRows={table.getSelectedRowModel().rows.map(r => r.original)}
      />
      <MaterialReactTable table={table} />
    </>
  );
};
