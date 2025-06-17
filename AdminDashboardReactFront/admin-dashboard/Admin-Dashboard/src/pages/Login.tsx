import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useUser } from "../context/UserContext";

interface LoginProps {
  isAuthenticated: boolean;
}

export const Login: React.FC<LoginProps> = ({ isAuthenticated }) => {
  const [loginVal, setLoginVal] = useState("");
  const [password, setPassword] = useState("");
  const { login } = useUser();
  const navigate = useNavigate();

  useEffect(() => {
    if (isAuthenticated) {
      navigate("/dashboard");
    }
  }, [isAuthenticated, navigate]);
  
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
  try {
    const success = await login(loginVal, password);
    if (success) {
      navigate("/dashboard");
      window.location.reload();
    } else {
      alert("Неверный логин или пароль");
    }
  } catch (error) {
    console.error("Ошибка при логине:", error);
    alert("Произошла ошибка, попробуйте позже");
  }
  };

  return (
    <form onSubmit={handleSubmit} className="p-4">
      <h2>Login</h2>
      <input
        value={loginVal}
        onChange={(e) => setLoginVal(e.target.value)}
        placeholder="Login"
      />
      <input
        type="password"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
        placeholder="Password"
      />
      <button type="submit">Login</button>
    </form>
  );
}

export default Login;