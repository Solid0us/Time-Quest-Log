import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { useGetUserGameStats } from "@/services/userGameServices";
import GeneralStatsTable from "./AllTimeStatsTable";
import TopGamesPieChart from "./TopGamesPieChart";
import TopGenrePieChart from "./TopGenrePieChart";

const StatisticsDashboard = () => {
  const { data } = useGetUserGameStats();
  return (
    <div className="p-10 w-full overflow-auto">
      {data?.data && (
        <>
          <Card>
            <CardHeader>
              <CardTitle className="text-center font-bold text-xl">
                All Time Stats
              </CardTitle>
            </CardHeader>
            <CardContent className="w-full">
              <div className="max-w-96 ml-auto mr-auto border-2 rounded-2xl border-primary">
                <GeneralStatsTable data={data.data} />
              </div>
              <div className="flex flex-col gap-5 lg:flex-row items-center justify-center">
                <TopGamesPieChart data={data.data} />
                <TopGenrePieChart data={data.data} />
              </div>
            </CardContent>
          </Card>
        </>
      )}
    </div>
  );
};

export default StatisticsDashboard;
