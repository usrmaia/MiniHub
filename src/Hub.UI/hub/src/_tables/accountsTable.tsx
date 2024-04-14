"use client";

import { MRT_ColumnFiltersState, MRT_PaginationState, type MRT_ColumnDef, type MRT_SortingState } from "material-react-table";
import { useCallback, useEffect, useMemo, useState } from "react";
import { useDispatch, useSelector } from "react-redux";

import env from "@/env";
import { selectStatus, selectTotalCount, selectUsers } from "@/_redux/features/user/slice";
import { deleteUser, getUsers } from "@/_redux/features/user/thunks";
import { AppDispatch } from "@/_redux/store";
import { user, userFilter } from "@/_types";
import { useDefaultMaterialReactTable } from "./defaultMaterialReactTable";

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

  const toCreate = "/account";
  const toEdit = "/account";
  const handleDelete = (id: string) =>
    dispatch(deleteUser(id));

  useEffect(() => { onSubmit(); }, []);

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

    // if (env.NODE_ENV !== "production")
    //   console.debug("buildQueryString", params.toString());

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

  const onSubmit = () => {
    buildQueryString();
    const filter = getQueryObject();

    // if (env.NODE_ENV !== "production")
    //   console.debug("onSubmit", filter);

    dispatch(getUsers(filter));
  };

  useEffect(() => {
    const search = buildQueryString();
    const newUrl = `${window.location.pathname}?${search}`;
    window.history.replaceState({}, "", newUrl);
  }, [status]);

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

  return useDefaultMaterialReactTable({
    columns,
    data: users || [],
    title: "Accounts",

    setGlobalFilter,
    setColumnFilters,
    setSorting,
    setPagination,
    rowCount: rowCount ?? users?.length ?? 0,

    initialState: {
      globalFilter,
      sorting,
      columnFilters,
      pagination,
    },

    state: {
      columnFilters,
      globalFilter,
      pagination,
      sorting,
    },

    onSubmit,
    toCreate,
    toEdit,
    handleDelete,

    isLoading,
  });
};