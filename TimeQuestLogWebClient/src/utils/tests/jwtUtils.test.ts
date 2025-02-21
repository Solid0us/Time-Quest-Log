import { expect, test } from "vitest";
import { getJwtPayload } from "../jwtUtils";

test("Decode Valid JWT properly", () => {
  expect(
    getJwtPayload(
      "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOiIxMjM0NTY3ODg5Iiwic3ViIjoiMTIzNDU2Nzg5MCIsIm5hbWUiOiJKb2huIERvZSIsImlhdCI6MTUxNjIzOTAyMn0._AMA0W2WXP-T8s-rH2i8ZVH_Zvwb8MN0uUdbw4_Mz8I"
    )
  ).toMatchObject({
    exp: "1234567889",
    sub: "1234567890",
    name: "John Doe",
    iat: 1516239022,
  });
});

test("Returns Invalid JWT Payload as Null", () => {
  expect(
    getJwtPayload(
      "eyyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.efyJleHAiOiIxMjM0NTY3ODg5Iiwic3ViIjoiMTIzNDU2Nzg5MCIsIm5hbWUiOiJKb2huIERvZSIsImlhdCI6MTUxNjIzOTAyMn0._AMA0W2WXP-T8s-rH2i8ZVH_Zvwb8MN0uUdbw4_Mz8I"
    )
  ).toBeNull();
});
