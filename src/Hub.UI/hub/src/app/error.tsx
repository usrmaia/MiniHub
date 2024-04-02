'use client'

import { Box, Typography } from '@mui/material'
import { useEffect } from 'react'

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