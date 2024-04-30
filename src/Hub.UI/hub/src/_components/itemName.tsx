import { Box } from "@mui/material";
import { ReactNode } from "react";

import { FaFile, FaFileImage, FaFileVideo, FaFolder, FaMusic, FaRegFilePdf, FaRegFileZipper } from "react-icons/fa6";
import { SiGoogledocs, SiGooglesheets, SiGoogleslides } from "react-icons/si";

export const ItemName = ({ name }: { name: string }): ReactNode => {
  const extension = name.split(".").slice(1, 2).join(".");
  let icon = null;

  switch (extension) {
    case "pdf":
      icon = <FaRegFilePdf />;
      break;
    case "doc":
    case "docx":
      icon = <SiGoogledocs />;
      break;
    case "xls":
    case "xlsx":
      icon = <SiGooglesheets />;
      break;
    case "ppt":
    case "pptx":
      icon = <SiGoogleslides />;
      break;
    case "jpg":
    case "jpeg":
    case "png":
    case "gif":
      icon = <FaFileImage />;
      break;
    case "mp4":
    case "avi":
    case "mkv":
      icon = <FaFileVideo />;
      break;
    case "mp3":
    case "wav":
    case "flac":
      icon = <FaMusic />;
      break;
    case "zip":
    case "rar":
      icon = <FaRegFileZipper />;
      break;
    default:
      icon = <FaFile />;
      break;
  }
  return (
    <Box display="flex" alignItems="center" gap={1}>
      {extension ? icon : <FaFolder />}
      {name}
    </Box>
  );
};