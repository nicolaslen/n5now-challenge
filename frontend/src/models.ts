export type Permission = {
  id: number;
  nombreEmpleado: string;
  apellidoEmpleado: string;
  tipo: PermissionType;
  fechaPermiso: Date;
};

export type PermissionPost = {
  id?: number;
  nombreEmpleado: string;
  apellidoEmpleado: string;
  tipo: number;
};

export type PermissionType = {
  id: number;
  description: string;
};

export type PaginationParams = {
  pageNumber: number;
  pageSize: number;
};

export type PagedResult<T> = {
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  items: Array<T>;
};
