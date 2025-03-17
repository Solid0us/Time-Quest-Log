import { ApiResponse } from "@/types/apiTypes";
import { useQuery } from "@tanstack/react-query";
import { noAuthFetch } from "./authServices";

export const useGetInstaller = () => {
  const url = import.meta.env.VITE_API_BASE_URL + `api/v1/aws/s3/installers`;
  return useQuery<ApiResponse<string>>({
    queryKey: ["installers"],
    queryFn: async () => {
      const response = await noAuthFetch(url, {
        method: "GET",
      });

      if (!response.ok) {
        throw new Error("Failed to fetch installer.");
      }

      return response.json();
    },
    staleTime: 1000 * 60 * 5,
  });
};
