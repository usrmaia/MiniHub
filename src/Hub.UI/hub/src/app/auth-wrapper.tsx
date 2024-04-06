"use client";

import { jwtDecode } from "jwt-decode";
import { useRouter } from "next/navigation";
import { useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";

import { logoutUser, selectAuthToken, selectStatus, selectUser } from "@/_redux/features/auth/slice";
import { Loading } from "@/_components";
import { jwtPayload } from "@/_types";

export const AuthWrapper = ({ children }: Readonly<{ children: React.ReactNode }>) => {
  const dispatch = useDispatch();
  const { push } = useRouter();
  const user = useSelector(selectUser);
  const token = useSelector(selectAuthToken);
  const status = useSelector(selectStatus);

  useEffect(() => {
    if (token?.accessToken) {
      const jwt: jwtPayload = jwtDecode(token?.accessToken || "");
      const authorized = user?.id === jwt.nameid
        && user?.userName === jwt.unique_name
        && jwt.role.includes("Colaborador")
        && jwt.exp > Date.now() / 1000;

      if (authorized)
        return;
    }

    push("/signin");
    dispatch(logoutUser());
  }, [dispatch, push, token, user]);

  return status === "loading" ? <Loading /> : children;
};
