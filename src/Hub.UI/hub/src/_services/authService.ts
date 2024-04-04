import { authToken, user } from "@/_types";
import { getKey, getStorageValue } from "./localStorageService";

export const getAuthToken = async (): authToken => {
  const accessToken = getStorageValue(getKey('accessToken')) as string;
  const refreshToken = getStorageValue(getKey('refreshToken')) as string;

  return { accessToken, refreshToken };
}

export const setAuthToken = (authToken: authToken) => {
  localStorage.setItem(getKey('accessToken'), authToken.accessToken);
  localStorage.setItem(getKey('refreshToken'), authToken.refreshToken);
}

export const getUser = async (): user => {
  const user = getStorageValue(getKey('user')) as user;
  return user;
}

export const setUser = (user: user) => {
  localStorage.setItem(getKey('user'), JSON.stringify(user));
}