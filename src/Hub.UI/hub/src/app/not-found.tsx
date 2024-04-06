import { Box, Typography } from "@mui/material";
import Link from "next/link";

export default function NotFound() {
  return (
    <Box>
      <Typography>Not Found</Typography>
      <Typography>Could not find requested resource</Typography>
      <Link href="/">Return Home</Link>
    </Box>
  );
}