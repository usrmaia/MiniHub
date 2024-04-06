"use client";

import { getStorageValue, setStorageValue } from "./localStorageService";
import { authToken, user } from "@/_types";

export const getAuthToken = (): authToken =>
  getStorageValue("authToken", null) as authToken;

export const setAuthToken = (authToken: authToken) =>
  setStorageValue("authToken", authToken);

export const getUser = (): user =>
  getStorageValue("user", null) as user;

export const setUser = (user: user) =>
  setStorageValue("user", user);