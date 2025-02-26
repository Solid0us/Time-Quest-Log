import { useMemo, useState } from "react";
import { Area, AreaChart, CartesianGrid, XAxis } from "recharts";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import {
  ChartContainer,
  ChartLegend,
  ChartLegendContent,
  ChartTooltip,
  ChartTooltipContent,
} from "@/components/ui/chart";
import { UserGameStats } from "@/services/userGameServices";
import { getUTCMonthName } from "@/utils/timeUtils";
import { Label } from "@/components/ui/label";

type ChartSeriesConfig = {
  label: string;
  color?: string;
  valueFormatter?: (value: number) => string;
};

type GenreChartConfig = {
  [K in string]: ChartSeriesConfig;
};

interface MonthlyDataPoint {
  date: string;
  [key: string]: string | number;
}

const createChartConfig = (genres: string[]): GenreChartConfig => {
  const config: GenreChartConfig = {
    hours: {
      label: "Hours Played",
      valueFormatter: (value: number) => `${value.toFixed(1)} hrs`,
    },
  };

  genres.forEach((genre, index) => {
    config[genre] = {
      label: genre,
      color: `hsl(var(--chart-${index + 1}))`,
      valueFormatter: (value: number) => `${value.toFixed(1)} hrs`,
    };
  });

  return config;
};

const GameStatsDashboard = ({ stats }: { stats: UserGameStats }) => {
  const [timeRange, setTimeRange] = useState(`${new Date().getFullYear()}`);

  const monthlyData: MonthlyDataPoint[] = useMemo(
    () => formatYearMonthGenreDataToMonthlyDataPoint(stats, timeRange),
    [stats.hoursPlayedDistributionPerYearPerGenre, timeRange]
  );

  const genreNames = useMemo(() => {
    let genreSet: Set<string> = new Set();
    monthlyData.forEach((data) => {
      for (const [key, value] of Object.entries(data)) {
        if (key !== "date") {
          genreSet.add(key);
        }
      }
    });
    return Array.from(genreSet);
  }, [monthlyData]);
  const chartConfig = createChartConfig(genreNames);

  return (
    <Card className="w-full">
      <CardHeader className="flex items-center gap-5 space-y-0 border-b py-5 sm:flex-row">
        <div className="grid flex-1 gap-1 text-center sm:text-left">
          <CardTitle>Genre Preference</CardTitle>
          <CardDescription>
            Hours played across different genres over the course of the year.
          </CardDescription>
        </div>
        <div className="flex flex-col gap-y-2">
          <Label className="text-center">Year</Label>
          <Select value={timeRange} onValueChange={setTimeRange}>
            <SelectTrigger
              className="w-[160px] rounded-lg sm:ml-auto"
              aria-label="Select time range"
            >
              <SelectValue placeholder="Year" />
            </SelectTrigger>
            <SelectContent className="rounded-xl">
              {Object.entries(stats.hoursPlayedDistributionPerYear).map(
                (entry) => (
                  <SelectItem
                    key={entry[0]}
                    value={`${entry[0]}`}
                    className="rounded-lg"
                  >
                    {entry[0]}
                  </SelectItem>
                )
              )}
            </SelectContent>
          </Select>
        </div>
      </CardHeader>
      <CardContent className="px-2 pt-4 sm:px-6 sm:pt-6">
        <ChartContainer
          config={chartConfig}
          className="aspect-auto h-[300px] w-full"
        >
          <AreaChart data={monthlyData}>
            <defs>
              {genreNames.map((genre) => (
                <linearGradient
                  key={genre}
                  id={`fill${genre.replace(/\s+/g, "")}`}
                  x1="0"
                  y1="0"
                  x2="0"
                  y2="1"
                >
                  <stop
                    offset="5%"
                    stopColor={`var(--color-${genre
                      .toLowerCase()
                      .replace(/\s+/g, "")})`}
                    stopOpacity={0.8}
                  />
                  <stop
                    offset="95%"
                    stopColor={`var(--color-${genre
                      .toLowerCase()
                      .replace(/\s+/g, "")})`}
                    stopOpacity={0.1}
                  />
                </linearGradient>
              ))}
            </defs>
            <CartesianGrid vertical={false} />
            <XAxis
              dataKey="date"
              tickLine={false}
              axisLine={false}
              tickMargin={8}
              minTickGap={32}
              tickFormatter={(value: string) => {
                const date = new Date(value);
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
            {genreNames.map((genre, idx) => (
              <Area
                key={genre}
                dataKey={genre}
                type="natural"
                fill={`var(--chart-${(idx + 1) % 20})`}
                stroke={`var(--chart-${(idx + 1) % 20})`}
                stackId="a"
              />
            ))}
            <ChartLegend
              className="flex-wrap"
              content={<ChartLegendContent />}
            />
          </AreaChart>
        </ChartContainer>
      </CardContent>
    </Card>
  );
};

export const formatYearMonthGenreDataToMonthlyDataPoint = (
  stats: UserGameStats,
  year: string
) => {
  const yearData = stats.hoursPlayedDistributionPerYearPerGenre.filter(
    (entry) => entry.year === year
  );

  const allGenres = new Set(yearData.map((entry) => entry.genreName));

  const monthlyDataMap = new Map();

  yearData.forEach((entry) => {
    const monthKey = `${entry.year}-${entry.month.padStart(2, "0")}`;

    if (!monthlyDataMap.has(monthKey)) {
      const monthData: MonthlyDataPoint = { date: monthKey };
      allGenres.forEach((genre) => {
        monthData[genre] = 0;
      });
      monthlyDataMap.set(monthKey, monthData);
    }

    monthlyDataMap.get(monthKey)[entry.genreName] = entry.hoursPlayed;
  });

  return Array.from(monthlyDataMap.values()).sort((a, b) =>
    a.date.localeCompare(b.date)
  );
};
export default GameStatsDashboard;
