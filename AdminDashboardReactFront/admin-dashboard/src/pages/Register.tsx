import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useUser } from "../context/UserContext";

const Register: React.FC = () => {
  const [loginVal, setLoginVal] = useState("");
  const [emailVal, setEmailVal] = useState("");
  const [password, setPassword] = useState("");
  const { register } = useUser();
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    await register(loginVal, password);
    navigate("/admin");
  };

  return (
    <form onSubmit={handleSubmit} className="p-4">
      <h2>Register</h2>
      <input value={loginVal} onChange={(e) => setLoginVal(e.target.value)} placeholder="Login" />
      <input type="email" value={emailVal} onChange={(e) => setEmailVal(e.target.value)} placeholder="Email" />
      <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} placeholder="Password" />
      <button type="submit">Register</button>
    </form>
  );
};

export default Register;
