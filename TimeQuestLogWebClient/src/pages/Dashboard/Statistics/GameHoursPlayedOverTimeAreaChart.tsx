import { UserGameStats } from "@/services/userGameServices";
import { useMemo, useState } from "react";
import AreaChartContainer from "./AreaChartContainer";
import {
  ChartContainer,
  ChartTooltip,
  ChartTooltipContent,
} from "@/components/ui/chart";
import { Area, AreaChart, CartesianGrid, XAxis, YAxis } from "recharts";
import { getUTCMonthName } from "@/utils/timeUtils";

type ChartSeriesConfig = {
  label: string;
  color?: string;
  valueFormatter?: (value: number) => string;
};

type GenreChartConfig = {
  [K in string]: ChartSeriesConfig;
};

const createChartConfig = (): GenreChartConfig => {
  const config: GenreChartConfig = {
    hours: {
      label: "Hours Played",
      valueFormatter: (value: number) => `${value.toFixed(1)} hrs`,
    },
  };

  return config;
};

const GameHoursPlayedOverTimeAreaChart = ({
  stats,
}: {
  stats: UserGameStats;
}) => {
  const [year, setYear] = useState(`${new Date().getFullYear()}`);
  const monthlyData = useMemo(
    () => formatGameMonthyData(stats, year),
    [stats.hoursPlayedDistributionPerYearPerGame, year]
  );
  const chartConfig = createChartConfig();
  return (
    <AreaChartContainer
      chartTitle="Gaming Hours"
      chartDescription="Total hours played over the course of the year by month."
      year={year}
      setYear={setYear}
      yearSelectList={Object.entries(stats.hoursPlayedDistributionPerYear)
        .map((entry) => entry[0])
        .reverse()}
    >
      <ChartContainer
        config={chartConfig}
        className="aspect-auto h-[300px] w-full"
      >
        <AreaChart margin={{ left: 10, right: 10 }} data={monthlyData}>
          <CartesianGrid vertical={false} />
          <XAxis
            dataKey="date"
            tickLine={false}
            axisLine={false}
            tickMargin={8}
            minTickGap={5}
            tickFormatter={(value: string) => {
              return getUTCMonthName(value, "short");
            }}
          />
          <ChartTooltip
            cursor={false}
            content={
              <ChartTooltipContent
                labelFormatter={(value: string) => {
                  return getUTCMonthName(value);
                }}
                indicator="dot"
              />
            }
          />
          <YAxis
            tickLine={false}
            axisLine={false}
            tickMargin={8}
            domain={[0, "auto"]}
            width={50}
          />
          <Area
            dataKey={"hours"}
            type="monotone"
            fill={`var(--chart-1)`}
            stroke={`var(--chart-1)`}
            stackId="a"
          />
        </AreaChart>
      </ChartContainer>
    </AreaChartContainer>
  );
};

const formatGameMonthyData = (stats: UserGameStats, year: string) => {
  const monthlyDataMap = new Map();
  const yearData = stats.hoursPlayedDistributionPerYearPerGame.filter(
    (entry) => entry.year === year
  );

  for (let month = 1; month <= 12; month++) {
    const monthKey = `${year}-${month.toString().padStart(2, "0")}`;
    monthlyDataMap.set(monthKey, { date: monthKey, hours: 0 });
  }

  yearData.forEach((entry) => {
    const monthKey = `${entry.year}-${entry.month.padStart(2, "0")}`;
    monthlyDataMap.get(monthKey).hours += entry.hoursPlayed;
  });

  return Array.from(monthlyDataMap.values()).sort((a, b) =>
    a.date.localeCompare(b.date)
  );
};

export default GameHoursPlayedOverTimeAreaChart;
