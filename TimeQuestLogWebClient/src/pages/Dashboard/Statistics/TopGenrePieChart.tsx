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
import { Cell, Pie, PieChart } from "recharts";

interface TopGenrePieChartProps {
  data: UserGameStats;
}

const COLORS = [
  "var(--chart-1)",
  "var(--chart-2)",
  "var(--chart-3)",
  "var(--chart-4)",
  "var(--chart-5)",
];
const TopGenrePieChart = ({ data }: TopGenrePieChartProps) => {
  const { chartData, chartConfig } = useMemo(() => {
    let dataArray: { genreName: string; hoursPlayed: number }[] = [];
    for (const [key, value] of Object.entries(data.hoursPlayedPerGenre)) {
      dataArray.push({
        genreName: key,
        hoursPlayed: value,
      });
    }
    dataArray.sort((a, b) => b.hoursPlayed - a.hoursPlayed);
    const slicedArray = dataArray.slice(0, 5);

    const config: ChartConfig = {};
    const dataToPlot = slicedArray.map((item, index) => ({
      genreName: item.genreName,
      hoursPlayed: item.hoursPlayed,
      fill: COLORS[index % COLORS.length],
    }));

    slicedArray.forEach((item, index) => {
      config[item.genreName] = {
        label: item.genreName,
        color: COLORS[index % COLORS.length],
      };
    });

    return { chartData: dataToPlot, chartConfig: config };
  }, [data.hoursPlayedPerGenre]);
  return (
    <Card className="w-full">
      <CardHeader>
        <CardTitle className="text-center font-bold text-xl">
          Top 5 Genres
        </CardTitle>
        <CardDescription className="text-center">
          Genres with the most hours played
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
              nameKey="genreName"
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
              className="flex-wrap"
              content={<ChartLegendContent />}
            />
          </PieChart>
        </ChartContainer>
      </CardContent>
    </Card>
  );
};

export default TopGenrePieChart;
