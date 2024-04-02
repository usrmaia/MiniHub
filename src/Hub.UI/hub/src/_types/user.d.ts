export type user = {
  id: string;
  username: string;
  email: string;
  phonenumber: string;
  password: string;
  role: string[];
};

export type updatedPassword = {
  oldPassword: string;
  password: string;
};

export type role = {
  id: string;
  name: string;
};

export type userRole = {
  userId: string;
  roleName: string;
};

