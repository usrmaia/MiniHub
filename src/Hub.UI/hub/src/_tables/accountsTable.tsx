"use client";

import { jsPDF } from "jspdf";
import autoTable from "jspdf-autotable";
import { MaterialReactTable, MRT_Cell, MRT_ColumnFiltersState, MRT_PaginationState, MRT_Row, MRT_ShowHideColumnsButton, MRT_TableInstance, MRT_ToggleDensePaddingButton, MRT_ToggleFiltersButton, MRT_ToggleFullScreenButton, MRT_ToggleGlobalFilterButton, useMaterialReactTable, type MRT_ColumnDef, type MRT_SortingState } from "material-react-table";
import { Add, ClearAll, Delete, Edit, FileDownload, Share } from "@mui/icons-material";
import { Box, Button, IconButton } from "@mui/material";
import { useCallback, useContext, useEffect, useMemo, useState } from "react";
import { useDispatch, useSelector } from "react-redux";

import env from "@/env";
import { selectStatus, selectTotalCount, selectUsers } from "@/_redux/features/user/slice";
import { getUsers } from "@/_redux/features/user/thunks";
import { AppDispatch } from "@/_redux/store";
import { user, userFilter } from "@/_types";
import { useSnackbar } from "@/_contexts";
import { colors, ThemeContext } from "@/_theme";

export const AccountsTable = () => {
  const dispatch = useDispatch<AppDispatch>();

  const users = useSelector(selectUsers);
  const rowCount = useSelector(selectTotalCount);

  const status = useSelector(selectStatus);
  const isLoading = () => status === "idle" || status === "loading";

  const params: URLSearchParams = typeof window === "undefined" ?
    new URLSearchParams() :
    new URLSearchParams(window.location.search);

  const [globalFilter, setGlobalFilter] = useState<string>(params.get("search") ?? "");
  const [sorting, setSorting] = useState<MRT_SortingState>([]);
  const [columnFilters, setColumnFilters] = useState<MRT_ColumnFiltersState>([]);
  const [pagination, setPagination] = useState<MRT_PaginationState>({
    pageIndex: parseInt(params.get("pageIndex") as string) || 0,
    pageSize: parseInt(params.get("pageSize") as string) || 10
  });

  const snackbar = useSnackbar();
  const { themeName } = useContext(ThemeContext);

  useEffect(() => { onsubmit(); }, []);

  // useEffect(() => { if (env.NODE_ENV !== "production") console.debug("users", users); }), [users];
  // useEffect(() => { if (env.NODE_ENV !== "production") console.debug("globalFilter", globalFilter); }), [globalFilter];
  // useEffect(() => { if (env.NODE_ENV !== "production") console.debug("sorting", sorting); }), [sorting];
  // useEffect(() => { if (env.NODE_ENV !== "production") console.debug("columnFilters", columnFilters); }), [columnFilters];
  // useEffect(() => { if (env.NODE_ENV !== "production") console.debug("pagination", pagination); }), [pagination];
  // useEffect(() => { if (env.NODE_ENV !== "production") console.debug("rowCount", rowCount); }), [rowCount];

  const buildQueryString = useCallback(() => {
    Array.from(params.keys()).forEach(key => params.delete(key));

    globalFilter && params.set("search", globalFilter);
    sorting.forEach(sort => params.set(sort.id + "Sort", sort.desc ? "desc" : "asc"));
    columnFilters.forEach(filter => params.set(filter.id, filter.value as string));
    params.set("pageIndex", pagination.pageIndex.toString());
    params.set("pageSize", pagination.pageSize.toString());

    if (env.NODE_ENV !== "production")
      console.debug("buildQueryString", params.toString());

    return params.toString();
  }, [globalFilter, sorting, columnFilters, pagination]);

  const getQueryObject = (): userFilter => {
    const filter: userFilter = {
      search: params.get("search") ?? undefined,
      userName: params.get("userName") ?? undefined,
      email: params.get("email") ?? undefined,
      phoneNumber: params.get("phoneNumber") ?? undefined,
      role: params.get("roles") ?? undefined,
      pageIndex: parseInt(params.get("pageIndex") || "0"),
      pageSize: parseInt(params.get("pageSize") || "10"),
      userNameOrderSort: params.get("userNameSort") as "asc" | "desc" ?? undefined,
      emailOrderSort: params.get("emailSort") as "asc" | "desc" ?? undefined,
    };

    return filter;
  };

  const onsubmit = () => {
    buildQueryString();
    const filter = getQueryObject();

    if (env.NODE_ENV !== "production")
      console.debug("onsubmit", filter);

    dispatch(getUsers(filter));
  };

  useEffect(() => {
    const search = buildQueryString();
    const newUrl = `${window.location.pathname}?${search}`;
    window.history.replaceState({}, "", newUrl);
  }, [status]);

  const handleShare = () => {
    const url = typeof window === "undefined" ? "" : `${window.location.href}`;

    navigator.clipboard.writeText(url)
      .then(() => snackbar("Link copied to clipboard"));

    if (env.NODE_ENV !== "production")
      console.debug("handleShare", url);
  };

  const handleClearFilters = () => {
    setGlobalFilter("");
    setSorting([]);
    setColumnFilters([]);
    setPagination({ pageIndex: 0, pageSize: 10 });

    Array.from(params.keys()).forEach(key => params.delete(key));

    onsubmit();
  };

  const handleExportRows = (rows: MRT_Row<user>[]) => {
    const doc = new jsPDF();
    const tableData = rows.map(row => Object.values(row.original));
    const tableHeaders = columns.map(c => c.header);

    autoTable(doc, {
      head: [tableHeaders],
      body: tableData,
    });

    doc.save(`accounts-${new Date().toISOString()}.pdf`);
  };

  const columns = useMemo<MRT_ColumnDef<user>[]>(() => [
    {
      accessorKey: "id",
      header: "ID",
      size: 80,
      Cell: ({ row }) => row.original.id ? row.original.id.slice(0, 8) + "..." : "",
      enableSorting: false,
    },
    {
      accessorKey: "userName",
      header: "User Name",
      size: 150,
    },
    {
      accessorKey: "email",
      header: "Email",
      size: 250,
    },
    {
      accessorKey: "phoneNumber",
      header: "Phone Number",
      size: 200,
      enableSorting: false,
    },
    {
      accessorKey: "roles",
      header: "Roles",
      size: 200,
      Cell: ({ row }) => row.original.roles.join(", "),
      enableSorting: false,
    },
  ], [],);

  const renderRowActions = ({ cell, row, table }: { cell: MRT_Cell<user>, row: MRT_Row<user>, staticRowIndex?: number | undefined, table: MRT_TableInstance<user> }) => [
    <IconButton key="edit" size="small" onClick={() => console.debug(row.original.id)}><Edit /></IconButton>,
    <IconButton key="delete" size="small" onClick={() => console.debug(row.original.id)}><Delete /></IconButton>,
  ];

  const renderTopToolbarCustomActions = ({ table }: { table: MRT_TableInstance<user> }) => (
    <Box display="flex" alignItems="center" gap={1}>
      <Button key="run" variant="contained" size="small" onClick={onsubmit}>Run</Button>
      <Button key="create" variant="outlined" size="small" endIcon={<Add />} onClick={() => console.debug("create")}>Create</Button>
      <Button key="edit" variant="outlined" size="small" endIcon={<Edit />} onClick={() => console.debug("edit")}>Edit</Button>
      <Button key="delete" variant="outlined" size="small" endIcon={<Delete />} onClick={() => console.debug("delete")}>Delete</Button>
    </Box>
  );

  const renderToolbarInternalActions = ({ table }: { table: MRT_TableInstance<user> }) => [
    <MRT_ToggleGlobalFilterButton key="globalFilter" table={table} />,
    <IconButton key="share" size="medium" aria-label="teste" onClick={handleShare}><Share /></IconButton>,
    <IconButton key="clear-filters" size="medium" onClick={handleClearFilters}><ClearAll /></IconButton>,
    <IconButton key="export" size="medium" onClick={() => handleExportRows(table.getRowModel().rows)}><FileDownload /></IconButton>,
    <MRT_ToggleFiltersButton key="toggleFilters" table={table} />,
    <MRT_ShowHideColumnsButton key="showHideColumns" table={table} />,
    <MRT_ToggleDensePaddingButton key="toggleDensePadding" table={table} />,
    <MRT_ToggleFullScreenButton key="toggleFullScreen" table={table} />,
  ];

  const table = useMaterialReactTable({
    columns,
    data: users || [],

    muiTablePaperProps: ({ table }) => ({
      style: {
        zIndex: table.getState().isFullScreen ? 9999 : undefined,
      },
    }),

    muiTopToolbarProps: {
      sx: () => ({
        "& .MuiIconButton-root": {
          color: `${themeName === "light" ? colors.black : colors.white}`
        },
      })
    },

    muiTableProps: {
      sx: () => ({
        "& .MuiIconButton-root, .MuiSelect-icon": {
          color: `${themeName === "light" ? colors.black : colors.white}`
        },
      })
    },

    muiFilterSliderProps: {
      sx: () => ({
        "& .MuiIconButton-root, .MuiSelect-icon": {
          color: `${themeName === "light" ? colors.black : colors.white}`
        },
      })
    },

    muiBottomToolbarProps: {
      sx: () => ({
        "& .MuiIconButton-root, .MuiSelect-icon": {
          color: `${themeName === "light" ? colors.black : colors.white}`
        },
      })
    },

    renderToolbarInternalActions: renderToolbarInternalActions,
    renderTopToolbarCustomActions: renderTopToolbarCustomActions,

    initialState: {
      globalFilter,
      sorting,
      columnFilters,
      pagination,
      showColumnFilters: true,
      density: "compact",
    },

    onGlobalFilterChange: setGlobalFilter,

    manualFiltering: true,
    onColumnFiltersChange: setColumnFilters,

    manualSorting: true,
    onSortingChange: setSorting,

    manualPagination: true,
    onPaginationChange: setPagination,

    rowCount: rowCount ?? 0,

    // enableRowActions: true,
    // renderRowActions: renderRowActions,

    enableRowSelection: true,
    getRowId: row => row.id ?? "",

    state: {
      isLoading: isLoading(),
      columnFilters,
      globalFilter,
      pagination,
      sorting,
    },

    enableColumnResizing: true,
    layoutMode: "grid",
  });

  return <MaterialReactTable table={table} />;
};