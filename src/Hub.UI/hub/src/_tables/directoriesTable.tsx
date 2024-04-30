"use client";

import { MRT_ColumnFiltersState, MRT_PaginationState, type MRT_ColumnDef, type MRT_SortingState } from "material-react-table";
import { useCallback, useEffect, useMemo, useState } from "react";
import { useDispatch, useSelector } from "react-redux";

import { ItemName } from "@/_components";
import env from "@/env";
import { selectDirectories, selectFiles, selectItems, selectStatus, selectTotalCount } from "@/_redux/features/hub/slice";
import { getItems } from "@/_redux/features/hub/thunks";
import { AppDispatch } from "@/_redux/store";
import { directory, file, items, itemsFilter } from "@/_types";
import { useHubMaterialReactTable } from "./hubMaterialReactTable";

export const DirectoriesTable = () => {
  const dispatch = useDispatch<AppDispatch>();

  const items = useSelector(selectItems);
  const directories = useSelector(selectDirectories);
  const files = useSelector(selectFiles);
  const rowCount = useSelector(selectTotalCount);

  const status = useSelector(selectStatus);
  const isLoading = () => status === "idle" || status === "loading";

  const params: URLSearchParams = typeof window === "undefined" ?
    new URLSearchParams() :
    new URLSearchParams(window.location.search);

  const [globalFilter, setGlobalFilter] = useState<string>(params.get("search") ?? "");
  const [sorting, setSorting] = useState<MRT_SortingState>([]);
  const [columnFilters, setColumnFilters] = useState<MRT_ColumnFiltersState>([]);

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

    // if (env.NODE_ENV !== "production")
    //   console.debug("buildQueryString", params.toString());

    return params.toString();
  }, [globalFilter, sorting, columnFilters]);

  const getQueryObject = (): itemsFilter => {
    const filter: itemsFilter = {
      search: params.get("search") ?? undefined,
      name: params.get("name") ?? undefined,
      fileId: params.get("file") ?? undefined,
      directoryId: params.get("directory") ?? undefined,
      parentId: params.get("parent") ?? undefined,
      flagId: params.get("flag") ?? undefined,
      roleId: params.get("role") ?? undefined,
      userId: params.get("userId") ?? undefined,
      nameOrderSort: params.get("nameSort") as "asc" | "desc" ?? undefined,
      flagOrderSort: params.get("flagSort") as "asc" | "desc" ?? undefined,
      userNameOrderSort: params.get("userNameSort") as "asc" | "desc" ?? undefined,
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

    dispatch(getItems(filter));
  };

  useEffect(() => {
    const search = buildQueryString();
    const newUrl = `${window.location.pathname}?${search}`;
    window.history.replaceState({}, "", newUrl);
  }, [status]);

  const columns = useMemo<MRT_ColumnDef<directory & file>[]>(() => [
    {
      accessorKey: "name",
      header: "Name",
      size: 200,
      Cell: ({ row }) => <ItemName name={row.original.name} />,
    },
    {
      accessorKey: "flags",
      header: "Flags",
      size: 75,
      Cell: ({ row }) => row.original.flags.map(flag => flag.name).join(", "),
    },
    {
      accessorKey: "user.userName",
      header: "Owner",
      size: 75,
    },
    {
      accessorKey: "updatedAt",
      header: "Last Updated",
      size: 75,
      Cell: ({ row }) => {
        const lastUpdate = new Date(row.original.updatedAt ?? "");
        return lastUpdate ? lastUpdate.toLocaleTimeString() + " " + lastUpdate.toLocaleDateString() : "";
      },
    },
  ], [],);

  return useHubMaterialReactTable({
    columns,
    data: [...(directories ?? []), ...(files ?? [])] as (directory & file)[],
    title: "Directories",

    setGlobalFilter,
    setColumnFilters,
    setSorting,

    initialState: {
      globalFilter,
      sorting,
      columnFilters,
    },

    state: {
      columnFilters,
      globalFilter,
      sorting,
    },

    onSubmit,
    // handleDelete,

    isLoading,
  });
};