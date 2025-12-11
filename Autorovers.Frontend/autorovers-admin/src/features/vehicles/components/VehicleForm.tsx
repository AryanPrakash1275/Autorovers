import { useState, useEffect } from "react";
import type { Vehicle } from "../types";

type Props = {
  initial?: Vehicle | null;
  mode: "create" | "edit";
  onSubmit: (data: Vehicle) => Promise<void>;
  onCancel: () => void;
};

type Tab = "basics" | "specs";

type FormErrors = {
  brand?: string;
  model?: string;
  year?: string;
  price?: string;
  category?: string;
  transmission?: string;
};

const PLACEHOLDER_IMAGE =
  "https://via.placeholder.com/400x250?text=No+Image";

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
    imageUrl: initial?.imageUrl ?? "",

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
    frontType: initial?.frontType ?? "",
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

  const [errors, setErrors] = useState<FormErrors>({});
  const [status, setStatus] = useState<{
    type: "success" | "error" | null;
    message: string;
  }>({ type: null, message: "" });

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

    setForm((prev) => {
      if (numberFields.has(name)) {
        return {
          ...prev,
          [name]: value === "" ? 0 : Number(value),
        };
      }

      return { ...prev, [name]: value };
    });

    // clear error while typing
    setErrors((prev) => ({
      ...prev,
      [name]: undefined,
    }));

    // clear status if user starts editing again
    if (status.type) {
      setStatus({ type: null, message: "" });
    }
  }

  function validate(): boolean {
    const newErrors: FormErrors = {};
    const currentYear = new Date().getFullYear();

    if (!form.brand.trim()) newErrors.brand = "Brand is required";
    if (!form.model.trim()) newErrors.model = "Model is required";
    if (!form.category.trim()) newErrors.category = "Category is required";
    if (!form.transmission.trim())
      newErrors.transmission = "Transmission is required";

    if (!form.year || isNaN(form.year)) {
      newErrors.year = "Year is required";
    } else if (form.year < 1980 || form.year > currentYear + 1) {
      newErrors.year = "Enter a valid year";
    }

    if (
      form.price === undefined ||
      form.price === null ||
      isNaN(form.price)
    ) {
      newErrors.price = "Price is required";
    } else if (form.price <= 0) {
      newErrors.price = "Price must be greater than 0";
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    if (submitting) return;

    setStatus({ type: null, message: "" });

    if (!validate()) {
      // stay on basics tab where required stuff lives
      setActiveTab("basics");
      return;
    }

    setSubmitting(true);

    try {
      const payload: Vehicle = {
        ...form,
        // clean up strings
        brand: form.brand.trim(),
        model: form.model.trim(),
        category: form.category.trim(),
        transmission: form.transmission.trim(),
        slug: form.slug?.trim() ?? "",
        // enforce placeholder image if empty
        imageUrl: form.imageUrl?.trim() || PLACEHOLDER_IMAGE,
      };

      await onSubmit(payload);

      setStatus({
        type: "success",
        message:
          mode === "create"
            ? "Vehicle created successfully"
            : "Vehicle updated successfully",
      });
    } catch (err: any) {
      console.error(err);
      setStatus({
        type: "error",
        message: err?.message || "Failed to save vehicle",
      });
    } finally {
      setSubmitting(false);
    }
  }

  const previewImage = form.imageUrl?.trim() || PLACEHOLDER_IMAGE;

  return (
    <form className="form slide-form" onSubmit={handleSubmit}>
      <div className="slide-form-header">
        <h2>{mode === "create" ? "Add Vehicle" : "Edit Vehicle"}</h2>
        <button type="button" className="icon-btn" onClick={onCancel}>
          ✕
        </button>
      </div>

      {/* Status banner inside panel */}
      {status.type && (
        <div
          className={`form-status ${
            status.type === "success"
              ? "form-status--success"
              : "form-status--error"
          }`}
        >
          {status.message}
        </div>
      )}

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
            <div className={`field ${errors.brand ? "has-error" : ""}`}>
              <label>
                Brand <span className="required">*</span>
              </label>
              <input
                name="brand"
                value={form.brand}
                onChange={handleChange}
              />
              {errors.brand && (
                <div className="field-error">{errors.brand}</div>
              )}
            </div>

            <div className={`field ${errors.model ? "has-error" : ""}`}>
              <label>
                Model <span className="required">*</span>
              </label>
              <input
                name="model"
                value={form.model}
                onChange={handleChange}
              />
              {errors.model && (
                <div className="field-error">{errors.model}</div>
              )}
            </div>

            <div className="field">
              <label>Variant</label>
              <input
                name="variant"
                value={form.variant}
                onChange={handleChange}
              />
            </div>

            <div className={`field ${errors.year ? "has-error" : ""}`}>
              <label>
                Year <span className="required">*</span>
              </label>
              <input
                type="number"
                name="year"
                value={form.year}
                onChange={handleChange}
              />
              {errors.year && (
                <div className="field-error">{errors.year}</div>
              )}
            </div>

            <div className={`field ${errors.price ? "has-error" : ""}`}>
              <label>
                Price (₹) <span className="required">*</span>
              </label>
              <input
                type="number"
                name="price"
                value={form.price}
                onChange={handleChange}
              />
              {errors.price && (
                <div className="field-error">{errors.price}</div>
              )}
            </div>

            <div className={`field ${errors.category ? "has-error" : ""}`}>
              <label>
                Category <span className="required">*</span>
              </label>
              <input
                name="category"
                value={form.category}
                onChange={handleChange}
              />
              {errors.category && (
                <div className="field-error">{errors.category}</div>
              )}
            </div>

            <div
              className={`field ${errors.transmission ? "has-error" : ""}`}
            >
              <label>
                Transmission <span className="required">*</span>
              </label>
              <input
                name="transmission"
                value={form.transmission}
                onChange={handleChange}
              />
              {errors.transmission && (
                <div className="field-error">{errors.transmission}</div>
              )}
            </div>

            <div className="field">
              <label>Image URL</label>
              <input
                name="imageUrl"
                value={form.imageUrl ?? ""}
                onChange={handleChange}
                placeholder="https://example.com/bike.jpg (optional)"
              />
            </div>

            <div className="field" style={{ gridColumn: "1 / -1" }}>
              <label>Preview</label>
              <div className="image-preview">
                <img
                  src={previewImage}
                  alt="preview"
                  onError={(e) => {
                    (e.currentTarget as HTMLImageElement).src =
                      PLACEHOLDER_IMAGE;
                  }}
                />
              </div>
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
                <label>Front type</label>
                <input
                  name="frontType"
                  value={form.frontType}
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
