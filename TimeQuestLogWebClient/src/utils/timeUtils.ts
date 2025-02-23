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
