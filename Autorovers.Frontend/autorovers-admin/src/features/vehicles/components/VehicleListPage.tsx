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

type VehicleTypeFilter = "all" | "Bike" | "Car";

function uniqSorted(list: string[]) {
  return Array.from(new Set(list.map((x) => x.trim()).filter(Boolean))).sort(
    (a, b) => a.localeCompare(b)
  );
}

export function VehicleListPage() {
  const { vehicles, loading, error, reload } = useVehicles();

  // Search (brand/model/variant/slug)
  const [query, setQuery] = useState("");

  // Sort
  const [sortBy, setSortBy] = useState<SortKey>("brand");
  const [sortDir, setSortDir] = useState<"asc" | "desc">("asc");

  // Filters
  const [typeFilter, setTypeFilter] = useState<VehicleTypeFilter>("all");
  const [brandFilter, setBrandFilter] = useState<string>("all");
  const [transFilter, setTransFilter] = useState<string>("all");
  const [yearFilter, setYearFilter] = useState<string>("all");

  // Panel
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

  // Build filter option lists from data (client-side)
  const brandOptions = useMemo(() => {
    return uniqSorted(vehicles.map((v) => v.brand ?? ""));
  }, [vehicles]);

  const transmissionOptions = useMemo(() => {
    return uniqSorted(vehicles.map((v) => v.transmission ?? ""));
  }, [vehicles]);

  const yearOptions = useMemo(() => {
    const years = Array.from(new Set(vehicles.map((v) => v.year))).filter(
      (y) => typeof y === "number" && !Number.isNaN(y)
    );
    years.sort((a, b) => b - a); // newest first
    return years.map(String);
  }, [vehicles]);

  const visibleVehicles = useMemo(() => {
    const q = query.trim().toLowerCase();

    let filtered = vehicles;

    // Type filter (uses vehicleType; if missing, allow it through when filter=all)
    if (typeFilter !== "all") {
      filtered = filtered.filter(
        (v) => (v.vehicleType ?? "") === typeFilter
      );
    }

    // Brand filter
    if (brandFilter !== "all") {
      filtered = filtered.filter((v) => (v.brand ?? "") === brandFilter);
    }

    // Transmission filter
    if (transFilter !== "all") {
      filtered = filtered.filter(
        (v) => (v.transmission ?? "") === transFilter
      );
    }

    // Year filter
    if (yearFilter !== "all") {
      filtered = filtered.filter((v) => String(v.year) === yearFilter);
    }

    // Search filter (brand/model/variant/slug)
    if (q) {
      filtered = filtered.filter((v: Vehicle) => {
        const brand = (v.brand ?? "").toLowerCase();
        const model = (v.model ?? "").toLowerCase();
        const variant = (v.variant ?? "").toLowerCase();
        const slug = (v.slug ?? "").toLowerCase();
        return (
          brand.includes(q) ||
          model.includes(q) ||
          variant.includes(q) ||
          slug.includes(q)
        );
      });
    }

    // Sort
    const sorted = [...filtered].sort((a, b) => {
      const dir = sortDir === "asc" ? 1 : -1;

      const getVal = (v: Vehicle): string | number => {
        switch (sortBy) {
          case "brand":
            return v.brand;
          case "model":
            return v.model;
          case "variant":
            return v.variant;
          case "year":
            return v.year;
          case "price":
            return v.price;
          case "category":
            return v.category;
          case "transmission":
            return v.transmission;
          case "slug":
            return v.slug;
          default:
            return "";
        }
      };

      const av = getVal(a);
      const bv = getVal(b);

      if (typeof av === "number" && typeof bv === "number") {
        return (av - bv) * dir;
      }

      return String(av ?? "").localeCompare(String(bv ?? "")) * dir;
    });

    return sorted;
  }, [vehicles, query, sortBy, sortDir, typeFilter, brandFilter, transFilter, yearFilter]);

  const hasFilters =
    query.trim() ||
    typeFilter !== "all" ||
    brandFilter !== "all" ||
    transFilter !== "all" ||
    yearFilter !== "all";

  function clearFilters() {
    setQuery("");
    setTypeFilter("all");
    setBrandFilter("all");
    setTransFilter("all");
    setYearFilter("all");
  }

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
        // basics from core table + fallback to row
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

        // engine & performance
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

        // dimensions
        length: d.length ?? 0,
        width: d.width ?? 0,
        height: d.height ?? 0,
        weight: d.weight ?? 0,
        groundClearance: d.groundClearance ?? 0,
        wheelBase: d.wheelBase ?? 0,

        // capacity
        personCapacity: d.personCapacity ?? 0,
        rows: d.rows ?? 0,
        doors: d.doors ?? 0,
        bootSpace: d.bootSpace ?? 0,
        tankSize: d.tankSize ?? 0,

        // tyres & brakes
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
    setEditing(null);
    // IMPORTANT: we do NOT reset query/filters/sort here -> preserves state
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

  if (loading) {
    return <p style={{ padding: "2rem" }}>Loading...</p>;
  }

  if (error) {
    return (
      <p style={{ padding: "2rem", color: "red" }}>
        Error: {error}
      </p>
    );
  }

  return (
    <div className="admin-main">
      <div className="page-header">
        <div>
          <h1 className="page-title">Admin – Vehicles</h1>
          <p className="page-subtitle">
            Search, sort and manage Autorovers vehicle catalog.
          </p>
        </div>
        <button className="btn" onClick={openCreate}>
          + Add Vehicle
        </button>
      </div>

      {/* Search */}
      <div style={{ marginBottom: "0.75rem" }}>
        <input
          type="text"
          placeholder="Search by brand, model, variant, slug..."
          value={query}
          onChange={(e) => setQuery(e.target.value)}
          className="search-input"
        />
      </div>

      {/* Filters row */}
      <div
        style={{
          display: "flex",
          gap: "0.6rem",
          flexWrap: "wrap",
          alignItems: "center",
          marginBottom: "0.75rem",
        }}
      >
        <select
          className="search-input"
          style={{ width: 160 }}
          value={typeFilter}
          onChange={(e) => setTypeFilter(e.target.value as VehicleTypeFilter)}
        >
          <option value="all">All types</option>
          <option value="Bike">Bike</option>
          <option value="Car">Car</option>
        </select>

        <select
          className="search-input"
          style={{ width: 180 }}
          value={brandFilter}
          onChange={(e) => setBrandFilter(e.target.value)}
        >
          <option value="all">All brands</option>
          {brandOptions.map((b) => (
            <option key={b} value={b}>
              {b}
            </option>
          ))}
        </select>

        <select
          className="search-input"
          style={{ width: 180 }}
          value={transFilter}
          onChange={(e) => setTransFilter(e.target.value)}
        >
          <option value="all">All transmissions</option>
          {transmissionOptions.map((t) => (
            <option key={t} value={t}>
              {t}
            </option>
          ))}
        </select>

        <select
          className="search-input"
          style={{ width: 140 }}
          value={yearFilter}
          onChange={(e) => setYearFilter(e.target.value)}
        >
          <option value="all">All years</option>
          {yearOptions.map((y) => (
            <option key={y} value={y}>
              {y}
            </option>
          ))}
        </select>

        {hasFilters && (
          <button className="btn btn-ghost" onClick={clearFilters}>
            Clear filters
          </button>
        )}
      </div>

      {actionError && <div className="alert alert-error">{actionError}</div>}

      {/* Empty state for filters */}
      {visibleVehicles.length === 0 ? (
        <div
          className="alert"
          style={{
            background: "#ffffff",
            border: "1px solid var(--border)",
            boxShadow: "var(--shadow-soft)",
          }}
        >
          <div style={{ fontWeight: 700, marginBottom: 6 }}>
            No vehicles match your filters.
          </div>
          <div className="muted" style={{ marginBottom: 10 }}>
            Try clearing filters or changing your search.
          </div>
          {hasFilters && (
            <button className="btn btn-ghost" onClick={clearFilters}>
              Reset filters
            </button>
          )}
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
