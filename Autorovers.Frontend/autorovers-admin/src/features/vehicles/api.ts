// src/features/vehicles/api.ts
import { apiGet, apiPost, apiPut, apiDelete } from "../../api/client";
import type { Vehicle } from "./types";

const VEHICLES_PATH = "/api/AdminVehicles";

// GET ALL VEHICLES
export async function getVehicles(): Promise<Vehicle[]> {
  return apiGet<Vehicle[]>(VEHICLES_PATH);
}

// GET SINGLE VEHICLE
export async function getVehicle(id: number): Promise<Vehicle> {
  return apiGet<Vehicle>(`${VEHICLES_PATH}/${id}`);
}

// CREATE
export async function createVehicle(payload: Vehicle): Promise<{ id: number }> {
  return apiPost<Vehicle, { id: number }>(VEHICLES_PATH, payload);
}

// UPDATE
export async function updateVehicle(id: number, payload: Vehicle): Promise<void> {
  return apiPut<Vehicle>(`${VEHICLES_PATH}/${id}`, payload);
}

// DELETE
export async function deleteVehicleById(id: number): Promise<void> {
  return apiDelete(`${VEHICLES_PATH}/${id}`);
}
