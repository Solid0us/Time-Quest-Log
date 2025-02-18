export type AuthResponse = {
  timestamp: Date;
  userId: string | null;
  username: string;
  token: string | null;
  refreshToken: string | null;
  error: string | null;
};
