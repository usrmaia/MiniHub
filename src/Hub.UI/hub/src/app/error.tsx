'use client'

import { Box, Typography } from '@mui/material'

export default function Error({ error }: { error: Error & { digest?: string } }) {
  console.error(error);

  return (
    <Box>
      <Typography align='center'>
        Something went wrong
      </Typography>
      <p>{error.message}</p>
    </Box>
  )
}