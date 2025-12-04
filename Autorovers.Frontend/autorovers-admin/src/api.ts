export interface VehicleListItem {
  id: string;
  brand: string;
  model: string;
  variant: string;
  year: number;
  price: number;
  category: string;
  transmission: string;
  slug: string;
}

const API_BASE_URL =
  import.meta.env.VITE_API_BASE_URL ?? "http://localhost:5029";

const VEHICLES_PATH = "/api/admin/vehicles"; // ✅ you said this is your real path

export async function fetchVehicles(): Promise<VehicleListItem[]> {
  const res = await fetch(`${API_BASE_URL}${VEHICLES_PATH}`);

  if (!res.ok) {
    throw new Error(`Failed to fetch vehicles: ${res.status}`);
  }

  return res.json();
}
