import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import type { Vehicle } from "../features/vehicles/types";
import { getPublicVehicleBySlug } from "../features/vehicles/api";

export function VehicleDetailsPage() {
  const { slug } = useParams<{ slug: string }>();
  const [vehicle, setVehicle] = useState<Vehicle | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (!slug) return;

    (async () => {
      try {
        setLoading(true);
        setError(null);
        const data = await getPublicVehicleBySlug(slug);
        setVehicle(data);
      } catch (err: any) {
        console.error(err);
        setError(err.message ?? "Failed to load vehicle");
      } finally {
        setLoading(false);
      }
    })();
  }, [slug]);

  if (loading) {
    return <div className="public-page">Loading vehicle…</div>;
  }

  if (error) {
    return <div className="public-page" style={{ color: "red" }}>{error}</div>;
  }

  if (!vehicle) {
    return <div className="public-page">Vehicle not found.</div>;
  }

  const v = vehicle;

  // --------- SAFE DISPLAY VALUES ---------
  const powerText = v.power ? `${v.power} hp` : "—";
  const torqueText = v.torque ? `${v.torque} Nm` : "—";
  const mileageText = v.mileage ? `${v.mileage} kmpl` : "—";
  const tankText = v.tankSize ? `${v.tankSize} L` : "—";
  const rangeText = v.range ? `${v.range} km` : "—";

  const lengthText = v.length ? `${v.length} mm` : "—";
  const widthText = v.width ? `${v.width} mm` : "—";
  const heightText = v.height ? `${v.height} mm` : "—";
  const wheelBaseText = v.wheelBase ? `${v.wheelBase} mm` : "—";
  const groundClearanceText = v.groundClearance ? `${v.groundClearance} mm` : "—";
  const weightText = v.weight ? `${v.weight} kg` : "—";

  const bootText = v.bootSpace ? `${v.bootSpace} L` : "—";
  const seatingText = v.personCapacity ? `${v.personCapacity} persons` : "—";

  return (
    <div className="public-page">
      <div className="public-topbar">
        <Link to="/vehicles/${v.slug}" className="btn btn-ghost">← Back to catalog</Link>
      </div>

      <div className="vehicle-hero">
        <div className="vehicle-hero-main">
          <h1 className="vehicle-title">
            {v.brand} {v.model} {v.variant}
          </h1>
          <p className="vehicle-subtitle">
            {v.year} • {v.category} • {v.transmission}
          </p>
          <p className="vehicle-price">₹ {v.price.toLocaleString("en-IN")}</p>

          <div className="vehicle-highlight-row">
            <div className="vehicle-highlight">
              <span className="label">Power</span>
              <span className="value">{powerText}</span>
            </div>
            <div className="vehicle-highlight">
              <span className="label">Torque</span>
              <span className="value">{torqueText}</span>
            </div>
            <div className="vehicle-highlight">
              <span className="label">Mileage</span>
              <span className="value">{mileageText}</span>
            </div>
            <div className="vehicle-highlight">
              <span className="label">Tank</span>
              <span className="value">{tankText}</span>
            </div>
          </div>
        </div>

        <div className="vehicle-hero-image">
          <div className="vehicle-image-placeholder">
            <span>{v.brand} {v.model}</span>
          </div>
        </div>
      </div>

      <div className="vehicle-spec-grid">

        {/* ENGINE & PERFORMANCE */}
        <section className="spec-card">
          <h2>Engine & Performance</h2>
          <dl>
            <div className="spec-row"><dt>Engine type</dt><dd>{v.engineType || "—"}</dd></div>
            <div className="spec-row"><dt>Specification</dt><dd>{v.specification || "—"}</dd></div>
            <div className="spec-row"><dt>Induction</dt><dd>{v.inductionType || "—"}</dd></div>
            <div className="spec-row"><dt>Power</dt><dd>{powerText}</dd></div>
            <div className="spec-row"><dt>Torque</dt><dd>{torqueText}</dd></div>
            <div className="spec-row"><dt>Emission</dt><dd>{v.emission || "—"}</dd></div>
            <div className="spec-row"><dt>Mileage</dt><dd>{mileageText}</dd></div>
            <div className="spec-row"><dt>Auto Start/Stop</dt><dd>{v.autoStartStop || "—"}</dd></div>
            <div className="spec-row"><dt>Range</dt><dd>{rangeText}</dd></div>
          </dl>
        </section>

        {/* DIMENSIONS */}
        <section className="spec-card">
          <h2>Dimensions & Weight</h2>
          <dl>
            <div className="spec-row"><dt>Length</dt><dd>{lengthText}</dd></div>
            <div className="spec-row"><dt>Width</dt><dd>{widthText}</dd></div>
            <div className="spec-row"><dt>Height</dt><dd>{heightText}</dd></div>
            <div className="spec-row"><dt>Wheelbase</dt><dd>{wheelBaseText}</dd></div>
            <div className="spec-row"><dt>Ground clearance</dt><dd>{groundClearanceText}</dd></div>
            <div className="spec-row"><dt>Kerb weight</dt><dd>{weightText}</dd></div>
          </dl>
        </section>

        {/* CAPACITY */}
        <section className="spec-card">
          <h2>Capacity & Layout</h2>
          <dl>
            <div className="spec-row"><dt>Seating</dt><dd>{seatingText}</dd></div>
            <div className="spec-row"><dt>Rows</dt><dd>{v.rows || "—"}</dd></div>
            <div className="spec-row"><dt>Doors</dt><dd>{v.doors || "—"}</dd></div>
            <div className="spec-row"><dt>Boot space</dt><dd>{bootText}</dd></div>
            <div className="spec-row"><dt>Fuel tank</dt><dd>{tankText}</dd></div>
          </dl>
        </section>

        {/* TYRES & BRAKES */}
        <section className="spec-card">
          <h2>Tyres, Brakes & Steering</h2>
          <dl>
            <div className="spec-row"><dt>Front tyre</dt><dd>{v.frontTyre || "—"}</dd></div>
            <div className="spec-row"><dt>Rear / Back type</dt><dd>{v.backType || "—"}</dd></div>
            <div className="spec-row"><dt>Front brake</dt><dd>{v.frontBrake || "—"}</dd></div>
            <div className="spec-row"><dt>Rear brake</dt><dd>{v.backBrake || "—"}</dd></div>
            <div className="spec-row"><dt>Tyre size (front)</dt><dd>{v.tyreSizeFront || "—"}</dd></div>
            <div className="spec-row"><dt>Tyre size (rear)</dt><dd>{v.tyreSizeBack || "—"}</dd></div>
            <div className="spec-row"><dt>Tyre type</dt><dd>{v.tyreType || "—"}</dd></div>
            <div className="spec-row"><dt>Wheel material</dt><dd>{v.wheelMaterial || "—"}</dd></div>
            <div className="spec-row"><dt>Spare</dt><dd>{v.spare || "—"}</dd></div>
            <div className="spec-row"><dt>Power steering</dt><dd>{v.poweredSteering || "—"}</dd></div>
          </dl>
        </section>

      </div>
    </div>
  );
}
