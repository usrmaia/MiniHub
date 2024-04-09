import { Typography } from "@mui/material";

import { AccountsTable } from "@/_tables";

export default function Page() {
  return (
    <>
      <Typography variant="h5" mb={2}>Accounts</Typography>
      <AccountsTable />
    </>
  );
}