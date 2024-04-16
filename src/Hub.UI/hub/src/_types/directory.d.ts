import { file, flag, role, user } from "@/_types";

export type directoryFilter = {
  search?: string;

  id?: string;
  name?: string;
  flag?: string;
  role?: string;
  userId?: string;

  pageIndex?: number;
  pageSize?: number;

  nameOrderSort?: "asc" | "desc";
  createdAtOrderSort?: "asc" | "desc";
  updatedAtOrderSort?: "asc" | "desc";
};

export type directory = {
  id?: string;
  name: string;
  description?: string;
  parentId?: string;
  createdAt?: Date;
  updatedAt?: string;
  files: file[];
  flags: flag[];
  roles: role[];
  userId?: string;
  user?: user;
};

export type directoryFlag = {
  directoryId: string;
  directoy?: directory;
  flagId: string;
  flag?: flag;
};

export type directoryRole = {
  directoryId: string;
  directoy?: directory;
  roleId: string;
  role?: role;
};