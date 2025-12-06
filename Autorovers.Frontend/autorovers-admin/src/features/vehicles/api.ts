// src/features/vehicles/api.ts
import { apiGet, apiPost, apiPut, apiDelete } from "../../api/client";
import type { Vehicle } from "./types";

// ADMIN endpoints
const ADMIN_VEHICLES_PATH = "/api/AdminVehicles";

// PUBLIC endpoints
const PUBLIC_VEHICLES_PATH = "/api/Vehicles";

// ===== ADMIN =====

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

// ===== PUBLIC =====

export async function getPublicVehicles(): Promise<Vehicle[]> {
  return apiGet<Vehicle[]>(PUBLIC_VEHICLES_PATH);
}

export async function getPublicVehicleBySlug(slug: string): Promise<Vehicle> {
  return apiGet<Vehicle>(`${PUBLIC_VEHICLES_PATH}/slug/${slug}`);
}

export function getVehicleWithDetails(id: number) {
  return apiGet(`/api/AdminVehicles/${id}/details`);
}

export function updateVehicleDetails(id: number, payload: any) {
  return apiPut(`/api/AdminVehicles/${id}/details`, payload);
}
