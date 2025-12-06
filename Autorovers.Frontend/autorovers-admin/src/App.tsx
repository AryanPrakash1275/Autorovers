import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import { VehiclesPage } from "./public/VehiclesPage";
import { VehicleDetailsPage } from "./public/VehicleDetailsPage";
import { VehicleListPage } from "./features/vehicles/components/VehicleListPage";

import "./styles/admin.css";
import "./styles/public.css"; // make sure this path matches where you created it

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
        {/* Public catalog */}
        <Route path="/vehicles" element={<VehiclesPage />} />
        <Route path="/vehicles/:id" element={<VehicleDetailsPage />} />

        {/* Admin dashboard */}
        <Route path="/admin" element={<AdminLayout />} />

        {/* Default redirect: send root to public catalog */}
        <Route path="*" element={<Navigate to="/vehicles" replace />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
