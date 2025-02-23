import { useAuth } from "@/hooks/useAuth";
import { ApiResponse } from "@/types/apiTypes";
import { useQuery } from "@tanstack/react-query";
import { authFetch } from "./authServices";

export type GameSession = {
  id: string;
  user: {
    id: string;
    username: string;
    firstName: string;
    lastName: string;
    email: string;
  };
  game: {
    id: number;
    name: string;
    genres: {
      id: number;
      name: string;
    }[];
  };
  startTime: Date;
  endTime: Date | null;
};

export const useGetGameSessions = () => {
  const { userId } = useAuth();
  const url =
    import.meta.env.VITE_API_BASE_URL + `api/v1/game-sessions/users/${userId}`;
  return useQuery<ApiResponse<GameSession[]>>({
    queryKey: ["gameSessions", userId],
    queryFn: async () => {
      const response = await authFetch(url, {
        method: "GET",
      });

      if (!response.ok) {
        throw new Error("Failed to fetch user details");
      }

      return response.json();
    },
    staleTime: 1000 * 60 * 5,
  });
};
