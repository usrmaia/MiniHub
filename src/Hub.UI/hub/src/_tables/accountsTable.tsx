"use client";

import { MaterialReactTable, MRT_ColumnFiltersState, MRT_PaginationState, MRT_TableInstance, useMaterialReactTable, type MRT_ColumnDef, type MRT_SortingState } from "material-react-table";
import { Send } from "@mui/icons-material";
import { Box, Button, IconButton } from "@mui/material";
import { useCallback, useEffect, useMemo, useState } from "react";
import { useDispatch, useSelector } from "react-redux";

import env from "@/env";
import { selectStatus, selectTotalCount, selectUsers } from "@/_redux/features/user/slice";
import { getUsers } from "@/_redux/features/user/thunks";
import { AppDispatch } from "@/_redux/store";
import { user, userFilter } from "@/_types";

export const AccountsTable = () => {
  const dispatch = useDispatch<AppDispatch>();

  const users = useSelector(selectUsers);
  const rowCount = useSelector(selectTotalCount);

  const status = useSelector(selectStatus);
  const isLoading = () => status === "idle" || status === "loading";

  let params: URLSearchParams;
  if (typeof window === "undefined") params = new URLSearchParams();
  else params = new URLSearchParams(window.location.search);

  const [globalFilter, setGlobalFilter] = useState<string>(params.get("search") ?? "");
  const [sorting, setSorting] = useState<MRT_SortingState>([]);
  const [columnFilters, setColumnFilters] = useState<MRT_ColumnFiltersState>([]);
  const [pagination, setPagination] = useState<MRT_PaginationState>({
    pageIndex: parseInt(params.get("pageIndex") as string) || 0,
    pageSize: parseInt(params.get("pageSize") as string) || 10
  });

  useEffect(() => { onsubmit(); }, []);

  // useEffect(() => { if (env.NODE_ENV !== "production") console.debug("users", users); }), [users];
  // useEffect(() => { if (env.NODE_ENV !== "production") console.debug("globalFilter", globalFilter); }), [globalFilter];
  // useEffect(() => { if (env.NODE_ENV !== "production") console.debug("sorting", sorting); }), [sorting];
  // useEffect(() => { if (env.NODE_ENV !== "production") console.debug("columnFilters", columnFilters); }), [columnFilters];
  // useEffect(() => { if (env.NODE_ENV !== "production") console.debug("pagination", pagination); }), [pagination];
  // useEffect(() => { if (env.NODE_ENV !== "production") console.debug("rowCount", rowCount); }), [rowCount];

  const buildQueryString = useCallback(() => {
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

  const columns = useMemo<MRT_ColumnDef<user>[]>(
    () => [
      {
        accessorKey: "id",
        header: "ID",
        size: 50,
        Cell: ({ row }) => row.original.id ? row.original.id.slice(0, 8) + "..." : "",
        enableSorting: false,
      },
      {
        accessorKey: "userName",
        header: "User Name",
        size: 100,
      },
      {
        accessorKey: "email",
        header: "Email",
        size: 150,
      },
      {
        accessorKey: "phoneNumber",
        header: "Phone Number",
        size: 100,
        enableSorting: false,
      },
      {
        accessorKey: "roles",
        header: "Roles",
        size: 100,
        Cell: ({ row }) => row.original.roles.join(", "),
        enableSorting: false,
      },
    ],
    [],
  );

  const renderTopToolbarCustomActions = ({ table }: { table: MRT_TableInstance<user> }) => {
    const Buttons = () => (
      <>
        <Button
          variant="contained"
          size="small"
          onClick={onsubmit}
        >
          Run
        </Button>
      </>
    );

    const IconButtons = () => (
      <>
        <IconButton size="small">
          <Send fontSize="inherit" />
        </IconButton>
      </>
    );

    return (
      <Box display='flex' justifyContent='space-between' width='-webkit-fill-available' height={40}>
        <Buttons />
        <IconButtons />
      </Box>
    );
  };

  const table = useMaterialReactTable({
    columns,
    data: users || [],

    muiTablePaperProps: ({ table }) => ({
      style: {
        zIndex: table.getState().isFullScreen ? 9999 : undefined,
      },
    }),

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