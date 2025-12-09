// src/api/client.ts
import { getToken } from "../features/auth/storage";

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

console.log("API_BASE_URL from env:", API_BASE_URL);

async function request<T>(path: string, options: RequestInit = {}): Promise<T> {
  if (!API_BASE_URL) {
    throw new Error(
      "API_BASE_URL is not defined. Set VITE_API_BASE_URL in your env files."
    );
  }

  const url = `${API_BASE_URL}${path}`;

  const headers: Record<string, string> = {
    "Content-Type": "application/json",
    ...(options.headers as Record<string, string> | undefined),
  };

  const token = getToken();
  if (token) {
    headers["Authorization"] = `Bearer ${token}`;
  }

  const res = await fetch(url, {
    ...options,
    headers,
  });

  if (!res.ok) {
    const text = await res.text();
    throw new Error(
      text || `Request failed: ${res.status} ${res.statusText} (${url})`
    );
  }

  if (res.status === 204) {
    return undefined as T;
  }

  return (await res.json()) as T;
}

export function apiGet<T>(path: string): Promise<T> {
  return request<T>(path, { method: "GET" });
}

export function apiPost<TBody, TResp = unknown>(
  path: string,
  body: TBody
): Promise<TResp> {
  return request<TResp>(path, {
    method: "POST",
    body: JSON.stringify(body),
  });
}

export function apiPut<TBody, TResp = void>(
  path: string,
  body: TBody
): Promise<TResp> {
  return request<TResp>(path, {
    method: "PUT",
    body: JSON.stringify(body),
  });
}

export function apiDelete<TResp = void>(path: string): Promise<TResp> {
  return request<TResp>(path, {
    method: "DELETE",
  });
}
