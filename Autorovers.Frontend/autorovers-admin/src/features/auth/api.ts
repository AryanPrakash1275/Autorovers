import { apiPost } from "../../api/client";
import { saveToken } from "./storage";

export type LoginRequest = {
  email: string;
  password: string;
};

export async function login(req: LoginRequest): Promise<void> {
  const res: any = await apiPost<LoginRequest, any>("/api/auth/login", req);

  const token: string =
    res.token ?? res.accessToken ?? res.jwt ?? res.access_token ?? "";

  if (!token) {
    throw new Error("Login response did not contain a token");
  }

  saveToken(token);
}
