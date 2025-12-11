// src/App.tsx
import { Routes, Route, Navigate, Link, useNavigate } from "react-router-dom";
import type { JSX, ReactNode } from "react";

import { VehiclesPage } from "./public/VehiclesPage";
import { VehicleDetailsPage } from "./public/VehicleDetailsPage";
import { VehicleListPage } from "./features/vehicles/components/VehicleListPage";
import { LoginPage } from "./public/LoginPage";
import { isLoggedIn, clearToken } from "./features/auth/storage";

import "./styles/admin.css";
import "./styles/public.css";

type LayoutProps = {
  children: ReactNode;
};

function PublicLayout({ children }: LayoutProps) {
  return (
    <div className="public-root">
      <header className="public-header">
        <div className="public-logo">
          <Link to="/vehicles">Autorovers</Link>
        </div>
        <nav className="public-nav">
          <Link to="/vehicles" className="public-nav-link">
            Vehicles
          </Link>
          <Link to="/admin/vehicles" className="public-nav-link">
            Login
          </Link>
        </nav>
      </header>

      <main className="public-main">{children}</main>
    </div>
  );
}

// Small route guard component
function RequireAuth({ children }: { children: JSX.Element }) {
  if (!isLoggedIn()) {
    return <Navigate to="/login" replace />;
  }
  return children;
}

// Admin layout with logout
function AdminLayout() {
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
          <VehicleListPage />
        </div>
      </main>
    </div>
  );
}

function App() {
  return (
    <Routes>
      {/* Public catalog + details wrapped in PublicLayout with navbar */}
      <Route
        path="/"
        element={
          <PublicLayout>
            <VehiclesPage />
          </PublicLayout>
        }
      />
      <Route
        path="/vehicles"
        element={
          <PublicLayout>
            <VehiclesPage />
          </PublicLayout>
        }
      />
      <Route
        path="/vehicles/:slug"
        element={
          <PublicLayout>
            <VehicleDetailsPage />
          </PublicLayout>
        }
      />

      {/* Login route */}
      <Route
        path="/login"
        element={
          <PublicLayout>
            <LoginPage />
          </PublicLayout>
        }
      />

      {/* Protected admin route */}
      <Route
        path="/admin/vehicles"
        element={
          <RequireAuth>
            <AdminLayout />
          </RequireAuth>
        }
      />

      {/* Default: send everything else to catalog */}
      <Route path="*" element={<Navigate to="/vehicles" replace />} />
    </Routes>
  );
}

export default App;
