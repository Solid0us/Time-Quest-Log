import { useQuery } from "@tanstack/react-query";
import { authFetch } from "./authServices";
import { getJwtPayload } from "@/utils/jwtUtils";
import { ApiResponse } from "@/types/apiTypes";

export type UserDetails = {
  id: string;
  username: string;
  firstName: string;
  lastName: string;
  email: string;
};

export const useGetUserDetails = () => {
  const jwt = localStorage.getItem("jwt");
  let userId = null;
  if (jwt) {
    userId = getJwtPayload(jwt)?.id;
  }
  const url = import.meta.env.VITE_API_BASE_URL + `api/v1/users/${userId}`;
  return useQuery<ApiResponse<UserDetails>>({
    queryKey: ["userDetails"],
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
