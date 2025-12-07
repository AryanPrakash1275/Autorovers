import { useState, useEffect } from "react";
import type { Vehicle } from "../types";

type Props = {
  initial?: Vehicle | null;
  mode: "create" | "edit";
  onSubmit: (data: Vehicle) => Promise<void>;
  onCancel: () => void;
};

type Tab = "basics" | "specs";

export function VehicleForm({ initial, mode, onSubmit, onCancel }: Props) {
  const [form, setForm] = useState<Vehicle>({
    id: initial?.id ?? 0,
    brand: initial?.brand ?? "",
    model: initial?.model ?? "",
    variant: initial?.variant ?? "",
    year: initial?.year ?? new Date().getFullYear(),
    price: initial?.price ?? 0,
    category: initial?.category ?? "",
    transmission: initial?.transmission ?? "",
    slug: initial?.slug ?? "",
    imageUrl: initial?.imageUrl?? "",

    // Engine & performance
    engineType: initial?.engineType ?? "",
    specification: initial?.specification ?? "",
    inductionType: initial?.inductionType ?? "",
    power: initial?.power ?? 0,
    torque: initial?.torque ?? 0,
    emission: initial?.emission ?? "",
    mileage: initial?.mileage ?? 0,
    autoStartStop: initial?.autoStartStop ?? "",
    range: initial?.range ?? 0,

    // Dimensions & weight
    length: initial?.length ?? 0,
    width: initial?.width ?? 0,
    height: initial?.height ?? 0,
    weight: initial?.weight ?? 0,
    groundClearance: initial?.groundClearance ?? 0,
    wheelBase: initial?.wheelBase ?? 0,

    // Capacity
    personCapacity: initial?.personCapacity ?? 0,
    rows: initial?.rows ?? 0,
    doors: initial?.doors ?? 0,
    bootSpace: initial?.bootSpace ?? 0,
    tankSize: initial?.tankSize ?? 0,

    // Tyres, brakes & steering
    frontTyre: initial?.frontTyre ?? "",
    backType: initial?.backType ?? "",
    frontBrake: initial?.frontBrake ?? "",
    backBrake: initial?.backBrake ?? "",
    poweredSteering: initial?.poweredSteering ?? "",
    tyreSizeFront: initial?.tyreSizeFront ?? "",
    tyreSizeBack: initial?.tyreSizeBack ?? "",
    tyreType: initial?.tyreType ?? "",
    wheelMaterial: initial?.wheelMaterial ?? "",
    spare: initial?.spare ?? "",
  });

  const [submitting, setSubmitting] = useState(false);
  const [activeTab, setActiveTab] = useState<Tab>("basics");

  // When initial changes (edit vs create), sync form
  useEffect(() => {
    if (initial) {
      setForm((prev) => ({
        ...prev,
        ...initial,
      }));
    }
  }, [initial]);

  function handleChange(
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) {
    const { name, value } = e.target;
    setForm((prev) => {
      // numeric fields
      const numberFields = new Set([
        "year",
        "price",
        "power",
        "torque",
        "mileage",
        "range",
        "length",
        "width",
        "height",
        "weight",
        "groundClearance",
        "wheelBase",
        "personCapacity",
        "rows",
        "doors",
        "bootSpace",
        "tankSize",
      ]);

      if (numberFields.has(name)) {
        return {
          ...prev,
          [name]: value === "" ? 0 : Number(value),
        };
      }

      return { ...prev, [name]: value };
    });
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    if (submitting) return;
    setSubmitting(true);

    try {
      await onSubmit(form);
    } finally {
      setSubmitting(false);
    }
  }

  return (
    <form className="form slide-form" onSubmit={handleSubmit}>
      <div className="slide-form-header">
        <h2>{mode === "create" ? "Add Vehicle" : "Edit Vehicle"}</h2>
        <button type="button" className="icon-btn" onClick={onCancel}>
          ✕
        </button>
      </div>

      {/* Tabs */}
      <div className="form-tabs">
        <button
          type="button"
          className={`form-tab ${activeTab === "basics" ? "is-active" : ""}`}
          onClick={() => setActiveTab("basics")}
        >
          Basics
        </button>
        <button
          type="button"
          className={`form-tab ${activeTab === "specs" ? "is-active" : ""}`}
          onClick={() => setActiveTab("specs")}
        >
          Specifications
        </button>
      </div>

      <div className="slide-form-body">
        {activeTab === "basics" && (
          <div className="form-grid">
            <div className="field">
              <label>Brand</label>
              <input
                name="brand"
                value={form.brand}
                onChange={handleChange}
              />
            </div>

            <div className="field">
              <label>Model</label>
              <input
                name="model"
                value={form.model}
                onChange={handleChange}
              />
            </div>

            <div className="field">
              <label>Variant</label>
              <input
                name="variant"
                value={form.variant}
                onChange={handleChange}
              />
            </div>

            <div className="field">
              <label>Year</label>
              <input
                type="number"
                name="year"
                value={form.year}
                onChange={handleChange}
              />
            </div>

            <div className="field">
              <label>Price (₹)</label>
              <input
                type="number"
                name="price"
                value={form.price}
                onChange={handleChange}
              />
            </div>

            <div className="field">
              <label>Category</label>
              <input
                name="category"
                value={form.category}
                onChange={handleChange}
              />
            </div>

            <div className="field">
              <label>Transmission</label>
              <input
                name="transmission"
                value={form.transmission}
                onChange={handleChange}
              />
            </div>

            <div className="field">
              <label>Image URL</label>
              <input
                name="imageUrl"
                value={form.imageUrl ?? ""}   // ✅ force null/undefined → ""
                onChange={handleChange}
                placeholder="https://example.com/bike.jpg"
              />
            </div>

            <div className="field">
              <label>Slug (optional)</label>
              <input
                name="slug"
                value={form.slug}
                onChange={handleChange}
                placeholder="Leave blank for auto"
              />
            </div>
          </div>
        )}

        {activeTab === "specs" && (
          <>
            {/* ENGINE & PERFORMANCE */}
            <h3 className="form-section-title">Engine & Performance</h3>
            <div className="form-grid">
              <div className="field">
                <label>Engine type</label>
                <input
                  name="engineType"
                  value={form.engineType}
                  onChange={handleChange}
                />
              </div>
              <div className="field">
                <label>Specification</label>
                <input
                  name="specification"
                  value={form.specification}
                  onChange={handleChange}
                />
              </div>
              <div className="field">
                <label>Induction</label>
                <input
                  name="inductionType"
                  value={form.inductionType}
                  onChange={handleChange}
                />
              </div>
              <div className="field">
                <label>Power (hp)</label>
                <input
                  type="number"
                  name="power"
                  value={form.power}
                  onChange={handleChange}
                />
              </div>
              <div className="field">
                <label>Torque (Nm)</label>
                <input
                  type="number"
                  name="torque"
                  value={form.torque}
                  onChange={handleChange}
                />
              </div>
              <div className="field">
                <label>Emission</label>
                <input
                  name="emission"
                  value={form.emission}
                  onChange={handleChange}
                />
              </div>
              <div className="field">
                <label>Mileage (kmpl)</label>
                <input
                  type="number"
                  name="mileage"
                  value={form.mileage}
                  onChange={handleChange}
                />
              </div>
              <div className="field">
                <label>Auto Start/Stop</label>
                <input
                  name="autoStartStop"
                  value={form.autoStartStop}
                  onChange={handleChange}
                />
              </div>
              <div className="field">
                <label>Range (km)</label>
                <input
                  type="number"
                  name="range"
                  value={form.range}
                  onChange={handleChange}
                />
              </div>
            </div>

            {/* DIMENSIONS & WEIGHT */}
            <h3 className="form-section-title">Dimensions & Weight</h3>
            <div className="form-grid">
              <div className="field">
                <label>Length (mm)</label>
                <input
                  type="number"
                  name="length"
                  value={form.length}
                  onChange={handleChange}
                />
              </div>
              <div className="field">
                <label>Width (mm)</label>
                <input
                  type="number"
                  name="width"
                  value={form.width}
                  onChange={handleChange}
                />
              </div>
              <div className="field">
                <label>Height (mm)</label>
                <input
                  type="number"
                  name="height"
                  value={form.height}
                  onChange={handleChange}
                />
              </div>
              <div className="field">
                <label>Wheelbase (mm)</label>
                <input
                  type="number"
                  name="wheelBase"
                  value={form.wheelBase}
                  onChange={handleChange}
                />
              </div>
              <div className="field">
                <label>Ground clearance (mm)</label>
                <input
                  type="number"
                  name="groundClearance"
                  value={form.groundClearance}
                  onChange={handleChange}
                />
              </div>
              <div className="field">
                <label>Kerb weight (kg)</label>
                <input
                  type="number"
                  name="weight"
                  value={form.weight}
                  onChange={handleChange}
                />
              </div>
            </div>

            {/* CAPACITY & LAYOUT */}
            <h3 className="form-section-title">Capacity & Layout</h3>
            <div className="form-grid">
              <div className="field">
                <label>Seating (persons)</label>
                <input
                  type="number"
                  name="personCapacity"
                  value={form.personCapacity}
                  onChange={handleChange}
                />
              </div>
              <div className="field">
                <label>Rows</label>
                <input
                  type="number"
                  name="rows"
                  value={form.rows}
                  onChange={handleChange}
                />
              </div>
              <div className="field">
                <label>Doors</label>
                <input
                  type="number"
                  name="doors"
                  value={form.doors}
                  onChange={handleChange}
                />
              </div>
              <div className="field">
                <label>Boot space (L)</label>
                <input
                  type="number"
                  name="bootSpace"
                  value={form.bootSpace}
                  onChange={handleChange}
                />
              </div>
              <div className="field">
                <label>Fuel tank (L)</label>
                <input
                  type="number"
                  name="tankSize"
                  value={form.tankSize}
                  onChange={handleChange}
                />
              </div>
            </div>

            {/* TYRES, BRAKES & STEERING */}
            <h3 className="form-section-title">Tyres, Brakes & Steering</h3>
            <div className="form-grid">
              <div className="field">
                <label>Front tyre</label>
                <input
                  name="frontTyre"
                  value={form.frontTyre}
                  onChange={handleChange}
                />
              </div>
              <div className="field">
                <label>Rear / back type</label>
                <input
                  name="backType"
                  value={form.backType}
                  onChange={handleChange}
                />
              </div>
              <div className="field">
                <label>Front brake</label>
                <input
                  name="frontBrake"
                  value={form.frontBrake}
                  onChange={handleChange}
                />
              </div>
              <div className="field">
                <label>Rear brake</label>
                <input
                  name="backBrake"
                  value={form.backBrake}
                  onChange={handleChange}
                />
              </div>
              <div className="field">
                <label>Tyre size (front)</label>
                <input
                  name="tyreSizeFront"
                  value={form.tyreSizeFront}
                  onChange={handleChange}
                />
              </div>
              <div className="field">
                <label>Tyre size (rear)</label>
                <input
                  name="tyreSizeBack"
                  value={form.tyreSizeBack}
                  onChange={handleChange}
                />
              </div>
              <div className="field">
                <label>Tyre type</label>
                <input
                  name="tyreType"
                  value={form.tyreType}
                  onChange={handleChange}
                />
              </div>
              <div className="field">
                <label>Wheel material</label>
                <input
                  name="wheelMaterial"
                  value={form.wheelMaterial}
                  onChange={handleChange}
                />
              </div>
              <div className="field">
                <label>Spare</label>
                <input
                  name="spare"
                  value={form.spare}
                  onChange={handleChange}
                />
              </div>
              <div className="field">
                <label>Power steering</label>
                <input
                  name="poweredSteering"
                  value={form.poweredSteering}
                  onChange={handleChange}
                />
              </div>
            </div>
          </>
        )}
      </div>

      <div className="slide-form-footer">
        <button className="btn" type="submit" disabled={submitting}>
          {submitting
            ? "Saving..."
            : mode === "create"
            ? "Create"
            : "Save changes"}
        </button>

        <button
          type="button"
          className="btn btn-ghost"
          onClick={onCancel}
        >
          Cancel
        </button>
      </div>
    </form>
  );
}
