import React, { createContext, useContext, useState } from "react";
import type { User } from "../types/User";
import axios from "axios";

interface UserContextType {
    user: User | null;
    login: (login: string, password: string) => Promise<boolean>;
    logout: () => void;
}

const UserContext = createContext<UserContextType | undefined>(undefined);

export const UserProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const [user, setUser] = useState<User | null>(null);

    const login = async (email: string, password: string): Promise<boolean> => {
        try {
            const response = await axios.post(
                `http://localhost:5000/login?email=${email}&password=${password}`,
                null,
                { withCredentials: true }
            );
            if (response.status === 200) {
                const userData: User = response.data;
                setUser(userData);
                return true;
            }
            return false;
        }
        catch (error) {
            console.error("Login error:", error);
            alert("Login failed. Please try again.");
        }
        return false;
    };

    const logout = () => setUser(null);

    return (
        <UserContext.Provider value={{ user, login, logout }}>
            {children}
        </UserContext.Provider>
    );
};

export const useUser = (): UserContextType => {
    const context = useContext(UserContext);
    if (!context) throw new Error("useUser must be used within an UserProvider");
    return context;
};