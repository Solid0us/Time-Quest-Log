import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import {
  ChartConfig,
  ChartContainer,
  ChartTooltip,
  ChartTooltipContent,
} from "@/components/ui/chart";
import { GameSession } from "@/services/gameSessionService";
import { useMemo } from "react";
import { Area, AreaChart, XAxis, YAxis } from "recharts";

const chartConfig = {
  hours: {
    label: "Hours",
    color: "var(--chart-1)",
  },
} satisfies ChartConfig;

interface WeeklyHoursChartProps {
  data: GameSession[];
}

type WeeklyHoursChartData = {
  day: string;
  hours: number;
};

const WeeklyHoursChart = ({ data }: WeeklyHoursChartProps) => {
  const weeklyData: WeeklyHoursChartData[] = useMemo(() => {
    const daysOfWeek = ["Sun", "Mon", "Tues", "Wed", "Thur", "Fri", "Sat"];
    let weeklyDataArray: WeeklyHoursChartData[] = daysOfWeek.map((day) => ({
      day,
      hours: 0,
    }));

    const currentDate = new Date();
    const currentDay = currentDate.getDay();
    const startGraphDay =
      currentDate.getTime() - currentDay * 24 * 60 * 60 * 1000;
    const endGraphDay =
      currentDate.getTime() + (7 - currentDay) * 24 * 60 * 60 * 1000;

    for (let i = 0; i < data.length; i++) {
      const sessionStartDate = new Date(data[i].startTime);
      if (
        sessionStartDate.getTime() >= startGraphDay &&
        sessionStartDate.getTime() <= endGraphDay &&
        data[i].endTime
      ) {
        const hoursPlayed =
          (new Date(data[i].endTime as Date).getTime() -
            sessionStartDate.getTime()) /
          (1000 * 60 * 60);
        weeklyDataArray[sessionStartDate.getDay()].hours += hoursPlayed;
      }
    }

    return weeklyDataArray;
  }, [data]);
  return (
    <Card>
      <CardHeader>
        <CardTitle>Hours Played This Week</CardTitle>
        <CardDescription>
          You have played{" "}
          {weeklyData.reduce((prev, curr) => prev + curr.hours, 0).toFixed(2)}{" "}
          hours this week!
        </CardDescription>
      </CardHeader>
      <CardContent>
        <ChartContainer
          className="w-full h-[300px] aspect-auto"
          config={chartConfig}
        >
          <AreaChart accessibilityLayer data={weeklyData}>
            <XAxis
              dataKey="day"
              tickLine={false}
              axisLine={false}
              tickMargin={8}
              tickFormatter={(value) => value.slice(0, 3)}
            />
            <YAxis dataKey="hours" />
            <ChartTooltip
              cursor={false}
              content={<ChartTooltipContent indicator="dot" hideLabel />}
            />
            <Area
              dataKey="hours"
              type="step"
              fill="var(--color-hours)"
              fillOpacity={0.4}
              stroke="var(--color-hours)"
            />
          </AreaChart>
        </ChartContainer>
      </CardContent>
    </Card>
  );
};

export default WeeklyHoursChart;
