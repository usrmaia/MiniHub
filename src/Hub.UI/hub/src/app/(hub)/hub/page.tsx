import { Typography } from "@mui/material";

import { DirectoriesTable } from "@/_tables";

export default function Page() {
  return (
    <>
      <Typography variant="h5" mb={2}>Directories</Typography>
      <DirectoriesTable />
    </>
  );
}