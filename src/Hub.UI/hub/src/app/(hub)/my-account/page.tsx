"use client";

import { Typography } from "@mui/material";
import Link from "next/link";
import { useSelector } from "react-redux";

import { selectUser } from "@/_redux/features/auth/slice";
import { AccountForm } from "@/_forms";
import { user } from "@/_types";

export default function MyAccount() {
  const currentUser = useSelector(selectUser);

  return (
    <>
      <Typography variant="h5" mb={2}>My Account</Typography>
      <AccountForm user={currentUser as user} />
      <Link href="/my-account/change-password" style={{ textDecoration: "none", color: "inherit" }}>
        <Typography variant="subtitle2" color="primary" mt={2}>Change Password?</Typography>
      </Link>
    </>
  );
}