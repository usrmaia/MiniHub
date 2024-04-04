export type login = {
  username: string;
  password: string;
};

export type authToken = {
  accessToken: string;
  refreshToken: string;
};

export type userToken = {
  user: user;
  authToken: authToken;
};