import { useState, useEffect } from "react";
import type { Vehicle } from "../types";

type Props = {
  initial?: Vehicle | null;
  mode: "create" | "edit";
  onSubmit: (data: Vehicle) => Promise<void>;
  onCancel: () => void;
};

const NUMBER_FIELDS = new Set([
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

    engineType: initial?.engineType ?? "",
    specification: initial?.specification ?? "",
    inductionType: initial?.inductionType ?? "",
    power: initial?.power ?? 0,
    torque: initial?.torque ?? 0,
    emission: initial?.emission ?? "",
    mileage: initial?.mileage ?? 0,
    autoStartStop: initial?.autoStartStop ?? "",
    range: initial?.range ?? 0,

    length: initial?.length ?? 0,
    width: initial?.width ?? 0,
    height: initial?.height ?? 0,
    weight: initial?.weight ?? 0,
    groundClearance: initial?.groundClearance ?? 0,
    wheelBase: initial?.wheelBase ?? 0,

    personCapacity: initial?.personCapacity ?? 2,
    rows: initial?.rows ?? 1,
    doors: initial?.doors ?? 0,
    bootSpace: initial?.bootSpace ?? 0,
    tankSize: initial?.tankSize ?? 0,

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

  useEffect(() => {
    if (initial) {
      setForm((prev) => ({ ...prev, ...initial }));
    }
  }, [initial]);

  function handleChange(e: React.ChangeEvent<HTMLInputElement>) {
    const { name, value } = e.target;

    setForm((prev) => ({
      ...prev,
      [name]: NUMBER_FIELDS.has(name) ? Number(value) || 0 : value,
    }));
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
      </div>

      <div className="slide-form-body">
        <div className="form-grid">
          {/* BASIC INFO */}
          <div className="field">
            <label>Brand</label>
            <input name="brand" value={form.brand} onChange={handleChange} />
          </div>

          <div className="field">
            <label>Model</label>
            <input name="model" value={form.model} onChange={handleChange} />
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
            <label>Slug</label>
            <input
              name="slug"
              value={form.slug}
              onChange={handleChange}
              placeholder="auto-rovers-hornet-20"
            />
          </div>

          {/* SPECS */}
          <div className="field">
            <label>Engine Type</label>
            <input
              name="engineType"
              value={form.engineType}
              onChange={handleChange}
            />
          </div>

          <div className="field">
            <label>Induction Type</label>
            <input
              name="inductionType"
              value={form.inductionType}
              onChange={handleChange}
            />
          </div>

          <div className="field">
            <label>Power (HP)</label>
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
              placeholder="Yes / No"
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

          {/* DIMENSIONS */}
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
            <label>Weight (kg)</label>
            <input
              type="number"
              name="weight"
              value={form.weight}
              onChange={handleChange}
            />
          </div>

          <div className="field">
            <label>Ground Clearance (mm)</label>
            <input
              type="number"
              name="groundClearance"
              value={form.groundClearance}
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

          {/* CAPACITY */}
          <div className="field">
            <label>Seating Capacity</label>
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
            <label>Boot Space (L)</label>
            <input
              type="number"
              name="bootSpace"
              value={form.bootSpace}
              onChange={handleChange}
            />
          </div>

          <div className="field">
            <label>Fuel Tank Size (L)</label>
            <input
              type="number"
              name="tankSize"
              value={form.tankSize}
              onChange={handleChange}
            />
          </div>

          <div className="field" style={{ gridColumn: "1 / -1", marginTop: "0.5rem" }}>
            <label style={{ fontWeight: 600 }}>Dynamics</label>
          </div>

          <div className="field">
            <label>Front Tyre</label>
            <input
              name="frontTyre"
              value={form.frontTyre}
              onChange={handleChange}
              placeholder="e.g. 100/80-17, Tubeless"
            />
          </div>

          <div className="field">
            <label>Back Type</label>
            <input
              name="backType"
              value={form.backType}
              onChange={handleChange}
              placeholder="e.g. Mono-shock / Tyre description"
            />
          </div>

          <div className="field">
            <label>Front Brake</label>
            <input
              name="frontBrake"
              value={form.frontBrake}
              onChange={handleChange}
              placeholder="e.g. Disc"
            />
          </div>

          <div className="field">
            <label>Back Brake</label>
            <input
              name="backBrake"
              value={form.backBrake}
              onChange={handleChange}
              placeholder="e.g. Drum / Disc"
            />
          </div>

          <div className="field">
            <label>Powered Steering</label>
            <input
              name="poweredSteering"
              value={form.poweredSteering}
              onChange={handleChange}
              placeholder="e.g. No / Electric / Hydraulic"
            />
          </div>

          <div className="field">
            <label>Tyre Size (Front)</label>
            <input
              name="tyreSizeFront"
              value={form.tyreSizeFront}
              onChange={handleChange}
              placeholder="e.g. 100/80-17"
            />
          </div>

          <div className="field">
            <label>Tyre Size (Back)</label>
            <input
              name="tyreSizeBack"
              value={form.tyreSizeBack}
              onChange={handleChange}
              placeholder="e.g. 130/70-17"
            />
          </div>

          <div className="field">
            <label>Tyre Type</label>
            <input
              name="tyreType"
              value={form.tyreType}
              onChange={handleChange}
              placeholder="e.g. Tubeless"
            />
          </div>

          <div className="field">
            <label>Wheel Material</label>
            <input
              name="wheelMaterial"
              value={form.wheelMaterial}
              onChange={handleChange}
              placeholder="e.g. Alloy / Spoke"
            />
          </div>

          <div className="field">
            <label>Spare</label>
            <input
              name="spare"
              value={form.spare}
              onChange={handleChange}
              placeholder="e.g. No / Full-size / Space saver"
            />
          </div>

          <div className="field" style={{ gridColumn: "1 / -1" }}>
            <label>Specification / Notes</label>
            <input
              name="specification"
              value={form.specification}
              onChange={handleChange}
              placeholder="Any extra notes or spec summary"
            />
          </div>
        </div>
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
