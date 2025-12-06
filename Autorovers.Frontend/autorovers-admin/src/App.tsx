import { BrowserRouter, Routes, Route, Navigate, Link } from "react-router-dom";
import { VehiclesPage } from "./public/VehiclesPage";
import { VehicleDetailsPage } from "./public/VehicleDetailsPage";
import { VehicleListPage } from "./features/vehicles/components/VehicleListPage";

import "./styles/admin.css";
import "./styles/public.css";

type LayoutProps = {
  children: React.ReactNode;
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
          <Link to="/admin" className="public-nav-link">
            Admin
          </Link>
        </nav>
      </header>

      <main className="public-main">
        {children}
      </main>
    </div>
  );
}

function AdminLayout() {
  return (
    <div className="admin-root">
      <header className="admin-header">
        <h1>Autorovers Admin</h1>
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
    <BrowserRouter>
      <Routes>
        {/* Public catalog + details wrapped in PublicLayout with navbar */}
        <Route
          path="/vehicles"
          element={
            <PublicLayout>
              <VehiclesPage />
            </PublicLayout>
          }
        />
        <Route
          path="/vehicles/:id"
          element={
            <PublicLayout>
              <VehicleDetailsPage />
            </PublicLayout>
          }
        />

        {/* Admin dashboard */}
        <Route path="/admin" element={<AdminLayout />} />

        {/* Default: send everything else to public catalog */}
        <Route path="*" element={<Navigate to="/vehicles" replace />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
