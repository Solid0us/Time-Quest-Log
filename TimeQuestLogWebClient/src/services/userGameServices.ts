import { authFetch } from "./authServices";
import { ApiResponse } from "@/types/apiTypes";
import { useQuery } from "@tanstack/react-query";
import { useAuth } from "@/hooks/useAuth";

export type UserGame = {
  gameId: number;
  gameName: string;
  coverUrl: string;
  exeName: string;
  genres: string;
  hoursPlayed: number;
};

export type UserGameStats = {
  userId: string;
  totalHoursPlayed: number;
  totalGames: number;
  hoursPlayedPerGenre: {
    [key: string]: number;
  };
  hoursPlayedPerGame: {
    gameId: number;
    gameTitle: string;
    hoursPlayed: number;
  }[];
  hoursPlayedDistributionPerYear: {
    [key: string]: number;
  };
  hoursPlayedDistributionPerYearPerGenre: {
    genreId: number;
    genreName: string;
    year: string;
    month: string;
    hoursPlayed: number;
  }[];
  hoursPlayedDistributionPerYearPerGame: {
    gameId: number;
    gameTitle: string;
    year: string;
    month: string;
    hoursPlayed: string;
  }[];
};

export type UserIndividualGameStats = {
  hoursPlayed: number;
  avgSessionTime: number;
  longestSessionTime: number;
  longestSessionDate: Date;
  lastPlayed: Date;
  firstTimePlayed: Date;
};

export const useGetUserGames = () => {
  const { userId, setShowReLoginModal } = useAuth();
  const url =
    import.meta.env.VITE_API_BASE_URL +
    `api/v1/user-games/${userId}/?hours=true`;
  return useQuery<ApiResponse<UserGame[]>>({
    queryKey: ["userGames", userId],
    queryFn: async () => {
      const response = await authFetch(url, {
        method: "GET",
      });

      if (response.status === 401) {
        setShowReLoginModal(true);
      }

      if (!response.ok) {
        throw new Error("Failed to fetch user details");
      }

      return response.json();
    },
    staleTime: 1000 * 60 * 5,
  });
};

export const useGetUserGameStats = () => {
  const { userId, setShowReLoginModal } = useAuth();
  const url =
    import.meta.env.VITE_API_BASE_URL + `api/v1/user-games/${userId}/stats`;
  return useQuery<ApiResponse<UserGameStats>>({
    queryKey: ["userGames", userId, "stats"],
    queryFn: async () => {
      const response = await authFetch(url, {
        method: "GET",
      });

      if (response.status === 401) {
        setShowReLoginModal(true);
      }

      if (!response.ok) {
        throw new Error("Failed to fetch user details");
      }

      return response.json();
    },
    staleTime: 1000 * 60 * 5,
  });
};

export const useGetUserIndividualGameStats = (gameId: number) => {
  const { userId, setShowReLoginModal } = useAuth();
  const url =
    import.meta.env.VITE_API_BASE_URL +
    `api/v1/user-games/${userId}/stats/${gameId}`;
  return useQuery<ApiResponse<UserIndividualGameStats>>({
    queryKey: ["userGames", userId, "stats", gameId],
    queryFn: async () => {
      const response = await authFetch(url, {
        method: "GET",
      });

      if (response.status === 401) {
        setShowReLoginModal(true);
      }

      if (!response.ok) {
        throw new Error("Failed to fetch user details");
      }

      return response.json();
    },
    staleTime: 1000 * 60 * 5,
  });
};
