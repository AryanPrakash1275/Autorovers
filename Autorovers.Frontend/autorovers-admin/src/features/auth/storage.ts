const STORAGE_KEY = "autorovers_auth_token";

export function saveToken(token: string) {
  localStorage.setItem(STORAGE_KEY, token);
}

export function getToken(): string | null {
  return localStorage.getItem(STORAGE_KEY);
}

export function clearToken() {
  localStorage.removeItem(STORAGE_KEY);
}

export function isLoggedIn(): boolean {
  return !!getToken();
}
