import { getJwtPayload } from "@/utils/jwtUtils";
import { authFetch } from "./authServices";
import { ApiResponse } from "@/types/apiTypes";
import { useQuery } from "@tanstack/react-query";
import { useAuth } from "@/hooks/useAuth";

export type UserGame = {
  id: string;
  user: {
    id: string;
    email: string;
    username: string;
    firstName: string;
    lastName: string;
    createdAt: Date;
  };
  game: {
    id: string;
    name: string;
    genres: {
      id: number;
      name: string;
    }[];
    coverUrl: string;
  };
  exeName: string;
};

export const useGetUserGames = () => {
  const { userId } = useAuth();
  const url = import.meta.env.VITE_API_BASE_URL + `api/v1/user-games/${userId}`;
  return useQuery<ApiResponse<UserGame[]>>({
    queryKey: ["userGames"],
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
