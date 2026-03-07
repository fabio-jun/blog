import { useState } from "react";
import { useAuth } from "../contexts/AuthContext"
import { useNavigate } from "react-router-dom";

export default function RegisterPage() {
    const [userName, setUserName] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");
    const { register } = useAuth();
    const navigate = useNavigate();

    const handleSubmit =async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            await register(userName, email, password);
            navigate("/");
        }
        catch (err: any) {                                                                                                              
            setError(err.response?.data?.error || "Failed to register.");                                                                 
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            <h1>Register</h1>
            {error && <p style={{ color: "red" }}>{error}</p>}
            <input type="text" placeholder="Username" value={userName} onChange={(e) => setUserName(e.target.value)} />
            <input type="email" placeholder="Email" value={email} onChange={(e) => setEmail(e.target.value)} />
            <input type="password" placeholder="Password" value={password} onChange={(e) => setPassword(e.target.value)} />
            <button type="submit">Register</button>
        </form>
    );
}