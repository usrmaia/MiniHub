import { createAsyncThunk } from "@reduxjs/toolkit";

import { Axios } from "@/_http/axios";
import { getAuthToken, setAuthToken } from "@/_services";
import { authToken, login, user, userToken } from "@/_types";

const LOGIN_URL = "/Auth/login";
const REFRESH_TOKEN_URL = "/Auth/refresh-token";
const CURRENT_USER_URL = "/Auth/user";

export const loginUser = createAsyncThunk(
  "auth/loginUser",
  async (loginData: login): Promise<userToken> => {
    const res = await Axios.post<userToken>(LOGIN_URL, loginData);
    return res.data as userToken;
  }
)

export const refreshToken = createAsyncThunk(
  "auth/refreshToken",
  async (): Promise<authToken> => {
    const authToken = getAuthToken();
    authToken.accessToken = authToken.refreshToken;
    setAuthToken(authToken);

    const res = await Axios.post<authToken>(REFRESH_TOKEN_URL);
    return res.data as authToken;
  }
)

export const currentUser = createAsyncThunk(
  "auth/currentUser",
  async (): Promise<user> => {
    const res = await Axios.get<user>(CURRENT_USER_URL);
    return res.data as user;
  }
)