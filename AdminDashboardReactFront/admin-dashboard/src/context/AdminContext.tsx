import React, { createContext, useContext, useState, useEffect } from "react";

export interface User {
  id: number;
  login: string;
  password?: string;
  balance: Record<string, number>;
}

interface AdminContextType {
  users: User[];
  fetchUsers: () => Promise<void>;
  updateUser: (updatedUser: User) => Promise<void>;
}

const AdminContext = createContext<AdminContextType | undefined>(undefined);

export const AdminProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [users, setUsers] = useState<User[]>([]);

  const fetchUsers = async () => {
    try {
      const response = await fetch("https://localhost:5000/api/users");
      if (!response.ok) throw new Error("Failed to fetch users");
      const data: User[] = await response.json();
      setUsers(data);
    } catch (error) {
      console.error("Error fetching users:", error);
    }
  };

  const updateUser = async (updatedUser: User) => {
    try {
      const response = await fetch(`https://localhost:5000/api/user/${updatedUser.id}`, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(updatedUser),
      });
      if (!response.ok) throw new Error("Failed to update user");
      const updated = await response.json();
      setUsers((prevUsers) =>
        prevUsers.map((user) => (user.id === updated.id ? updated : user))
      );
    } catch (error) {
      console.error("Error updating user:", error);
    }
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
