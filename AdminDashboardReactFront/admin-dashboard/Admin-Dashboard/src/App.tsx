import { Routes, Route, Navigate } from "react-router-dom";
import { Login } from "./pages/Login";
import Dashboard from "./pages/Dashboard";
import { UserProvider } from "./context/UserContext";
import PrivateRoute from "./components/PrivateRoute";
import { AdminProvider } from "./context/AdminContext";
import { useEffect, useState } from "react";
import axios from "axios";

function App() {
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  useEffect(() => {
    axios.get('http://localhost:5000/auth', { withCredentials: true })
      .then(() => {
        setIsAuthenticated(true);
      })
      .catch(() => {
        setIsAuthenticated(false);
      });
  }, []);

  return (
      <UserProvider>
        <Routes>
          <Route path="/" element={<RootRedirect isAuthenticated={isAuthenticated} />} />
          <Route path="/login" element={<Login isAuthenticated={isAuthenticated} />} />
          <Route
            path="/dashboard"
            element={
              <AdminProvider>
                <PrivateRoute isAuthenticated={isAuthenticated}>
                  <Dashboard />
                </PrivateRoute>
              </AdminProvider>
            }
          />
          <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
      </UserProvider>
  );
}

const RootRedirect: React.FC<{ isAuthenticated: boolean }> = ({ isAuthenticated }) => {
  return isAuthenticated ? <Navigate to="/dashboard" replace /> : <Navigate to="/login" replace />;
};

export default App;
