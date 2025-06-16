import React, { createContext, useContext, useState } from "react";
import { User } from "../types/User";

interface UserContextType {
    user: User | null;
    login: (login: string, password: string) => Promise<boolean>;
    register: (login: string, password: string) => Promise<void>;
    logout: () => void;
    updateUser: (updatedUser: User) => Promise<void>;
}

const UserContext = createContext<UserContextType | undefined>(undefined);

export const UserProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const [user, setUser] = useState<User | null>(null);

    const login = async (login: string, password: string): Promise<boolean> => {
        try {
            const response = await fetch("https://localhost:5000/api/login", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({ login, password }),
            });
            if (!response.ok) return false;
            const userData: User = await response.json();
            setUser(userData);
            return true;
        } catch (error) {
            console.error("Login error:", error);
            return false;
        }
    };

    const register = async (login: string, password: string): Promise<void> => {
        try {
            const response = await fetch("https://localhost:5000/api/register", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({ login, password }),
            });
            if (!response.ok) throw new Error("Registration failed");
            const userData: User = await response.json();
            setUser(userData);
        } catch (error) {
            console.error("Registration error:", error);
        }
    };

    const logout = () => setUser(null);

    const updateUser = async (updatedUser: User) => {
        try {
            const response = await fetch("https://localhost:5000/api/users/" + updatedUser.id, {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(updatedUser),
            });
            if (!response.ok) throw new Error("Failed to update user");
        } catch (error) {
            console.error("Error updating user:", error);
        }
    };

    return (
        <UserContext.Provider value={{ user, login, register, logout, updateUser }}>
            {children}
        </UserContext.Provider>
    );
};

export const useUser = (): UserContextType => {
    const context = useContext(UserContext);
    if (!context) throw new Error("useUser must be used within an UserProvider");
    return context;
};