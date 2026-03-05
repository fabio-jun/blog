import { Link } from "react-router-dom";
import { useAuth } from "../contexts/AuthContext";


export default function Navbar() {
    // Get user and logout function from AuthContext
    const { user, logout } = useAuth();

    return (
        <nav>
            <Link to="/">Home</Link>
            {/*Ternary operator that shows email and logout button if logged in, otherwise show login.register links*/}
            {user ? (
                <>
                    <span> Hello, {user.email}</span>
                    <button onClick ={logout}>Logout</button>
                </>
            ) : (
                <>
                <Link to="/login">Login</Link>
                <Link to="/register">Register</Link>
                </>
            )}
        </nav>
    );
}