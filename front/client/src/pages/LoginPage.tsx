import { useState } from "react";
import { useAuth } from "../contexts/AuthContext";
import { useNavigate } from "react-router-dom";

export default function LoginPage() {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");
    const { login } = useAuth();
    const navigate = useNavigate();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            await login(email, password);
            navigate("/")
        } catch {
            setError("Invalid email or password");
        }
    };

    return (
        <form onSubmit ={handleSubmit}>
            <h1>Login</h1>
            {error && <p style={{ color: "red" }}>{error}</p>}
            <input type="email" placeholder="Email" value={email} onChange={(e) => setEmail(e.target.value)} />
            <input type="password" placeholder = "Password" value={password} onChange={(e) => setPassword(e.target.value)} />
            <button type="submit">Enter</button>    
        </form>
    );
}