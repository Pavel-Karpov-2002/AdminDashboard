import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";
import { Login } from "./pages/Login";
import Register from "./pages/Register";
import Dashboard from "./pages/Dashboard";
import { UserProvider } from "./context/UserContext";
import PrivateRoute from "./components/PrivateRoute";
import { AdminProvider } from "./context/AdminContext";
import UsersList from "./pages/UsersList";

function App() {
  return (
    <UserProvider>
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route
          path="/admin"
          element={
            <AdminProvider>
              <PrivateRoute>
                <Dashboard />
                <UsersList />
              </PrivateRoute>
            </AdminProvider>
          }
        />


        <Route path="*" element={<Navigate to="/login" />} />
      </Routes>
    </UserProvider>
  );
};

export default App;

