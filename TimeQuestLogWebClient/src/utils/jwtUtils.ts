export type Jwt = {
  exp: number;
  iat: number;
  id: string;
  sub: string;
};

export const getJwtPayload = (token: string): Jwt | null => {
  try {
    const [, payload] = token.split(".");
    return JSON.parse(atob(payload));
  } catch (err) {
    return null;
  }
};
