import { useGetUserGames, UserGame } from "@/services/userGameServices";
import { ColumnDef, Row, Table } from "@tanstack/react-table";
import DataTable from "../../../components/DataTable";
import { ArrowUpDown } from "lucide-react";
import { Button } from "@/components/ui/button";
import DashboardTitle from "../DashboardTitle";
import { useState } from "react";
import IndividualGameStat from "./IndividualGameStat";

const GameLibraryDashboard = () => {
  const { data } = useGetUserGames();
  const [gameLibraryTable, setGameLibraryTable] = useState<Table<UserGame>>();
  const columns: ColumnDef<UserGame>[] = [
    {
      accessorKey: "coverUrl",
      header: ({ column }) => {
        return <div className="min-w-24"></div>;
      },
      cell: ({ row }) => (
        <img
          src={row.original.coverUrl}
          alt={row.original.gameName}
          className="size-24 object-cover rounded-md"
        />
      ),
    },
    {
      accessorKey: "gameName",
      header: ({ column }) => {
        return (
          <Button
            variant="ghost"
            onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}
          >
            Title
            <ArrowUpDown className="ml-2 h-4 w-4" />
          </Button>
        );
      },
    },
    {
      accessorKey: "genres",
      header: ({ column }) => {
        return (
          <Button
            variant="ghost"
            onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}
          >
            Genres
            <ArrowUpDown className="ml-2 h-4 w-4" />
          </Button>
        );
      },
      cell: ({ row }) => <p className="max-w-60">{row.original.genres}</p>,
    },
    {
      accessorKey: "exeName",
      header: ({ column }) => {
        return (
          <Button
            variant="ghost"
            onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}
          >
            Exe Name
            <ArrowUpDown className="ml-2 h-4 w-4" />
          </Button>
        );
      },
    },
    {
      accessorKey: "hoursPlayed",
      header: ({ column }) => {
        return (
          <Button
            variant="ghost"
            onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}
          >
            Hours Played
            <ArrowUpDown className="ml-2 h-4 w-4" />
          </Button>
        );
      },
      cell: ({ row }) => <>{row.original.hoursPlayed.toFixed(2)}</>,
    },
  ];
  const [selectedRow, setSelectedRow] = useState<Row<UserGame>>();

  const [isModalOpen, setIsModalOpen] = useState(false);

  const openIndividualGameModal = (userGameRow: Row<UserGame>) => {
    setIsModalOpen(true);
    setSelectedRow(userGameRow);
  };

  return (
    <div className="p-3 md:p-10 w-full overflow-auto">
      {selectedRow && (
        <IndividualGameStat
          isModalOpen={isModalOpen}
          setIsModalOpen={setIsModalOpen}
          userGameRow={selectedRow}
          setUserGameRow={setSelectedRow}
          data={data?.data ?? []}
          gameLibraryTable={gameLibraryTable}
        />
      )}
      <DashboardTitle text="Game Library" />
      {data && (
        <DataTable
          columns={columns}
          data={data?.data ?? []}
          onRowClick={openIndividualGameModal}
          setTableData={setGameLibraryTable}
        />
      )}
    </div>
  );
};

export default GameLibraryDashboard;
