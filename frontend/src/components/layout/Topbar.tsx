import { AppBar, Toolbar, Typography, IconButton } from "@mui/material";
import MenuIcon from "@mui/icons-material/Menu";
import { useAppDispatch } from "../../app/hooks";
import { toggleSidebar } from "../../features/ui/uiSlice";

export default function Topbar() {
  const dispatch = useAppDispatch();

  return (
    <AppBar position="static">
      <Toolbar>
        <IconButton
          size="large"
          edge="start"
          color="inherit"
          aria-label="menu"
          onClick={() => dispatch(toggleSidebar())}
        >
          <MenuIcon />
        </IconButton>
        <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
          Personal Data Autonomy Hub
        </Typography>
      </Toolbar>
    </AppBar>
  );
}
