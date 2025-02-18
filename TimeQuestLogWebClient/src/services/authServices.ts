import { AuthResponse } from "@/types/apiTypes";

type LoginForm = {
  username: string;
  password: string;
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
