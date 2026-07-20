export interface MaintenanceRequest {
  id: string;
  title: string;
  description: string;
  customerId: string;
  technicianId: string | null;
  serviceCategoryId: string;
  status: string;
  priority: string;
  location: string | null;
  preferredDate: string | null;
  createdAt: string;
  updatedAt: string | null;
}

export interface PagedResult<T> {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}