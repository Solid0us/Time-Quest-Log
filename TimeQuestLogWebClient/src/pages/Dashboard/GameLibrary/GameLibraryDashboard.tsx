import { useGetUserGames, UserGame } from "@/services/userGameServices";
import { ColumnDef } from "@tanstack/react-table";
import GameLibraryTable from "./GameLibraryTable";
import { ArrowUpDown } from "lucide-react";
import { Button } from "@/components/ui/button";

const GameLibraryDashboard = () => {
  const { data } = useGetUserGames();
  const columns: ColumnDef<UserGame>[] = [
    {
      accessorKey: "game.coverUrl",
      header: "",
      cell: ({ row }) => (
        <img
          src={row.original.game.coverUrl}
          alt={row.original.game.name}
          className="size-24 object-cover rounded-md"
        />
      ),
    },
    {
      accessorKey: "game.name",
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
      accessorKey: "game.genres",
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
      cell: ({ row }) =>
        row.original.game.genres.map((genre) => genre.name).join(", "),
      sortingFn: (rowA, rowB) => {
        const genresA = rowA.original.game.genres.map((g) => g.name).join(", ");
        const genresB = rowB.original.game.genres.map((g) => g.name).join(", ");
        return genresA.localeCompare(genresB);
      },
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
  ];

  return (
    <div className="p-5 w-full">
      {data && <GameLibraryTable columns={columns} data={data?.data ?? []} />}
    </div>
  );
};

export default GameLibraryDashboard;
