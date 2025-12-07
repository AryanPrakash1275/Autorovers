import { Outlet, useNavigate, Link } from "react-router-dom";
import { clearToken } from "../../features/auth/storage";

export function AdminLayout() {
  const navigate = useNavigate();

  function handleLogout() {
    clearToken();
    navigate("/login");
  }

  return (
    <div className="admin-root">
      <header className="admin-header">
        <div
          style={{
            display: "flex",
            justifyContent: "space-between",
            alignItems: "center",
          }}
        >
          <h1>Autorovers Admin</h1>
          <div style={{ display: "flex", gap: "0.5rem", alignItems: "center" }}>
            <Link to="/vehicles" className="btn btn-ghost">
              View site
            </Link>
            <button className="btn btn-ghost" onClick={handleLogout}>
              Logout
            </button>
          </div>
        </div>
      </header>

      <main className="admin-main">
        <div className="admin-content">
          <Outlet />
        </div>
      </main>
    </div>
  );
}
