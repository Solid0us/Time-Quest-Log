import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { useGetUserGameStats } from "@/services/userGameServices";
import GeneralStatsTable from "./AllTimeStatsTable";
import TopGamesPieChart from "./TopGamesPieChart";
import TopGenrePieChart from "./TopGenrePieChart";
import GenresPlayedOvertimeAreaChart from "./GenresPlayedOvertimeAreaChart";
import GameHoursPlayedOverTimeAreaChart from "./GameHoursPlayedOverTimeAreaChart";

const StatisticsDashboard = () => {
  const { data } = useGetUserGameStats();
  return (
    <div className="p-3 md:p-10 w-full overflow-auto flex flex-col gap-5">
      {data?.data && (
        <>
          <Card>
            <CardHeader>
              <CardTitle className="text-center font-bold text-xl">
                All Time Stats
              </CardTitle>
            </CardHeader>
            <CardContent className="w-full flex flex-col gap-y-5">
              <div className="max-w-96 ml-auto mr-auto border-2 rounded-2xl border-primary">
                <GeneralStatsTable data={data.data} />
              </div>
              <div className="flex flex-col gap-5 lg:flex-row items-center justify-center">
                <TopGamesPieChart data={data.data} />
                <TopGenrePieChart data={data.data} />
              </div>
            </CardContent>
          </Card>
          <Card>
            <CardHeader>
              <CardTitle className="text-center font-bold text-xl">
                Gaming Habits Over Time
              </CardTitle>
            </CardHeader>
            <CardContent className="w-full">
              <div className="flex flex-col gap-5 items-center justify-center">
                <GenresPlayedOvertimeAreaChart stats={data.data} />
                <GameHoursPlayedOverTimeAreaChart stats={data.data} />
              </div>
            </CardContent>
          </Card>
        </>
      )}
    </div>
  );
};

export default StatisticsDashboard;
