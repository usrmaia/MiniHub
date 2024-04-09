import { Typography } from "@mui/material";

import { UpdatePasswordForm } from "@/_forms";

export default function Page() {
  return (
    <>
      <Typography variant="h5" mb={2}>Change Password</Typography>
      <UpdatePasswordForm />
    </>
  );
}