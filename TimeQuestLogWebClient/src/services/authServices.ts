import { AuthResponse } from "@/types/apiTypes";

export type LoginForm = {
  username: string;
  password: string;
};

type RegisterForm = {
  username: string;
  email: string;
  firstName: string;
  lastName: string;
  password: string;
  confirmPassword: string;
};

export const loginUser = async (
  loginForm: LoginForm
): Promise<AuthResponse> => {
  const url = import.meta.env.VITE_API_BASE_URL + "api/v1/users/login";
  const response = await fetch(url, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      username: loginForm.username,
      password: loginForm.password,
    }),
  });
  const json = (await response.json()) as AuthResponse;
  if (response.ok) {
    return json;
  } else if (response.status == 401) {
    throw new Error("Invalid username or password.");
  }
  throw new Error("Unknown server error occurred.");
};

export const registerUser = async (registerForm: RegisterForm) => {
  const url = import.meta.env.VITE_API_BASE_URL + "api/v1/users/register";
  const { confirmPassword, firstName, lastName, password, username, email } =
    registerForm;
  const response = await fetch(url, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      username,
      firstName,
      lastName,
      email,
      password,
      confirmPassword,
    }),
  });
  const json = (await response.json()) as AuthResponse;
  if (response.ok) {
    return json;
  }
  throw new Error("Unknown server error occurred.");
};

let isRefreshing = false;
let refreshPromise: Promise<string> | null = null;

interface TokenResponse {
  token: string | null;
}

async function performTokenRefresh(): Promise<string> {
  const url = import.meta.env.VITE_API_BASE_URL + "api/v1/users/refresh";
  const refreshToken = localStorage
    .getItem("refreshToken")
    ?.replace(/^"(.*)"$/, "$1");

  if (!refreshToken) {
    throw new Error("No refresh token available");
  }

  try {
    const response = await fetch(url, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ refreshToken }),
    });

    if (!response.ok) {
      throw new Error("Token refresh failed");
    }

    const { token }: TokenResponse = await response.json();
    if (token) localStorage.setItem("jwt", token);

    return token ?? "";
  } catch (error) {
    throw error;
  }
}

export async function noAuthFetch(
  input: RequestInfo | URL,
  init: RequestInit = {}
): Promise<Response> {
  let headers =
    init.headers instanceof Headers
      ? init.headers
      : new Headers(init.headers || {});
  headers.set("Content-Type", "application/json");

  const config: RequestInit = {
    ...init,
    headers,
  };
  const response = await fetch(input, config);
  return response;
}

export async function authFetch(
  input: RequestInfo | URL,
  init: RequestInit = {},
  retryCount: number = 0
): Promise<Response> {
  const maxRetries = 1;
  let jwt = localStorage.getItem("jwt")?.replace(/^"(.*)"$/, "$1");
  let headers =
    init.headers instanceof Headers
      ? init.headers
      : new Headers(init.headers || {});
  headers.set("Content-Type", "application/json");
  if (!jwt) {
    try {
      jwt = (await performTokenRefresh()).replace(/^"(.*)"$/, "$1");
    } catch (err) {
      console.log(err);
    }
  }

  headers.set("Authorization", `Bearer ${jwt}`);

  const config: RequestInit = {
    ...init,
    headers,
  };

  const response = await fetch(input, config);
  if (response.status === 401) {
    if (retryCount >= maxRetries) {
      return response;
    }

    if (!isRefreshing) {
      isRefreshing = true;
      try {
        refreshPromise = performTokenRefresh().finally(() => {
          isRefreshing = false;
        });
        await refreshPromise;
        return authFetch(input, init, retryCount + 1);
      } catch (err) {
        console.log(err);
      }
    }
  }

  return response;
}
