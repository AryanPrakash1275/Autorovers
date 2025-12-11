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

export function VehicleListPage() {
  const { vehicles, loading, error, reload } = useVehicles();
  const [query, setQuery] = useState("");
  const [sortBy, setSortBy] = useState<SortKey>("brand");
  const [sortDir, setSortDir] = useState<"asc" | "desc">("asc");

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

  async function openEdit(row: Vehicle) {
    try {
      setMode("edit");
      setIsPanelOpen(true);
      setActionError(null);
      setEditLoading(true);

      const full: any = await getVehicleWithDetails(row.id);

      const d = full.details ?? {};

      const merged: Vehicle = {
        id: full.id,
        vehicleType: full.vehicleType ?? "",

        brand: full.brand,
        model: full.model,
        variant: full.variant,
        year: full.year,
        price: full.price,
        category: full.category,
        transmission: full.transmission,
        slug: full.slug,
        imageUrl: full.imageUrl ?? "",

        // specs
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
    setEditing(null);
  }

  async function handleSubmit(data: Vehicle) {
    try {
      setActionError(null);

      if (mode === "create") {
        await createVehicle(data);
      } else if (mode === "edit" && editing?.id) {
        // 1) Update core vehicle
        await updateVehicle(editing.id, data);

        // 2) Update details via dedicated endpoint
        await updateVehicleDetails(editing.id, {
          engineType: data.engineType,
          specification: data.specification,
          inductionType: data.inductionType,
          power: data.power,
          torque: data.torque,
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
