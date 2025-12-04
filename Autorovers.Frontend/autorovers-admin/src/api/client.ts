const API_BASE_URL =
  import.meta.env.VITE_API_BASE_URL ?? "http://localhost:5029";

export async function apiGet<T>(path: string): Promise<T> {
  const url = `${API_BASE_URL}${path}`;
  const res = await fetch(url);

  if (!res.ok) {
    throw new Error(`GET ${url} failed with ${res.status}`);
  }

  return res.json() as Promise<T>;
}

export async function apiPost<TBody, TResp = any>(
  path: string,
  body: TBody
): Promise<TResp> {
  const url = `${API_BASE_URL}${path}`;
  const res = await fetch(url, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(body),
  });

  if (!res.ok) {
    const text = await res.text();
    throw new Error(text || `POST ${url} failed with ${res.status}`);
  }

  return (await res.json()) as TResp;
}

export async function apiPut<TBody>(
  path: string,
  body: TBody
): Promise<void> {
  const url = `${API_BASE_URL}${path}`;
  const res = await fetch(url, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(body),
  });

  if (!res.ok) {
    const text = await res.text();
    throw new Error(text || `PUT ${url} failed with ${res.status}`);
  }
}

export async function apiDelete(path: string): Promise<void> {
  const url = `${API_BASE_URL}${path}`;
  const res = await fetch(url, { method: "DELETE" });

  if (!res.ok) {
    const text = await res.text();
    throw new Error(text || `DELETE ${url} failed with ${res.status}`);
  }
}
