import "./styles/admin.css";
import { VehicleListPage } from "./features/vehicles/components/VehicleListPage";

function App() {
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

export default App;
