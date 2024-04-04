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

