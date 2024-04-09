export type userFilter = {
  search?: string;

  id?: string;
  userName?: string;
  email?: string;
  phoneNumber?: string;
  role?: string;

  page?: number;
  pageSize?: number;

  userNameOrderDesc?: boolean;
  emailOrderDesc?: boolean;
};

export type user = {
  id?: string;
  userName: string;
  email: string;
  phoneNumber: string;
  password: string;
  roles: string[];
};

export type updatedPassword = {
  oldPassword: string;
  newPassword: string;
};

export type role = {
  id?: string;
  name: string;
};

export type userRole = {
  userId: string;
  roleName: string;
};

