import { Routes, Route, Navigate } from "react-router-dom";
import { useAppSelector } from "./app/hooks";
import AppLayout from "./components/layout/AppLayout";
import LoginPage from "./pages/LoginPage";
import RegistrationPage from "./pages/RegistrationPage";
import ActivitiesFeed from "./pages/ActivitiesFeed";

function App() {
  const token = useAppSelector((state) => state.auth.token);

  return (
    <Routes>
      <Route
        path="/login"
        element={!token ? <LoginPage /> : <Navigate to="/" />}
      />
      <Route
        path="/register"
        element={!token ? <RegistrationPage /> : <Navigate to="/" />}
      />
      <Route
        path="/*"
        element={token ? <AppLayout /> : <Navigate to="/login" />}
      >
        <Route index element={<ActivitiesFeed />} />
      </Route>
    </Routes>
  );
}

export default App;
