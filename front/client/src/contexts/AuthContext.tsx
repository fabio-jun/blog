import { createContext, useContext, useState, useEffect, type ReactNode } from "react";
import { loginUser, registerUser } from "../api/authApi";

// User type definition (extracted from JWT payload)
interface User {
    id: string;
    email: string;
    role: string;
}

// Context type
interface AuthContextType {
    user: User | null;
    login: (email: string, password: string) => Promise<void>;
    register: (userName: string, email: string, password: string) => Promise<void>;
    logout: () => void;
}

// Creates the context
const AuthContext = createContext<AuthContextType>({} as AuthContextType);

function parseJwt(token: string): User {
    const base64Url = token.split(".")[1];
    const base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");
    const payload = JSON.parse(atob(base64));

    return {
      id: payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"],
      email: payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"],
      role: payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/role"],
    };
}
// Component that wraps the app
export function AuthProvider({ children }: { children: ReactNode }) {
    //User state
    const [user, setUser] = useState<User | null>(null);

    // Restore section when reloading (avoid losing user state on page refresh)
    useEffect(() => {
        const token = localStorage.getItem("accessToken");
        if (token) {
            setUser(parseJwt(token));
        }
    }, []);

    // Decodes the JWT token to extract user information and updates the user state
    const login = async (email: string, password: string) => {
        const response = await loginUser({email, password});
        localStorage.setItem("accessToken", response.data.accessToken);
        localStorage.setItem("refreshToken", response.data.refreshToken);
        setUser(parseJwt(response.data.accessToken));
    };

    const register = async (userName: string, email: string, password: string) => {
        const response = await registerUser({ userName, email, password });
        localStorage.setItem("accessToken", response.data.accessToken);
        localStorage.setItem("refreshToken", response.data.refreshToken);
        setUser(parseJwt(response.data.accessToken));
    };

    const logout = () => {
        localStorage.removeItem("accessToken");
        localStorage.removeItem("refreshToken");
        setUser(null);
    }

    return (
        <AuthContext.Provider value = {{ user, login, register, logout }}>
            {children}
        </AuthContext.Provider>
    );
}


export const useAuth = () => useContext(AuthContext);
