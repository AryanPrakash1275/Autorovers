import { useState, useEffect } from "react";
import type { Vehicle } from "../types";

type Props = {
  initial?: Vehicle | null;
  mode: "create" | "edit";
  onSubmit: (data: Vehicle) => Promise<void>;
  onCancel: () => void;
};

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
      [name]: name === "price" || name === "year" ? Number(value) : value,
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
        <button type="button" className="icon-btn" onClick={onCancel}>
          ✕
        </button>
      </div>

      <div className="slide-form-body">
        <div className="form-grid">
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
            <input name="variant" value={form.variant} onChange={handleChange} />
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
