export type AuthResponse = {
  timestamp: Date;
  userId: string | null;
  username: string;
  token: string | null;
  refreshToken: string | null;
  error: string | null;
};

export type ErrorDetail = {
  field: string;
  message: string;
};

export type Pagination = {
  currentPage: number;
  pageSize: number;
  totalPages: number;
  totalItems: number;
};

export type ApiResponse<T> = {
  status: string;
  message: string;
  data: T;
  timestamp: Date;
  errors: ErrorDetail[] | null;
  pagination: Pagination | null;
};
