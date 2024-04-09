import { colors } from "@/_theme";
import { Box, Button, Checkbox, Divider, FormControlLabel, FormGroup, Typography } from "@mui/material";

export default function Page() {
  return (
    <Box display='flex' flexDirection='column' gap={2}>
      <Typography variant="h5" mb={2}>Settings</Typography>
      <Typography variant="h6">Language</Typography>
      <Box>
        <Button variant="outlined" sx={{ borderRadius: 8 }}>Change language setting</Button>
      </Box>
      <Divider sx={{ borderColor: `${colors.imperialRed}` }} />
      <Typography variant="h6">Theme</Typography>
      <Box>
        <Button variant="outlined" sx={{ borderRadius: 8 }}>Color setting</Button>
      </Box>
      <Divider sx={{ borderColor: `${colors.imperialRed}` }} />
      <Typography variant="h6">Notification</Typography>
      <Box>
        <Button variant="outlined" sx={{ borderRadius: 8 }}>Notification setting</Button>
      </Box>
      <Divider sx={{ borderColor: `${colors.imperialRed}` }} />
      <Typography variant="h6">Off-line</Typography>
      <FormGroup>
        <FormControlLabel control={<Checkbox />} label="Edit your files offline and sync them later" />
      </FormGroup>
    </Box>
  );
}