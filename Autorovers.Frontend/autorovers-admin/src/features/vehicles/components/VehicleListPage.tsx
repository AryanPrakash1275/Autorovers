import { useState, useMemo } from "react";
import { useVehicles } from "../hooks/useVehicles";
import { VehicleTable } from "./VehicleTable";
import type { Vehicle } from "../types";
import { VehicleForm } from "./VehicleForm";
import {
  createVehicle,
  updateVehicle,
  deleteVehicleById,
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

export function VehicleListPage() {
  const { vehicles, loading, error, reload } = useVehicles();
  const [query, setQuery] = useState("");
  const [sortBy, setSortBy] = useState<SortKey>("brand");
  const [sortDir, setSortDir] = useState<"asc" | "desc">("asc");

  const [isPanelOpen, setIsPanelOpen] = useState(false);
  const [mode, setMode] = useState<"create" | "edit">("create");
  const [editing, setEditing] = useState<Vehicle | null>(null);
  const [actionError, setActionError] = useState<string | null>(null);

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

  const visibleVehicles = useMemo(() => {
    const q = query.trim().toLowerCase();

    let filtered = vehicles;
    if (q) {
      filtered = vehicles.filter((v: Vehicle) => {
        return (
          v.brand.toLowerCase().includes(q) ||
          v.model.toLowerCase().includes(q) ||
          v.variant.toLowerCase().includes(q) ||
          v.slug.toLowerCase().includes(q)
        );
      });
    }

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

      return String(av).localeCompare(String(bv)) * dir;
    });

    return sorted;
  }, [vehicles, query, sortBy, sortDir]);

  function openCreate() {
    setMode("create");
    setEditing(null);
    setIsPanelOpen(true);
    setActionError(null);
  }

  function openEdit(v: Vehicle) {
    setMode("edit");
    setEditing(v);
    setIsPanelOpen(true);
    setActionError(null);
  }

  function closePanel() {
    setIsPanelOpen(false);
    setEditing(null);
  }

  async function handleSubmit(data: Vehicle) {
    try {
      setActionError(null);
      if (mode === "create") {
        await createVehicle(data);
      } else if (mode === "edit" && editing?.id) {
        await updateVehicle(editing.id, data);
      }
      await reload();
      closePanel();
    } catch (err: any) {
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

      <div style={{ marginBottom: "1rem" }}>
        <input
          type="text"
          placeholder="Search by brand, model, variant, slug..."
          value={query}
          onChange={(e) => setQuery(e.target.value)}
          className="search-input"
        />
      </div>

      {actionError && (
        <div className="alert alert-error">{actionError}</div>
      )}

      <VehicleTable
        vehicles={visibleVehicles}
        sortBy={sortBy}
        sortDir={sortDir}
        onSortChange={handleSortChange}
        onEdit={openEdit}
        onDelete={handleDelete}
      />

      {/* Slide-over */}
      <div className={`slide-over ${isPanelOpen ? "is-open" : ""}`}>
        <div className="slide-over-backdrop" onClick={closePanel} />
        <div className="slide-over-panel">
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
