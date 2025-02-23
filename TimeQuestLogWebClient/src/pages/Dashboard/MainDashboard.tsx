import DataTable from "@/components/DataTable";
import { Button } from "@/components/ui/button";
import { GameSession, useGetGameSessions } from "@/services/gameSessionService";
import {
  getHourMinSecFromMilliseconds,
  getTimeDifferenceInMilliSeconds,
} from "@/utils/timeUtils";
import { ColumnDef } from "@tanstack/react-table";
import { ArrowUpDown, ArrowUpDownIcon } from "lucide-react";

const MainDashboard = () => {
  const { data } = useGetGameSessions();
  const columns: ColumnDef<GameSession>[] = [
    {
      accessorKey: "id",
      header: ({ column }) => {
        return (
          <Button
            variant="ghost"
            onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}
          >
            Id
            <ArrowUpDown className="ml-2 h-4 w-4" />
          </Button>
        );
      },
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
      accessorKey: "startTime",
      header: ({ column }) => {
        return (
          <Button
            variant="ghost"
            onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}
          >
            Start Time
            <ArrowUpDown className="ml-2 h-4 w-4" />
          </Button>
        );
      },
      cell: ({ row }) => new Date(row.original.startTime).toLocaleString(),
    },
    {
      accessorKey: "endTime",
      header: ({ column }) => {
        return (
          <Button
            variant="ghost"
            onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}
          >
            End Time
            <ArrowUpDown className="ml-2 h-4 w-4" />
          </Button>
        );
      },
      cell: ({ row }) =>
        row.original.endTime && new Date(row.original.endTime).toLocaleString(),
    },
    {
      accessorKey: "sessionTime",
      accessorFn: (row) => "Session Time",
      header: ({ column }) => {
        return (
          <Button
            variant="ghost"
            onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}
          >
            Session Time
            <ArrowUpDown className="ml-2 h-4 w-4" />
          </Button>
        );
      },
      cell: ({ row }) => {
        if (row.original.endTime) {
          const milliseconds = getTimeDifferenceInMilliSeconds(
            row.original.startTime,
            row.original.endTime
          );
          const { hours, minutes, seconds } =
            getHourMinSecFromMilliseconds(milliseconds);
          return `${hours} Hr ${minutes} Min ${seconds} Sec`;
        }
        return "In Progress";
      },
      sortingFn: (rowA, rowB) => {
        const timeA = getTimeDifferenceInMilliSeconds(
          rowA.original.startTime,
          rowA.original.endTime ?? new Date(-8640000000000000)
        );
        const timeB = getTimeDifferenceInMilliSeconds(
          rowB.original.startTime,
          rowB.original.endTime ?? new Date(-8640000000000000)
        );

        return timeA - timeB;
      },
    },
  ];
  return (
    <div className="p-5 w-full">
      {data && <DataTable columns={columns} data={data?.data ?? []} />}
    </div>
  );
};

export default MainDashboard;
