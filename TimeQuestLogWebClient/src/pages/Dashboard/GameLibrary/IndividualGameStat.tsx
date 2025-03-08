import { DataItem, TwoColumnTable } from "@/components/TwoColumnTable";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import {
  useGetUserIndividualGameStats,
  UserGame,
} from "@/services/userGameServices";
import { ColumnDef, Row, Table } from "@tanstack/react-table";
import React, { useMemo } from "react";
import YearlyChart from "./YearlyChart";
import GameModalNavigation from "./GameModalNavigation";

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
  setUserGameRow: React.Dispatch<
    React.SetStateAction<Row<UserGame> | undefined>
  >;
  isModalOpen: boolean;
  setIsModalOpen: React.Dispatch<React.SetStateAction<boolean>>;
  data: UserGame[];
  gameLibraryTable?: Table<UserGame>;
}
const IndividualGameStat = ({
  userGameRow,
  setUserGameRow,
  isModalOpen,
  setIsModalOpen,
  data,
  gameLibraryTable,
}: IndividualGameStatProps) => {
  const { data: gameSessionAggregateData } = useGetUserIndividualGameStats(
    userGameRow.original.gameId
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

  const getNextItem = () => {
    if (gameLibraryTable) {
      setUserGameRow(
        gameLibraryTable.getCoreRowModel().rows[userGameRow.index + 1]
      );
    }
  };

  const getPrevItem = () => {
    if (gameLibraryTable) {
      setUserGameRow(
        gameLibraryTable.getCoreRowModel().rows[userGameRow.index - 1]
      );
    }
  };

  return (
    <Dialog open={isModalOpen} onOpenChange={(e) => setIsModalOpen(e)}>
      <DialogContent
        className="w-full max-w-[95%] md:max-w-[80%] h-[90%] overflow-auto flex flex-col gap-10"
        onInteractOutside={(e) => e.preventDefault()}
      >
        <DialogHeader className="space-y-0">
          <DialogTitle className="ml-auto mr-auto">
            {userGameRow.original.gameName}
          </DialogTitle>
          <DialogDescription className="text-center">
            Detailed statistics that highlight your gameplay.
          </DialogDescription>
          <GameModalNavigation
            data={data}
            getNextItem={getNextItem}
            getPrevItem={getPrevItem}
            userGameRow={userGameRow}
          />
        </DialogHeader>

        <div className="flex flex-col lg:flex-row gap-3">
          <section className="max-w-50 lg:max-w-96 flex flex-row ml-auto mr-auto lg:ml-0 lg:mr-0 border rounded-md w-fit h-fit ">
            <img
              draggable={false}
              className="aspect-auto lg:aspect-[9/16] mt-auto mb-auto rounded-md"
              src={`${changeIGDBCoverImageSize(
                userGameRow.original.coverUrl,
                IGDBImageSize.HD1080P
              )}`}
            />
          </section>
          <section className="w-full">
            <div className="flex flex-col gap-5 items-center justify-center">
              <div className="border rounded-md w-full">
                <TwoColumnTable data={tableData} columns={columns} />
              </div>
              <YearlyChart userGameRow={userGameRow} />
            </div>
          </section>
        </div>
      </DialogContent>
    </Dialog>
  );
};

export default IndividualGameStat;
