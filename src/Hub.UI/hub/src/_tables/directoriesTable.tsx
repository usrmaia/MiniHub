"use client";

import { MRT_ColumnFiltersState, MRT_PaginationState, type MRT_ColumnDef, type MRT_SortingState } from "material-react-table";
import { useCallback, useEffect, useMemo, useState } from "react";
import { useDispatch, useSelector } from "react-redux";

import env from "@/env";
import { selectStatus, selectTotalCount, selectDirectories } from "@/_redux/features/directory/slice";
import { getDirectories } from "@/_redux/features/directory/thunks";
import { AppDispatch } from "@/_redux/store";
import { directory, directoryFilter } from "@/_types";
import { useHubMaterialReactTable } from "./hubMaterialReactTable";

export const DirectoriesTable = () => {
  const dispatch = useDispatch<AppDispatch>();

  const directories = useSelector(selectDirectories);
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

  const toCreate = "/directory";
  const toEdit = "/directory";
  // const handleDelete = (id: string) =>
  //   dispatch(deleteUser(id));

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

  const getQueryObject = (): directoryFilter => {
    const filter: directoryFilter = {
      search: params.get("search") ?? undefined,
      name: params.get("name") ?? undefined,
      flag: params.get("flag") ?? undefined,
      userId: params.get("userId") ?? undefined,
      pageIndex: parseInt(params.get("pageIndex") || "0"),
      pageSize: parseInt(params.get("pageSize") || "10"),
      nameOrderSort: params.get("nameSort") as "asc" | "desc" ?? undefined,
      createdAtOrderSort: params.get("createdAtSort") as "asc" | "desc" ?? undefined,
      updatedAtOrderSort: params.get("updatedAtSort") as "asc" | "desc" ?? undefined,
    };

    return filter;
  };

  const onSubmit = () => {
    buildQueryString();
    const filter = getQueryObject();

    // if (env.NODE_ENV !== "production")
    //   console.debug("onSubmit", filter);

    dispatch(getDirectories(filter));
  };

  useEffect(() => {
    const search = buildQueryString();
    const newUrl = `${window.location.pathname}?${search}`;
    window.history.replaceState({}, "", newUrl);
  }, [status]);

  const columns = useMemo<MRT_ColumnDef<directory>[]>(() => [
    {
      accessorKey: "id",
      header: "ID",
      size: 80,
      Cell: ({ row }) => row.original.id ? row.original.id.slice(0, 8) + "..." : "",
      enableSorting: false,
    },
    {
      accessorKey: "name",
      header: "Name",
      size: 200,
    },
    {
      accessorKey: "flags",
      header: "Flags",
      size: 100,
      Cell: ({ row }) => row.original.flags.map(f => f.name).join(", "),
    },
    {
      accessorKey: "user.userName",
      header: "Owner",
      size: 100,
    },
    {
      accessorKey: "updatedAt",
      header: "Last Updated",
      size: 200,
      Cell: ({ row }) => {
        const lastUpdate = new Date(row.original.updatedAt ?? "");
        return lastUpdate ? lastUpdate.toLocaleTimeString() + " " + lastUpdate.toLocaleDateString() : "";
      },
    },
  ], [],);

  return useHubMaterialReactTable({
    columns,
    data: directories || [],
    title: "Directories",

    setGlobalFilter,
    setColumnFilters,
    setSorting,
    setPagination,
    rowCount: rowCount ?? directories?.length ?? 0,

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
    // handleDelete,

    isLoading,
  });
};