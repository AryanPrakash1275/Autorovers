// src/public/VehiclesPage.tsx
import { useEffect, useState, useMemo } from "react";
import { Link } from "react-router-dom";
import type { VehicleListItem } from "../features/vehicles/types";
import { getPublicVehicles } from "../features/vehicles/api";
import { Footer } from "./Footer";

// Keep in sync with VehicleForm.tsx
const BIKE_CATEGORIES = [
  "Sport",
  "Commuter",
  "Cruiser",
  "Tourer",
  "Off-road",
  "Scooter",
  "EV Bike",
];

const CAR_CATEGORIES = [
  "Hatchback",
  "Sedan",
  "SUV",
  "MUV",
  "Coupe",
  "EV Car",
];

type CategoryFilter = "all" | "bike" | "car";

export function VehiclesPage() {
  const [vehicles, setVehicles] = useState<VehicleListItem[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // UI filters
  const [search, setSearch] = useState("");
  const [category, setCategory] = useState<CategoryFilter>("all");
  const [sortBy, setSortBy] = useState("priceAsc");

  useEffect(() => {
    (async () => {
      try {
        setLoading(true);
        const data = await getPublicVehicles(); // VehicleListItem[]
        setVehicles(data);
      } catch (err: any) {
        setError(err.message ?? "Failed to load vehicles");
      } finally {
        setLoading(false);
      }
    })();
  }, []);

  function isBikeCategory(cat: string | null | undefined) {
    if (!cat) return false;
    return BIKE_CATEGORIES.includes(cat);
  }

  function isCarCategory(cat: string | null | undefined) {
    if (!cat) return false;
    return CAR_CATEGORIES.includes(cat);
  }

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

    // Category filter: all / bike / car
    if (category === "bike") {
      list = list.filter((v) => isBikeCategory(v.category));
    } else if (category === "car") {
      list = list.filter((v) => isCarCategory(v.category));
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
      {/* ================= HERO ================= */}
      <section className="hero hero-with-image">
        {/* Background image + overlay */}
        <div className="hero-bg">
          <img
            src="https://images.pexels.com/photos/100654/pexels-photo-100654.jpeg?auto=compress&cs=tinysrgb&w=1600"
            alt="Autorovers hero"
            className="hero-bg-image"
          />
          <div className="hero-bg-overlay" />
        </div>

        {/* Foreground content */}
        <div className="hero-layer">
          <div className="hero-copy">
            <h1 className="hero-title">Find the right machine</h1>
            <p className="hero-tagline">
              Clean, structured specs for bikes and cars — no listings, no spam.
            </p>
            <p className="hero-subtext">
              Power, torque, mileage and dimensions in one place so you can
              compare without noise.
            </p>
          </div>

          {/* Search + filters pinned near bottom of hero */}
          <div className="hero-search-card">
            <div className="catalog-controls catalog-controls-hero">
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
                onChange={(e) => setCategory(e.target.value as CategoryFilter)}
              >
                <option value="all">All vehicles</option>
                <option value="bike">Bikes</option>
                <option value="car">Cars</option>
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
          </div>
        </div>
      </section>

      {/* ================= CATALOG ================= */}
      <section className="catalog-section">
        <h2 className="catalog-title">All vehicles</h2>

        <div className="vehicle-grid">
          {filteredVehicles.length === 0 && (
            <div className="no-results">No vehicles match your filters.</div>
          )}

          {filteredVehicles.map((v) => {
            const powerText = v.power ? `${v.power} bhp` : "—";
            const torqueText = v.torque ? `${v.torque} Nm` : "—";
            const priceText =
              v.price && v.price > 0
                ? `₹ ${v.price.toLocaleString("en-IN")}`
                : "Price unavailable";

            return (
              <Link
                key={v.id}
                to={`/vehicles/${encodeURIComponent(v.slug)}`}
                className="vehicle-card"
              >
                {v.imageUrl && (
                  <div className="vehicle-card-image-wrapper">
                    <img
                      src={v.imageUrl}
                      alt={`${v.brand} ${v.model}`}
                      className="vehicle-card-image"
                      onError={(e) =>
                        ((e.target as HTMLImageElement).src =
                          "https://dummyimage.com/600x400/cccccc/000000&text=No+Image")
                      }
                    />
                  </div>
                )}

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
      </section>

      <Footer />
    </div>
  );
}
