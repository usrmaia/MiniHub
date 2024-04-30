import { directory, file } from "@/_types";

export type itemsFilter = {
  search?: string;
  name?: string;
  userId?: string;
  fileId?: string;
  directoryId?: string;
  parentId?: string;
  flagId?: string;
  roleId?: string;

  nameOrderSort?: "asc" | "desc";
  flagOrderSort?: "asc" | "desc";
  createdAtOrderSort?: "asc" | "desc";
  updatedAtOrderSort?: "asc" | "desc";
  userNameOrderSort?: "asc" | "desc";
};

export type items = {
  directories: directory[];
  files: file[];
  totalCount: number;
};