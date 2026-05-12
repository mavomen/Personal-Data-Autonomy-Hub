import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { TextField, Button, Box, Typography } from "@mui/material";
import { useAppDispatch } from "../app/hooks";
import { setCredentials } from "../features/auth/authSlice";

export default function LoginPage() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const dispatch = useAppDispatch();
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    // Simulate login – replace with real API call
    const token = "fake-jwt-token";
    dispatch(setCredentials({ token, user: { email, id: "1" } }));
    navigate("/");
  };

  return (
    <Box sx={{ maxWidth: 400, mx: "auto", mt: 8 }}>
      <Typography variant="h4" gutterBottom>
        Login
      </Typography>
      <form onSubmit={handleSubmit}>
        <TextField
          fullWidth
          margin="normal"
          label="Email"
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
        />
        <TextField
          fullWidth
          margin="normal"
          label="Password"
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
        />
        <Button fullWidth variant="contained" type="submit" sx={{ mt: 2 }}>
          Login
        </Button>
      </form>
    </Box>
  );
}
