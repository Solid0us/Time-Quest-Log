import { DataItem, TwoColumnTable } from "@/components/TwoColumnTable";
import { UserGameStats } from "@/services/userGameServices";
import { ColumnDef } from "@tanstack/react-table";
import { useMemo, useState } from "react";

interface GeneralStatsTableProps {
  data: UserGameStats;
}

const GeneralStatsTable = ({ data }: GeneralStatsTableProps) => {
  const favoriteGenre = useMemo(() => {
    let genre = "";
    let maxHours = 0;
    for (const [key, value] of Object.entries(data.hoursPlayedPerGenre)) {
      if (value > maxHours) {
        genre = key;
        maxHours = value;
      }
    }
    return genre;
  }, [data.hoursPlayedPerGenre]);

  const favoriteGame = useMemo(() => {
    let favoriteGame = "";
    let maxHours = 0;
    data.hoursPlayedPerGame.forEach((game) => {
      if (game.hoursPlayed > maxHours) {
        favoriteGame = game.gameTitle;
        maxHours = game.hoursPlayed;
      }
    });
    return favoriteGame;
  }, [data.hoursPlayedPerGame]);

  const [tableData, setTableData] = useState<DataItem[]>([
    {
      description: "Total Hours Played",
      value: data.totalHoursPlayed.toFixed(2),
    },
    {
      description: "Games in Library",
      value: data.totalGames,
    },
    {
      description: "Favorite Genre",
      value: favoriteGenre,
    },
    {
      description: "Favorite Game",
      value: favoriteGame,
    },
  ]);
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
  return <TwoColumnTable data={tableData} columns={columns} />;
};

export default GeneralStatsTable;
