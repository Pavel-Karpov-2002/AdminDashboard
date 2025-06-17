import React, { createContext, useContext, useState, useEffect } from "react";
import { User } from "../types/User";
import axios from "axios";

interface AdminContextType {
  users: User[];
  fetchUsers: () => Promise<void>;
  updateUser: (updatedUser: User) => Promise<boolean>;
}

const AdminContext = createContext<AdminContextType | undefined>(undefined);

export const AdminProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [users, setUsers] = useState<User[]>([]);
  const [isEditing, setIsEditing] = useState<boolean>(false);

  const fetchUsers = async () => {
    try {
      axios.get("http://localhost:5000/clients", { withCredentials: true })
      .then((response) => { 
        const data: User[] = response.data;
        setUsers(data); 
    })
      .catch((err) => console.error(err));
     
    } catch (error) {
      console.error("Error fetching users:", error);
    }
  };

  const updateUser = async (updatedUser: User) => {
    try {
    const token = localStorage.getItem("token");
    axios
      .put(`http://localhost:5000/user/update?id=${updatedUser.id}`, updatedUser, { withCredentials: true })
      .then((req) => { return(true); })
      .catch((err) => console.error(err));
    } catch (error) {
      console.error("Error updating user:", error);
    }
    return false;
  };

  React.useEffect(() => {
    fetchUsers();
  }, []);

  return (
    <AdminContext.Provider value={{ users, fetchUsers, updateUser }}>
      {children}
    </AdminContext.Provider>
  );
};

export const useAdmin = (): AdminContextType => {
  const context = useContext(AdminContext);
  if (!context) throw new Error("useAdmin must be used within an AdminProvider");
  return context;
};
