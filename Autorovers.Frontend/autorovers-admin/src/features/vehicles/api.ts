import { apiGet, apiPost, apiPut, apiDelete } from "../../api/client";
import type { Vehicle } from "./types";

const BASE = "/api/admin/vehicles";

export function getVehicles() {
  return apiGet<Vehicle[]>(BASE);
}

export function createVehicle(data: Vehicle) {
  // backend returns { id: number }
  return apiPost<Partial<Vehicle>, { id: number }>(BASE, data);
}

export function updateVehicle(id: number, data: Vehicle) {
  return apiPut<Partial<Vehicle>>(`${BASE}/${id}`, data);
}

export function deleteVehicleById(id: number) {
  return apiDelete(`${BASE}/${id}`);
}
