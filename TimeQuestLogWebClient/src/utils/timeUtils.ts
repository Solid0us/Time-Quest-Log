export const getHourMinSecFromMilliseconds = (milliseconds: number) => {
  const totalSeconds = Math.floor(milliseconds / 1000);
  const totalMinutes = Math.floor(totalSeconds / 60);

  const hours = Math.floor(totalMinutes / 60);
  const minutes = totalMinutes % 60;
  const seconds = totalSeconds % 60;

  return { hours, minutes, seconds };
};

export const getTimeDifferenceInMilliSeconds = (
  startDate: Date,
  endDate: Date
) => {
  return new Date(endDate).getTime() - new Date(startDate).getTime();
};

/**
 * Determines the month given a valid YYYY-MM-DD formatted string. Invalid strings
 * will return undefined.
 * @param dateString YYYY-MM-DD format
 * @param length Long or short name for month to be returned
 * @returns string | undefined
 */
export const getUTCMonthName = (
  dateString: string,
  length: "short" | "long" = "long"
) => {
  const date = new Date(dateString + "T00:00:00Z");

  const monthNames = [
    "January",
    "February",
    "March",
    "April",
    "May",
    "June",
    "July",
    "August",
    "September",
    "October",
    "November",
    "December",
  ];

  const monthIndex = date.getUTCMonth();
  if (length === "short") {
    return monthNames[monthIndex].slice(0, 3);
  }
  return monthNames[monthIndex];
};
