import { useCallback, useEffect, useState } from "react";
import type { Vehicle } from "../types";
import { getVehicles } from "../api";

export function useVehicles() {
  const [vehicles, setVehicles] = useState<Vehicle[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const load = useCallback(async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await getVehicles();
      setVehicles(data);
    } catch (err: any) {
      console.error(err);
      setError(err.message ?? "Failed to load vehicles");
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    void load();
  }, [load]);

  return {
    vehicles,
    loading,
    error,
    reload: load,
  };
}
