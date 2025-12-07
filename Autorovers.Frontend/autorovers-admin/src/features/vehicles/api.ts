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

export async function updateVehicle(
  id: number,
  payload: Vehicle
): Promise<void> {
  return apiPut<Vehicle>(`${ADMIN_VEHICLES_PATH}/${id}`, payload);
}

export async function deleteVehicleById(id: number): Promise<void> {
  return apiDelete(`${ADMIN_VEHICLES_PATH}/${id}`);
}

// ===== PUBLIC (list) =====

export async function getPublicVehicles(): Promise<Vehicle[]> {
  return apiGet<Vehicle[]>(PUBLIC_VEHICLES_PATH);
}

// ADMIN details endpoints (used by VehicleListPage.openEdit)
export function getVehicleWithDetails(id: number) {
  return apiGet(`/api/AdminVehicles/${id}/details`);
}

export function updateVehicleDetails(id: number, payload: any) {
  return apiPut(`/api/AdminVehicles/${id}/details`, payload);
}

// ===== PUBLIC (details) =====
// *** THIS is the only part we really needed to fix ***

export async function getPublicVehicleBySlug(
  slug: string
): Promise<Vehicle> {
  // Use the same API client as everywhere else
  const full: any = await apiGet(
    `${PUBLIC_VEHICLES_PATH}/slug/${encodeURIComponent(slug)}`
  );

  const d = full.details ?? {};

  const vehicle: Vehicle = {
    // top-level
    id: full.id,
    brand: full.brand,
    model: full.model,
    variant: full.variant,
    year: full.year,
    price: full.price,
    category: full.category,
    transmission: full.transmission,
    slug: full.slug,

    // flattened details
    engineType: d.engineType ?? "",
    specification: d.specification ?? "",
    inductionType: d.inductionType ?? "",
    power: d.power ?? 0,
    torque: d.torque ?? 0,
    emission: d.emission ?? "",
    mileage: d.mileage ?? 0,
    autoStartStop: d.autoStartStop ?? "",
    range: d.range ?? 0,

    length: d.length ?? 0,
    width: d.width ?? 0,
    height: d.height ?? 0,
    weight: d.weight ?? 0,
    groundClearance: d.groundClearance ?? 0,
    wheelBase: d.wheelBase ?? 0,

    personCapacity: d.personCapacity ?? 0,
    rows: d.rows ?? 0,
    doors: d.doors ?? 0,
    bootSpace: d.bootSpace ?? 0,
    tankSize: d.tankSize ?? 0,

    frontTyre: d.frontTyre ?? "",
    backType: d.backType ?? "",
    frontBrake: d.frontBrake ?? "",
    backBrake: d.backBrake ?? "",
    poweredSteering: d.poweredSteering ?? "",
    tyreSizeFront: d.tyreSizeFront ?? "",
    tyreSizeBack: d.tyreSizeBack ?? "",
    tyreType: d.tyreType ?? "",
    wheelMaterial: d.wheelMaterial ?? "",
    spare: d.spare ?? "",
  };

  return vehicle;
}

 