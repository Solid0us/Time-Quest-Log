import { getJwtPayload } from "@/utils/jwtUtils";
import { createContext, useCallback, useEffect, useState } from "react";

interface AuthContextType {
  isAuthenticated: boolean;
  login: (jwt: string, refreshToken: string) => void;
  logout: () => void;
  jwt: string | null;
  userId: string | null | undefined;
  refreshToken: string | null;
}
export const AuthContext = createContext<AuthContextType | null>(null);

const isUserAuthenticated = (refreshToken: string | null) => {
  if (refreshToken) {
    const exp = getJwtPayload(refreshToken)?.exp;
    const userId = getJwtPayload(refreshToken)?.id;
    if (exp && exp > new Date().getTime() && userId) {
    }
    return true;
  }
  return false;
};

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [authState, setAuthState] = useState(() => {
    const jwt = localStorage.getItem("jwt");
    const refreshToken = localStorage.getItem("refreshToken");
    const userId = refreshToken ? getJwtPayload(refreshToken)?.id : null;

    return {
      jwt,
      refreshToken,
      userId,
      isAuthenticated: isUserAuthenticated(refreshToken),
    };
  });

  const login = useCallback((jwt: string, refreshToken: string) => {
    localStorage.setItem("jwt", jwt);
    localStorage.setItem("refreshToken", refreshToken);

    const userId = getJwtPayload(jwt)?.id;

    setAuthState({
      jwt,
      refreshToken,
      userId,
      isAuthenticated: Boolean(jwt && userId && refreshToken),
    });
  }, []);

  const logout = useCallback(() => {
    localStorage.removeItem("jwt");
    localStorage.removeItem("refreshToken");

    setAuthState({
      jwt: null,
      refreshToken: null,
      userId: null,
      isAuthenticated: false,
    });
  }, []);

  useEffect(() => {
    const validateToken = async () => {
      if (authState.jwt) {
        try {
          const userId = getJwtPayload(authState.jwt)?.id;
          if (!userId) {
            logout();
          }
        } catch (error) {
          console.error("Token validation failed:", error);
          logout();
        }
      }
    };
    validateToken();
  }, [authState.jwt]);

  const value = {
    isAuthenticated: authState.isAuthenticated,
    login,
    logout,
    jwt: authState.jwt,
    userId: authState.userId,
    refreshToken: authState.refreshToken,
  };
  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};
