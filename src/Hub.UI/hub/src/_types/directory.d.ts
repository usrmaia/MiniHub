export type directory = {
  id?: string;
  name: string;
  description?: string;
  parentId?: string;
  createdAt?: Date;
  updatedAt?: Date;
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