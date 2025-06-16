import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useUser } from "../context/UserContext";

export function Login() {
  const [loginVal, setLoginVal] = useState("");
  const [password, setPassword] = useState("");
  const { login } = useUser();
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const success = await login(loginVal, password);
    if (success) navigate("/admin");
    else alert("Invalid credentials");
  };

  return (
    <form onSubmit={handleSubmit} className="p-4">
      <h2>Login</h2>
      <input value={loginVal} onChange={(e) => setLoginVal(e.target.value)} placeholder="Login" />
      <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} placeholder="Password" />
      <button type="submit">Login</button>
    </form>
  );
};

export default Login;