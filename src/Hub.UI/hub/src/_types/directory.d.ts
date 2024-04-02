export type directory = {
  id: string | undefined;
  name: string;
  description: string | undefined;
  parentId: string | undefined;
  createdAt: Date | undefined;
  updatedAt: Date | undefined;
  files: file[];
  flags: flag[];
  roles: role[];
  userId: string;
};

export type directoryFlag = {
  directoryId: string;
  directoy: directory;
  flagId: string;
  flag: flag;
};

export type directoryRole = {
  directoryId: string;
  directoy: directory;
  roleId: string;
  role: role;
};