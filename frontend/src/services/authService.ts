import api from "./api";
import type { ApiResponse, AuthResponse, LoginRequest } from "../types/auth";

export async function loginUser(data: LoginRequest): Promise<AuthResponse> {
  const response = await api.post<ApiResponse<AuthResponse>>(
    "/auth/login",
    data
  );

  const authData = response.data.data;

  localStorage.setItem("fixflow_token", authData.token);
  localStorage.setItem("fixflow_user", JSON.stringify(authData));

  return authData;
}

export function logoutUser() {
  localStorage.removeItem("fixflow_token");
  localStorage.removeItem("fixflow_user");
}

export function getCurrentUser(): AuthResponse | null {
  const user = localStorage.getItem("fixflow_user");

  if (!user) {
    return null;
  }

  return JSON.parse(user) as AuthResponse;
}