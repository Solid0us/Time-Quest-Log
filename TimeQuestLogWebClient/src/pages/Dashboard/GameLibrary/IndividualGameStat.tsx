import { DataItem, TwoColumnTable } from "@/components/TwoColumnTable";
import {
  ChartConfig,
  ChartContainer,
  ChartTooltip,
  ChartTooltipContent,
} from "@/components/ui/chart";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { GameSession, useGetGameSessions } from "@/services/gameSessionService";
import {
  useGetUserIndividualGameStats,
  UserGame,
} from "@/services/userGameServices";
import { ColumnDef, Row } from "@tanstack/react-table";
import React, { useMemo, useState } from "react";
import { CartesianGrid, Line, LineChart, XAxis } from "recharts";
import YearlyChartCard from "../Statistics/YearlyChartCard";
import {
  getTimeDifferenceInMilliSeconds,
  getUTCMonthName,
} from "@/utils/timeUtils";

const chartConfig = {
  hours: {
    label: "Hours",
  },
  desktop: {
    label: "Desktop",
    color: "hsl(var(--chart-1))",
  },
} satisfies ChartConfig;

enum IGDBImageSize {
  CoverSmall = "cover_small",
  ScreenshotMed = "screenshot_med",
  CoverBig = "cover_big",
  LogoMed = "logo_med",
  ScreenshotBig = "screenshot_big",
  ScreenshotHuge = "screenshot_huge",
  Thumbnail = "thumb",
  Micro = "micro",
  HD720P = "720p",
  HD1080P = "1080p",
}

interface IndividualGameStatProps {
  userGameRow: Row<UserGame>;
  isModalOpen: boolean;
  setIsModalOpen: React.Dispatch<React.SetStateAction<boolean>>;
}
const IndividualGameStat = ({
  userGameRow,
  isModalOpen,
  setIsModalOpen,
}: IndividualGameStatProps) => {
  const { data: gameSessionAggregateData } = useGetUserIndividualGameStats(
    userGameRow.original.gameId
  );
  const [year, setYear] = useState(new Date().getUTCFullYear().toString());
  const { data: gameSessions, isFetching: isGameSessionsFetching } =
    useGetGameSessions();
  const monthlyData = useMemo(
    () =>
      formatGameMonthlyData(
        gameSessions?.data ?? [],
        userGameRow.original.gameId,
        year
      ),
    [gameSessions, userGameRow, year]
  );

  const changeIGDBCoverImageSize = (igdbUrl: string, size: IGDBImageSize) => {
    let splitUrl = igdbUrl.split("/");
    if (splitUrl.length === 8) {
      splitUrl[6] = "t_" + size;
      return splitUrl.join("/");
    }
    return igdbUrl;
  };

  const tableData: DataItem[] = useMemo(() => {
    return [
      {
        description: "Total Hours Played",
        value: gameSessionAggregateData
          ? gameSessionAggregateData.data.hoursPlayed.toFixed(2) + " Hours"
          : "No data",
      },
      {
        description: "Last Played",
        value: gameSessionAggregateData
          ? new Date(gameSessionAggregateData.data.lastPlayed).toLocaleString()
          : "No data",
      },
      {
        description: "Average Session Time",
        value: gameSessionAggregateData
          ? gameSessionAggregateData.data.avgSessionTime.toFixed(2) + " Hours"
          : "No data",
      },
      {
        description: "Longest Session Time",
        value: gameSessionAggregateData
          ? `${gameSessionAggregateData?.data.longestSessionTime.toFixed(
              2
            )} Hours (${new Date(
              gameSessionAggregateData.data.longestSessionDate
            ).toLocaleString()})`
          : "No data",
      },

      {
        description: "First Time Played",
        value: gameSessionAggregateData
          ? new Date(
              gameSessionAggregateData.data.firstTimePlayed
            ).toLocaleString()
          : "No data",
      },
    ];
  }, [gameSessionAggregateData]);
  const columns: ColumnDef<DataItem>[] = [
    {
      accessorKey: "description",
      header: "",
    },
    {
      accessorKey: "value",
      header: "",
    },
  ];
  return (
    <Dialog open={isModalOpen} onOpenChange={(e) => setIsModalOpen(e)}>
      <DialogContent
        className="w-full max-w-[80%] h-[90%] overflow-auto flex flex-col gap-10"
        onInteractOutside={(e) => e.preventDefault()}
      >
        <DialogHeader className="space-y-0">
          <DialogTitle className="ml-auto mr-auto">
            {userGameRow.original.gameName}
          </DialogTitle>
          <DialogDescription className="text-center">
            Detailed statistics that highlight your gameplay.
          </DialogDescription>
        </DialogHeader>

        <div className="flex flex-col md:flex-row gap-3">
          <section className="max-w-40 md:max-w-96 flex flex-row ml-auto mr-auto md:ml-0 md:mr-0">
            <img
              draggable={false}
              className="aspect-[9/16] mt-auto mb-auto"
              src={`${changeIGDBCoverImageSize(
                userGameRow.original.coverUrl,
                IGDBImageSize.HD1080P
              )}`}
            />
          </section>
          <section className="w-full">
            <div className="flex flex-col gap-5 items-center justify-center">
              <TwoColumnTable data={tableData} columns={columns} />

              <YearlyChartCard
                chartTitle="Hours Played Over Time"
                chartDescription="df"
                year={year}
                setYear={setYear}
                yearSelectList={["2025"]}
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
            </div>
          </section>
        </div>
      </DialogContent>
    </Dialog>
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

export default IndividualGameStat;
