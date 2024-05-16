export type file = {
  id?: string;
  name?: string;
  description?: string;
  length?: number;
  path?: string;
  createdAt?: Date;
  updatedAt?: Date;
  directoryId?: string;
  directory?: directory;
  flags?: flag[];
  roles?: role[];
  userId?: string;
  user?: user;
};

export type fileFlag = {
  fileId: string;
  file?: file;
  flagId: string;
  flag?: flag;
};

export type fileRole = {
  fileId: string;
  file?: file;
  roleId: string;
  role?: role;
};

export type move = {
  fileId: string;
  directoryId: string;
};