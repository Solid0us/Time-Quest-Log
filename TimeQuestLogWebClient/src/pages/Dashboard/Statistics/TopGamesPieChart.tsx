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
  ChartLegend,
  ChartLegendContent,
  ChartTooltip,
  ChartTooltipContent,
} from "@/components/ui/chart";
import { UserGameStats } from "@/services/userGameServices";
import { useMemo } from "react";
import { Pie, PieChart, Cell } from "recharts";

interface TopGamesPieChartProps {
  data: UserGameStats;
}

const COLORS = [
  "var(--chart-1)",
  "var(--chart-2)",
  "var(--chart-3)",
  "var(--chart-4)",
  "var(--chart-5)",
];

const TopGamesPieChart = ({ data }: TopGamesPieChartProps) => {
  const { chartData, chartConfig } = useMemo(() => {
    const slicedArray = data.hoursPlayedPerGame
      .sort((a, b) => b.hoursPlayed - a.hoursPlayed)
      .slice(0, 5);

    const config: ChartConfig = {};
    const dataToPlot = slicedArray.map((item, index) => ({
      gameTitle: item.gameTitle,
      hoursPlayed: item.hoursPlayed,
      fill: COLORS[index % COLORS.length],
    }));

    slicedArray.forEach((item, index) => {
      config[item.gameTitle] = {
        label: item.gameTitle,
        color: COLORS[index % COLORS.length],
      };
    });

    return { chartData: dataToPlot, chartConfig: config };
  }, [data.hoursPlayedPerGame]);

  return (
    <Card className="w-full">
      <CardHeader>
        <CardTitle className="text-center font-bold text-xl">
          Top 5 Games
        </CardTitle>
        <CardDescription className="text-center">
          Games with the most hours played
        </CardDescription>
      </CardHeader>
      <CardContent>
        <ChartContainer
          config={chartConfig}
          className="mx-auto aspect-square min-h-96 md:max-h-[350px] w-full"
        >
          <PieChart>
            <Pie
              data={chartData}
              dataKey="hoursPlayed"
              nameKey="gameTitle"
              innerRadius={55}
              outerRadius={80}
              paddingAngle={5}
              stroke="none"
            >
              {chartData.map((entry, index) => (
                <Cell key={`cell-${index}`} fill={entry.fill} />
              ))}
            </Pie>
            <ChartTooltip
              content={<ChartTooltipContent hideLabel />}
              cursor={{ fill: "transparent" }}
            />
            <ChartLegend
              className="flex flex-col lg:flex-row items-center text-[0.6rem] md:text-xs"
              content={<ChartLegendContent />}
            />
          </PieChart>
        </ChartContainer>
      </CardContent>
    </Card>
  );
};

export default TopGamesPieChart;
