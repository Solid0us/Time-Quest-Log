import { useMemo, useState } from "react";
import { Area, AreaChart, CartesianGrid, XAxis, YAxis } from "recharts";
import {
  ChartContainer,
  ChartLegend,
  ChartLegendContent,
  ChartTooltip,
  ChartTooltipContent,
} from "@/components/ui/chart";
import { UserGameStats } from "@/services/userGameServices";
import { getUTCMonthName } from "@/utils/timeUtils";
import AreaChartContainer from "./AreaChartContainer";

type ChartSeriesConfig = {
  label: string;
  color?: string;
  valueFormatter?: (value: number) => string;
};

type GenreChartConfig = {
  [K in string]: ChartSeriesConfig;
};

interface GenresMonthlyDataPoint {
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

const GenresPlayedOvertimeAreaChart = ({ stats }: { stats: UserGameStats }) => {
  const [year, setYear] = useState(`${new Date().getFullYear()}`);

  const monthlyData: GenresMonthlyDataPoint[] = useMemo(
    () => formatYearMonthGenreDataToMonthlyDataPoint(stats, year),
    [stats.hoursPlayedDistributionPerYearPerGenre, year]
  );

  const genreNames = useMemo(() => {
    let genreSet: Set<string> = new Set();
    monthlyData.forEach((data) => {
      for (const [key] of Object.entries(data)) {
        if (key !== "date") {
          genreSet.add(key);
        }
      }
    });
    return Array.from(genreSet);
  }, [monthlyData]);
  const chartConfig = createChartConfig(genreNames);
  console.log(monthlyData);
  return (
    <AreaChartContainer
      chartTitle="Genre Preference"
      chartDescription="Hours played across different genres over the course of the year."
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
            minTickGap={5}
            tickFormatter={(value: string) => {
              return getUTCMonthName(value, "short");
            }}
          />
          <YAxis
            tickLine={false}
            axisLine={false}
            tickMargin={8}
            domain={[0, "auto"]}
            width={50}
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
              type="monotone"
              fill={`var(--chart-${(idx + 1) % 20})`}
              stroke={`var(--chart-${(idx + 1) % 20})`}
              stackId="a"
            />
          ))}
          <ChartLegend className="flex-wrap" content={<ChartLegendContent />} />
        </AreaChart>
      </ChartContainer>
    </AreaChartContainer>
  );
};

export const formatYearMonthGenreDataToMonthlyDataPoint = (
  stats: UserGameStats,
  year: string
) => {
  const allGenres = new Set<string>();
  stats.hoursPlayedDistributionPerYearPerGenre.forEach((entry) => {
    if (year === entry.year) {
      allGenres.add(entry.genreName);
    }
  });

  const yearData = stats.hoursPlayedDistributionPerYearPerGenre.filter(
    (entry) => entry.year === year
  );

  const monthlyDataMap = new Map();
  for (let month = 1; month <= 12; month++) {
    const monthKey = `${year}-${month.toString().padStart(2, "0")}`;
    const monthData: GenresMonthlyDataPoint = { date: monthKey };

    allGenres.forEach((genre) => {
      monthData[genre] = 0;
    });

    monthlyDataMap.set(monthKey, monthData);
  }

  yearData.forEach((entry) => {
    const monthKey = `${entry.year}-${entry.month.padStart(2, "0")}`;
    if (monthlyDataMap.has(monthKey)) {
      monthlyDataMap.get(monthKey)[entry.genreName] = entry.hoursPlayed;
    }
  });

  return Array.from(monthlyDataMap.values()).sort((a, b) =>
    a.date.localeCompare(b.date)
  );
};
export default GenresPlayedOvertimeAreaChart;
