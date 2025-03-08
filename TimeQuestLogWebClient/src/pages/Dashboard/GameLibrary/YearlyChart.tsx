import {
  ChartConfig,
  ChartContainer,
  ChartTooltip,
  ChartTooltipContent,
} from "@/components/ui/chart";
import YearlyChartCard from "../Statistics/YearlyChartCard";
import { CartesianGrid, Line, LineChart, XAxis } from "recharts";
import {
  getTimeDifferenceInMilliSeconds,
  getUTCMonthName,
} from "@/utils/timeUtils";
import { GameSession, useGetGameSessions } from "@/services/gameSessionService";
import { useMemo, useState } from "react";
import { UserGame } from "@/services/userGameServices";
import { Row } from "@tanstack/react-table";

interface YearlyChartProps {
  userGameRow: Row<UserGame>;
}

const chartConfig = {
  hours: {
    label: "Hours",
  },
} satisfies ChartConfig;

const YearlyChart = ({ userGameRow }: YearlyChartProps) => {
  const { data: gameSessions, isFetching: isGameSessionsFetching } =
    useGetGameSessions();
  const [year, setYear] = useState(`${new Date().getFullYear()}`);
  const yearList = useMemo(() => {
    let yearSet = new Set();
    if (gameSessions) {
      gameSessions.data.forEach((session) =>
        yearSet.add(new Date(session.startTime).getUTCFullYear().toString())
      );
    }
    return Array.from(yearSet) as string[];
  }, [gameSessions, userGameRow]);
  const monthlyData = useMemo(
    () =>
      formatGameMonthlyData(
        gameSessions?.data ?? [],
        userGameRow.original.gameId,
        year
      ),
    [gameSessions, userGameRow, year]
  );
  return (
    <YearlyChartCard
      chartTitle="Gaming Hours"
      chartDescription="Your hours played per month."
      year={year}
      setYear={setYear}
      yearSelectList={yearList}
    >
      <ChartContainer
        config={chartConfig}
        className="aspect-auto h-[250px] w-full"
      >
        <LineChart
          accessibilityLayer
          data={monthlyData}
          margin={{
            left: 12,
            right: 12,
          }}
        >
          <CartesianGrid vertical={false} />
          <XAxis
            dataKey="date"
            tickLine={false}
            axisLine={false}
            tickMargin={8}
            minTickGap={32}
            tickFormatter={(value: string) => {
              return getUTCMonthName(value, "short");
            }}
          />
          <ChartTooltip
            content={
              <ChartTooltipContent
                className="w-[150px]"
                nameKey="hours"
                labelFormatter={(value: string) => {
                  return getUTCMonthName(value);
                }}
              />
            }
          />
          <Line
            dataKey={"hours"}
            type="monotone"
            stroke={`var(--chart-1)`}
            strokeWidth={2}
            dot={false}
          />
        </LineChart>
      </ChartContainer>
    </YearlyChartCard>
  );
};

const formatGameMonthlyData = (
  sessions: GameSession[],
  gameId: number,
  year: string
) => {
  const monthlyDataMap = new Map();
  const yearData = sessions.filter(
    (entry) =>
      new Date(entry.startTime).getUTCFullYear().toString() === year &&
      entry.endTime !== null &&
      gameId === entry.game.id
  );

  for (let month = 1; month <= 12; month++) {
    const monthKey = `${year}-${month.toString().padStart(2, "0")}`;
    monthlyDataMap.set(monthKey, { date: monthKey, hours: 0 });
  }

  yearData.forEach((entry) => {
    const monthKey = `${new Date(entry.startTime).getUTCFullYear()}-${(
      new Date(entry.startTime).getUTCMonth() + 1
    )
      .toString()
      .padStart(2, "0")}`;
    const milliseconds = getTimeDifferenceInMilliSeconds(
      new Date(entry.startTime),
      new Date(entry.endTime as Date)
    );
    const hoursPlayed = milliseconds / 1000 / 60 / 60;
    monthlyDataMap.get(monthKey).hours += hoursPlayed;
  });
  return Array.from(monthlyDataMap.values()).sort((a, b) =>
    a.date.localeCompare(b.date)
  );
};

export default YearlyChart;
