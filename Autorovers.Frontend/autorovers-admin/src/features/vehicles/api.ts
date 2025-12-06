import { apiGet, apiPost, apiPut, apiDelete } from "../../api/client";
import type { Vehicle } from "./types";

const ADMIN_VEHICLES_PATH = "/api/AdminVehicles";
const PUBLIC_VEHICLES_PATH = "/api/Vehicles";

// 🔹 PUBLIC ENDPOINTS
export async function getPublicVehicles(): Promise<Vehicle[]> {
  return apiGet<Vehicle[]>(PUBLIC_VEHICLES_PATH);
}

export async function getPublicVehicle(id: number): Promise<Vehicle> {
  return apiGet<Vehicle>(`${PUBLIC_VEHICLES_PATH}/${id}`);
}

// 🔹 ADMIN ENDPOINTS
export async function getVehicles(): Promise<Vehicle[]> {
  return apiGet<Vehicle[]>(ADMIN_VEHICLES_PATH);
}

export async function getVehicle(id: number): Promise<Vehicle> {
  return apiGet<Vehicle>(`${ADMIN_VEHICLES_PATH}/${id}`);
}

export async function createVehicle(payload: Vehicle): Promise<{ id: number }> {
  return apiPost<Vehicle, { id: number }>(ADMIN_VEHICLES_PATH, payload);
}

export async function updateVehicle(id: number, payload: Vehicle): Promise<void> {
  return apiPut<Vehicle>(`${ADMIN_VEHICLES_PATH}/${id}`, payload);
}

export async function deleteVehicleById(id: number): Promise<void> {
  return apiDelete(`${ADMIN_VEHICLES_PATH}/${id}`);
}
