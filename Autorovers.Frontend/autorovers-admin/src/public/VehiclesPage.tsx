// src/public/VehiclesPage.tsx
import { useEffect, useState, useMemo } from "react";
import { Link } from "react-router-dom";
import type { Vehicle } from "../features/vehicles/types";
import { getPublicVehicles } from "../features/vehicles/api";

export function VehiclesPage() {
  const [vehicles, setVehicles] = useState<Vehicle[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // UI filters
  const [search, setSearch] = useState("");
  const [category, setCategory] = useState("all");
  const [sortBy, setSortBy] = useState("priceAsc");

  useEffect(() => {
    (async () => {
      try {
        setLoading(true);
        const data = await getPublicVehicles();
        setVehicles(data);
      } catch (err: any) {
        setError(err.message ?? "Failed to load vehicles");
      } finally {
        setLoading(false);
      }
    })();
  }, []);

  // ---- FILTERING & SORTING ----
  const filteredVehicles = useMemo(() => {
    let list = [...vehicles];

    // Search
    if (search.trim()) {
      const s = search.toLowerCase();
      list = list.filter(
        (v) =>
          v.brand.toLowerCase().includes(s) ||
          v.model.toLowerCase().includes(s) ||
          (v.variant ?? "").toLowerCase().includes(s)
      );
    }

    // Category filter
    if (category !== "all") {
      list = list.filter((v) => v.category.toLowerCase() === category);
    }

    // Sorting
    list.sort((a, b) => {
      switch (sortBy) {
        case "priceAsc":
          return a.price - b.price;
        case "priceDesc":
          return b.price - a.price;
        case "yearAsc":
          return a.year - b.year;
        case "yearDesc":
          return b.year - a.year;
        default:
          return 0;
      }
    });

    return list;
  }, [vehicles, search, category, sortBy]);

  if (loading) return <div className="public-page">Loading vehicles…</div>;
  if (error) return <div className="public-page error">{error}</div>;

  return (
    <div className="public-page">
      <h1 className="public-title">Autorovers Catalog</h1>
      <p className="public-subtitle">Browse vehicles and discover details.</p>

      {/* FILTER BAR */}
      <div className="catalog-controls">
        <input
          type="text"
          placeholder="Search by brand, model, variant..."
          className="catalog-search"
          value={search}
          onChange={(e) => setSearch(e.target.value)}
        />

        <select
          className="catalog-select"
          value={category}
          onChange={(e) => setCategory(e.target.value)}
        >
          <option value="all">All categories</option>
          <option value="bike">Bikes</option>
          <option value="car">Cars</option>
          <option value="scooter">Scooters</option>
          <option value="adventure">Adventure</option>
          <option value="sports">Sports</option>
        </select>

        <select
          className="catalog-select"
          value={sortBy}
          onChange={(e) => setSortBy(e.target.value)}
        >
          <option value="priceAsc">Price: Low → High</option>
          <option value="priceDesc">Price: High → Low</option>
          <option value="yearAsc">Year: Old → New</option>
          <option value="yearDesc">Year: New → Old</option>
        </select>
      </div>

      {/* VEHICLE GRID */}
      <div className="vehicle-grid">
        {filteredVehicles.length === 0 && (
          <div className="no-results">No vehicles match your filters.</div>
        )}

        {filteredVehicles.map((v) => {
          const powerText = v.power ? `${v.power} hp` : "—";
          const torqueText = v.torque ? `${v.torque} Nm` : "—";
          const priceText =
            v.price && v.price > 0
              ? `₹ ${v.price.toLocaleString("en-IN")}`
              : "Price unavailable";

          return (
            <Link key={v.id} to={`/vehicles/${v.slug}`} className="vehicle-card">
              <div className="vehicle-card-header">
                <h2 className="vehicle-card-title">
                  {v.brand} {v.model}
                </h2>
                <p className="vehicle-card-variant">{v.variant || "—"}</p>
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
