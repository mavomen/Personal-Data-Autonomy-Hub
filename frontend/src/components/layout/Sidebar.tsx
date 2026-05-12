import {
  Drawer,
  List,
  ListItemButton,
  ListItemIcon,
  ListItemText,
} from "@mui/material";
import DashboardIcon from "@mui/icons-material/Dashboard";
import LogoutIcon from "@mui/icons-material/Logout";
import { useNavigate } from "react-router-dom";
import { useAppDispatch } from "../../app/hooks";
import { logout } from "../../features/auth/authSlice";

const DRAWER_WIDTH = 240;

export default function Sidebar() {
  const navigate = useNavigate();
  const dispatch = useAppDispatch();

  const handleLogout = () => {
    dispatch(logout());
    navigate("/login");
  };

  return (
    <Drawer
      variant="permanent"
      sx={{
        width: DRAWER_WIDTH,
        flexShrink: 0,
        "& .MuiDrawer-paper": { width: DRAWER_WIDTH, boxSizing: "border-box" },
      }}
    >
      <List>
        <ListItemButton onClick={() => navigate("/")}>
          <ListItemIcon>
            <DashboardIcon />
          </ListItemIcon>
          <ListItemText primary="Activities" />
        </ListItemButton>
        <ListItemButton onClick={handleLogout}>
          <ListItemIcon>
            <LogoutIcon />
          </ListItemIcon>
          <ListItemText primary="Logout" />
        </ListItemButton>
      </List>
    </Drawer>
  );
}
