// src/features/vehicles/types.ts

export interface Vehicle {
  id: number;
  brand: string;
  model: string;
  variant: string;
  year: number;
  price: number;
  category: string;
  transmission: string;
  slug: string;
  imageUrl?: string | null;
  
  // Engine & performance
  engineType: string;
  specification: string;
  inductionType: string;
  power: number;
  torque: number;
  emission: string;
  mileage: number;
  autoStartStop: string;
  range: number;

  // Dimensions & weight
  length: number;
  width: number;
  height: number;
  weight: number;
  groundClearance: number;
  wheelBase: number;

  // Capacity
  personCapacity: number;
  rows: number;
  doors: number;
  bootSpace: number;
  tankSize: number;

  // Tyres, brakes & steering
  frontTyre: string;
  backType: string;
  frontBrake: string;
  backBrake: string;
  poweredSteering: string;
  tyreSizeFront: string;
  tyreSizeBack: string;
  tyreType: string;
  wheelMaterial: string;
  spare: string;
}
