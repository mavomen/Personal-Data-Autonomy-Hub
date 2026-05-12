import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { TextField, Button, Box, Typography } from "@mui/material";

export default function RegistrationPage() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    // Simulate registration
    navigate("/login");
  };

  return (
    <Box sx={{ maxWidth: 400, mx: "auto", mt: 8 }}>
      <Typography variant="h4" gutterBottom>
        Register
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
          Register
        </Button>
      </form>
    </Box>
  );
}
