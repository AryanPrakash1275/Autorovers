// src/public/VehiclesPage.tsx
import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import type { Vehicle } from "../features/vehicles/types";
import { getPublicVehicles } from "../features/vehicles/api";

export function VehiclesPage() {
  const [vehicles, setVehicles] = useState<Vehicle[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    (async () => {
      try {
        setLoading(true);
        setError(null);
        const data = await getPublicVehicles();
        setVehicles(data);
      } catch (err: any) {
        console.error(err);
        setError(err.message ?? "Failed to load vehicles");
      } finally {
        setLoading(false);
      }
    })();
  }, []);

  if (loading) {
    return <div className="public-page">Loading vehicles…</div>;
  }

  if (error) {
    return (
      <div className="public-page" style={{ color: "red" }}>
        {error}
      </div>
    );
  }

  if (!vehicles.length) {
    return <div className="public-page">No vehicles yet.</div>;
  }

  return (
    <div className="public-page">
      <h1 className="public-title">Autorovers Catalog</h1>
      <p className="public-subtitle">
        Browse vehicles and open any for full specs.
      </p>

      <div className="vehicle-grid">
        {vehicles.map((v) => {
          const powerText = v.power ? `${v.power} hp` : "—";
          const torqueText = v.torque ? `${v.torque} Nm` : "—";
          const priceText =
            v.price && v.price > 0
              ? `₹ ${v.price.toLocaleString("en-IN")}`
              : "Price unavailable";

          return (
            <Link
              key={v.id}
              to={`/vehicles/${v.slug}`}   // 👈 IMPORTANT: use slug, not id
              className="vehicle-card"
            >
              <div className="vehicle-card-header">
                <h2 className="vehicle-card-title">
                  {v.brand} {v.model}
                </h2>
                <p className="vehicle-card-variant">
                  {v.variant || "—"}
                </p>
              </div>

              <div className="vehicle-card-specs">
                <span className="vehicle-card-spec">
                  {powerText} · {torqueText}
                </span>
                <span className="vehicle-card-price">{priceText}</span>
              </div>

              <div className="vehicle-card-footer">
                <span className="vehicle-card-tag">{v.year}</span>
                <span className="vehicle-card-tag">{v.category}</span>
                <span className="vehicle-card-tag">{v.transmission}</span>
              </div>
            </Link>
          );
        })}
      </div>
    </div>
  );
}
