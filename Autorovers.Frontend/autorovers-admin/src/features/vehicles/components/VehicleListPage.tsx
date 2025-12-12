// src/features/vehicles/components/VehicleListPage.tsx
import { useState, useMemo } from "react";
import { useVehicles } from "../hooks/useVehicles";
import { VehicleTable } from "./VehicleTable";
import type { Vehicle } from "../types";
import { VehicleForm } from "./VehicleForm";
import {
  createVehicle,
  updateVehicle,
  deleteVehicleById,
  getVehicleWithDetails,
  updateVehicleDetails,
} from "../api";

type SortKey =
  | "brand"
  | "model"
  | "variant"
  | "year"
  | "price"
  | "category"
  | "transmission"
  | "slug";

type TypeFilter = "all" | "bike" | "car";

export function VehicleListPage() {
  const { vehicles, loading, error, reload } = useVehicles();

  // search
  const [query, setQuery] = useState("");

  // filters
  const [fType, setFType] = useState<TypeFilter>("all");
  const [fBrand, setFBrand] = useState("all");
  const [fTransmission, setFTransmission] = useState("all");
  const [fYear, setFYear] = useState("all");

  // sort
  const [sortBy, setSortBy] = useState<SortKey>("brand");
  const [sortDir, setSortDir] = useState<"asc" | "desc">("asc");

  // panel
  const [isPanelOpen, setIsPanelOpen] = useState(false);
  const [mode, setMode] = useState<"create" | "edit">("create");
  const [editing, setEditing] = useState<Vehicle | null>(null);
  const [actionError, setActionError] = useState<string | null>(null);
  const [editLoading, setEditLoading] = useState(false);

  const handleSortChange = (key: string) => {
    const typedKey = key as SortKey;

    setSortBy((prevSortBy) => {
      setSortDir((prevSortDir) => {
        if (prevSortBy === typedKey) {
          return prevSortDir === "asc" ? "desc" : "asc";
        }
        return "asc";
      });
      return typedKey;
    });
  };

  const brandOptions = useMemo(() => {
    const set = new Set<string>();
    vehicles.forEach((v) => v.brand && set.add(v.brand));
    return ["all", ...Array.from(set).sort((a, b) => a.localeCompare(b))];
  }, [vehicles]);

  const transmissionOptions = useMemo(() => {
    const set = new Set<string>();
    vehicles.forEach((v) => v.transmission && set.add(v.transmission));
    return ["all", ...Array.from(set).sort((a, b) => a.localeCompare(b))];
  }, [vehicles]);

  const yearOptions = useMemo(() => {
    const set = new Set<number>();
    vehicles.forEach((v) => v.year && set.add(v.year));
    return ["all", ...Array.from(set).sort((a, b) => a - b).map(String)];
  }, [vehicles]);

  const visibleVehicles = useMemo(() => {
    const q = query.trim().toLowerCase();

    let filtered = [...vehicles];

    // Type filter (uses vehicleType if present, else fallback to category guess)
    if (fType !== "all") {
      filtered = filtered.filter((v) => {
        const t = (v.vehicleType ?? "").toLowerCase();
        if (t) return t === fType;

        // fallback guess if vehicleType not set
        const c = (v.category ?? "").toLowerCase();
        const looksBike =
          ["sport", "commuter", "cruiser", "tourer", "off-road", "scooter", "ev bike"].includes(c);
        const looksCar =
          ["hatchback", "sedan", "suv", "muv", "coupe", "ev car"].includes(c);

        return fType === "bike" ? looksBike : looksCar;
      });
    }

    // Brand
    if (fBrand !== "all") filtered = filtered.filter((v) => v.brand === fBrand);

    // Transmission
    if (fTransmission !== "all")
      filtered = filtered.filter((v) => v.transmission === fTransmission);

    // Year
    if (fYear !== "all")
      filtered = filtered.filter((v) => String(v.year ?? "") === fYear);

    // Search (brand/model/variant/slug)
    if (q) {
      filtered = filtered.filter((v: Vehicle) => {
        const hay = `${v.brand ?? ""} ${v.model ?? ""} ${v.variant ?? ""} ${v.slug ?? ""}`.toLowerCase();
        return hay.includes(q);
      });
    }

    // Sort
    const sorted = [...filtered].sort((a, b) => {
      const dir = sortDir === "asc" ? 1 : -1;

      const getVal = (v: Vehicle): string | number => {
        switch (sortBy) {
          case "brand":
            return v.brand ?? "";
          case "model":
            return v.model ?? "";
          case "variant":
            return v.variant ?? "";
          case "year":
            return v.year ?? 0;
          case "price":
            return v.price ?? 0;
          case "category":
            return v.category ?? "";
          case "transmission":
            return v.transmission ?? "";
          case "slug":
            return v.slug ?? "";
          default:
            return "";
        }
      };

      const av = getVal(a);
      const bv = getVal(b);

      if (typeof av === "number" && typeof bv === "number") {
        return (av - bv) * dir;
      }
      return String(av).localeCompare(String(bv)) * dir;
    });

    return sorted;
  }, [vehicles, query, fType, fBrand, fTransmission, fYear, sortBy, sortDir]);

  function openCreate() {
    setMode("create");
    setEditing(null);
    setIsPanelOpen(true);
    setActionError(null);
  }

  async function openEdit(row: Vehicle) {
    try {
      setMode("edit");
      setIsPanelOpen(true);
      setActionError(null);
      setEditLoading(true);

      const full: any = await getVehicleWithDetails(row.id);
      const d = full.details ?? {};

      const merged: Vehicle = {
        id: full.id ?? row.id,
        vehicleType: full.vehicleType ?? row.vehicleType ?? "",
        brand: full.brand ?? row.brand ?? "",
        model: full.model ?? row.model ?? "",
        variant: full.variant ?? row.variant ?? "",
        year: full.year ?? row.year ?? new Date().getFullYear(),
        price: full.price ?? row.price ?? 0,
        category: full.category ?? row.category ?? "",
        transmission: full.transmission ?? row.transmission ?? "",
        slug: full.slug ?? row.slug ?? "",
        imageUrl: full.imageUrl ?? row.imageUrl ?? "",

        engineType: d.engineType ?? "",
        specification: d.specification ?? "",
        inductionType: d.inductionType ?? "",
        power: d.power ?? 0,
        powerRpm: d.powerRpm ?? 0,
        torque: d.torque ?? 0,
        torqueRpm: d.torqueRpm ?? 0,
        emission: d.emission ?? "",
        mileage: d.mileage ?? 0,
        autoStartStop: d.autoStartStop ?? "",
        range: d.range ?? 0,

        length: d.length ?? 0,
        width: d.width ?? 0,
        height: d.height ?? 0,
        weight: d.weight ?? 0,
        groundClearance: d.groundClearance ?? 0,
        wheelBase: d.wheelBase ?? 0,

        personCapacity: d.personCapacity ?? 0,
        rows: d.rows ?? 0,
        doors: d.doors ?? 0,
        bootSpace: d.bootSpace ?? 0,
        tankSize: d.tankSize ?? 0,

        frontType: d.frontType ?? "",
        backType: d.backType ?? "",
        frontBrake: d.frontBrake ?? "",
        backBrake: d.backBrake ?? "",
        poweredSteering: d.poweredSteering ?? "",
        tyreSizeFront: d.tyreSizeFront ?? "",
        tyreSizeBack: d.tyreSizeBack ?? "",
        tyreType: d.tyreType ?? "",
        wheelMaterial: d.wheelMaterial ?? "",
        spare: d.spare ?? "",
      };

      setEditing(merged);
    } catch (err: any) {
      console.error(err);
      setActionError(err.message ?? "Failed to load vehicle details");
    } finally {
      setEditLoading(false);
    }
  }

  function closePanel() {
    setIsPanelOpen(false);
    setEditing(null); // keep this; do NOT reset filters/sort/search
  }

  async function handleSubmit(data: Vehicle) {
    try {
      setActionError(null);

      if (mode === "create") {
        await createVehicle(data);
      } else if (mode === "edit" && editing?.id) {
        await updateVehicle(editing.id, data);

        await updateVehicleDetails(editing.id, {
          engineType: data.engineType,
          specification: data.specification,
          inductionType: data.inductionType,
          power: data.power,
          powerRpm: data.powerRpm,
          torque: data.torque,
          torqueRpm: data.torqueRpm,
          emission: data.emission,
          mileage: data.mileage,
          autoStartStop: data.autoStartStop,
          range: data.range,

          length: data.length,
          width: data.width,
          height: data.height,
          groundClearance: data.groundClearance,
          wheelBase: data.wheelBase,

          personCapacity: data.personCapacity,
          rows: data.rows,
          doors: data.doors,
          bootSpace: data.bootSpace,
          tankSize: data.tankSize,

          frontType: data.frontType,
          backType: data.backType,
          frontBrake: data.frontBrake,
          backBrake: data.backBrake,
          poweredSteering: data.poweredSteering,
          tyreSizeFront: data.tyreSizeFront,
          tyreSizeBack: data.tyreSizeBack,
          tyreType: data.tyreType,
          wheelMaterial: data.wheelMaterial,
          spare: data.spare,
        });
      }

      await reload();
      closePanel();
    } catch (err: any) {
      console.error(err);
      setActionError(err.message ?? "Action failed");
    }
  }

  async function handleDelete(id: number) {
    if (!confirm("Delete this vehicle?")) return;
    try {
      setActionError(null);
      await deleteVehicleById(id);
      await reload();
    } catch (err: any) {
      console.error(err);
      setActionError(err.message ?? "Delete failed");
    }
  }

  const hasActiveFilters =
    query.trim() ||
    fType !== "all" ||
    fBrand !== "all" ||
    fTransmission !== "all" ||
    fYear !== "all";

  function resetFilters() {
    setQuery("");
    setFType("all");
    setFBrand("all");
    setFTransmission("all");
    setFYear("all");
  }

  if (loading) return <p style={{ padding: "2rem" }}>Loading...</p>;
  if (error) return <p style={{ padding: "2rem", color: "red" }}>Error: {error}</p>;

  return (
    <div className="admin-main">
      <div className="page-header">
        <div>
          <h1 className="page-title">Admin – Vehicles</h1>
          <p className="page-subtitle">Search, filter, sort and manage catalog.</p>
        </div>
        <button className="btn" onClick={openCreate}>
          + Add Vehicle
        </button>
      </div>

      {/* Filters */}
      <div className="admin-filters">
        <div className="admin-filters-row">
          <input
            type="text"
            placeholder="Search brand / model / variant / slug..."
            value={query}
            onChange={(e) => setQuery(e.target.value)}
            className="search-input"
            aria-label="Search vehicles"
          />

          <select
            className="search-input"
            value={fType}
            onChange={(e) => setFType(e.target.value as TypeFilter)}
            aria-label="Filter by type"
            style={{ maxWidth: 180 }}
          >
            <option value="all">All types</option>
            <option value="bike">Bike</option>
            <option value="car">Car</option>
          </select>

          <select
            className="search-input"
            value={fBrand}
            onChange={(e) => setFBrand(e.target.value)}
            aria-label="Filter by brand"
            style={{ maxWidth: 220 }}
          >
            {brandOptions.map((b) => (
              <option key={b} value={b}>
                {b === "all" ? "All brands" : b}
              </option>
            ))}
          </select>

          <select
            className="search-input"
            value={fTransmission}
            onChange={(e) => setFTransmission(e.target.value)}
            aria-label="Filter by transmission"
            style={{ maxWidth: 220 }}
          >
            {transmissionOptions.map((t) => (
              <option key={t} value={t}>
                {t === "all" ? "All transmissions" : t}
              </option>
            ))}
          </select>

          <select
            className="search-input"
            value={fYear}
            onChange={(e) => setFYear(e.target.value)}
            aria-label="Filter by year"
            style={{ maxWidth: 160 }}
          >
            {yearOptions.map((y) => (
              <option key={y} value={y}>
                {y === "all" ? "All years" : y}
              </option>
            ))}
          </select>

          <button
            className="btn btn-ghost"
            onClick={resetFilters}
            disabled={!hasActiveFilters}
            type="button"
          >
            Reset
          </button>
        </div>
      </div>

      {actionError && <div className="alert alert-error">{actionError}</div>}

      {/* Empty state for filters */}
      {visibleVehicles.length === 0 ? (
        <div className="empty-state">
          <h3>No vehicles match your filters</h3>
          <p className="muted">Try clearing filters or changing your search.</p>
          <button className="btn" onClick={resetFilters}>
            Clear filters
          </button>
        </div>
      ) : (
        <VehicleTable
          vehicles={visibleVehicles}
          sortBy={sortBy}
          sortDir={sortDir}
          onSortChange={handleSortChange}
          onEdit={openEdit}
          onDelete={handleDelete}
        />
      )}

      <div className={`slide-over ${isPanelOpen ? "is-open" : ""}`}>
        <div className="slide-over-backdrop" onClick={closePanel} />
        <div className="slide-over-panel">
          {editLoading && (
            <p className="muted" style={{ marginBottom: "0.5rem" }}>
              Loading full specs…
            </p>
          )}
          {isPanelOpen && (
            <VehicleForm
              initial={editing}
              mode={mode}
              onSubmit={handleSubmit}
              onCancel={closePanel}
            />
          )}
        </div>
      </div>
    </div>
  );
}
