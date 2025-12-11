// src/public/LoginPage.tsx
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { login } from "../features/auth/api";

export function LoginPage() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    if (loading) return;

    setLoading(true);
    setError(null);

    try {
      await login({ email, password });
      navigate("/admin/vehicles");
    } catch (err: any) {
      console.error(err);
      setError(err.message ?? "Login failed");
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="public-page">
      <h1 className="public-title">Login</h1>
      <p className="public-subtitle">
        Sign in to manage the Autorovers catalog.
      </p>

      <form
        onSubmit={handleSubmit}
        style={{
          maxWidth: "380px",
          padding: "1.25rem 1.5rem",
          borderRadius: "12px",
          background: "#020617",
          border: "1px solid #111827",
          boxShadow: "0 10px 24px rgba(15,23,42,0.8)",
        }}
      >
        {error && (
          <div
            style={{
              marginBottom: "0.75rem",
              fontSize: "0.8rem",
              color: "#fecaca",
              background: "#450a0a",
              borderRadius: "6px",
              padding: "0.4rem 0.6rem",
            }}
          >
            {error}
          </div>
        )}

        <div className="field" style={{ marginBottom: "0.7rem" }}>
          <label>Email</label>
          <input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </div>

        <div className="field" style={{ marginBottom: "0.9rem" }}>
          <label>Password</label>
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>

        <button className="btn" type="submit" disabled={loading}>
          {loading ? "Signing in..." : "Login"}
        </button>
      </form>
    </div>
  );
}
