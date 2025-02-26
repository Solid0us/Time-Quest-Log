import { expect, test } from "vitest";
import { getUTCMonthName } from "../timeUtils";

test("Valid YYYY-MM-DD results in correct months.", () => {
  expect(getUTCMonthName("2025-12-23")).toBe("December");
  expect(getUTCMonthName("2025-12-23", "short")).toBe("Dec");
  expect(getUTCMonthName("2025-03-23")).toBe("March");
  expect(getUTCMonthName("2025-04-23")).toBe("April");
  expect(getUTCMonthName("2025-01-23")).toBe("January");
});

test("Invalid YYYY-MM-DD results in returning undefined", () => {
  expect(getUTCMonthName("20254-01-20")).toBe(undefined);
  expect(getUTCMonthName("2025-1-20")).toBe(undefined);
  expect(getUTCMonthName("20254-01-2")).toBe(undefined);
  expect(getUTCMonthName("2022-3-20")).toBe(undefined);
  expect(getUTCMonthName("202-01-20")).toBe(undefined);
});
