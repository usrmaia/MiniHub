"use client";

import { MaterialReactTable, MRT_ColumnFiltersState, MRT_PaginationState, useMaterialReactTable, type MRT_ColumnDef, type MRT_SortingState } from "material-react-table";
import { useEffect, useMemo, useState } from "react";
import { useDispatch, useSelector } from "react-redux";

import { selectUsers } from "@/_redux/features/user/slice";
import { getUsers } from "@/_redux/features/user/thunks";
import { AppDispatch } from "@/_redux/store";
import { user, userFilter } from "@/_types";

export const AccountsTable = () => {
  const users = useSelector(selectUsers);
  const dispatch = useDispatch<AppDispatch>();

  const [isLoading, setIsLoading] = useState(true);
  const [filter, setFilter] = useState<userFilter>({ page: 0, pageSize: 10 });
  const [rowCount, setRowCount] = useState(0);

  useEffect(() => {
    dispatch(getUsers());
    setIsLoading(false);
  }, []);

  useEffect(() => { console.debug("user filter", filter); }, [filter]);
  useEffect(() => { console.debug("rowCount", rowCount); }, [rowCount]);

  useEffect(() => {
    const url = new URL("https://example.com");

    Object.entries(filter).forEach(([key, value]) => {
      if (value !== undefined && value !== null && value !== "")
        url.searchParams.append(key, String(value));
    });

    console.debug("url", url.toString());

  }, [filter]);

  const handlePaginationChange = ({ pageIndex, pageSize }: { pageIndex: number, pageSize: number }) =>
    setFilter(prev => ({ ...prev, page: pageIndex, pageSize }));

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

  const table = useMaterialReactTable({
    columns,
    data: users || [],

    muiTablePaperProps: ({ table }) => ({
      style: {
        zIndex: table.getState().isFullScreen ? 9999 : undefined,
      },
    }),

    initialState: {
      pagination: { pageIndex: filter.page, pageSize: filter.pageSize } as MRT_PaginationState,
    },

    onGlobalFilterChange: value => setFilter(prev => ({ ...prev, search: value })),

    manualFiltering: true,
    // onColumnFiltersChange: 

    manualSorting: true,
    // onSortingChange: 

    manualPagination: true,
    // onPaginationChange: ({ pageIndex, pageSize }) => handlePaginationChange({ pageIndex, pageSize }),

    rowCount,

    // state: {
    //   isLoading,
    //   columnFilters: {} as MRT_ColumnFiltersState,
    //   globalFilter: filter.search,
    //   pagination: { pageIndex: filter.page, pageSize: filter.pageSize } as MRT_PaginationState,
    //   sorting: {} as MRT_SortingState,
    // },

    enableColumnResizing: true,
    layoutMode: "grid",
  });

  return <MaterialReactTable table={table} />;
};