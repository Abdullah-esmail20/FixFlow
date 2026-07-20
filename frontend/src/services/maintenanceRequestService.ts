import api from "./api";
import type { ApiResponse } from "../types/auth";
import type {
  MaintenanceRequest,
  PagedResult,
} from "../types/maintenanceRequest";

export async function getAdminMaintenanceRequests() {
  const response = await api.get<ApiResponse<PagedResult<MaintenanceRequest>>>(
    "/maintenance-requests/admin",
    {
      params: {
        PageNumber: 1,
        PageSize: 10,
        SortBy: "CreatedAt",
        SortDirection: "desc",
      },
    }
  );

  return response.data.data;
}