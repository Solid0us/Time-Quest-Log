import ThemeModeToggle from "@/components/ThemeModeToggle";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { useGetUserDetails } from "@/services/userServices";
import DashboardTitle from "./DashboardTitle";

const SettingsDashboard = () => {
  const { data, isFetching } = useGetUserDetails();
  return (
    <div className="p-3 md:p-10 w-screen">
      <DashboardTitle text="Settings" />
      {isFetching && "Fetching..."}
      {data && (
        <>
          <section className="flex flex-col md:flex-row gap-x-10 justify-center">
            <div>
              <Label>Username</Label>
              <Input value={data?.data.username} readOnly={true} />

              <Label>First Name</Label>
              <Input value={data?.data.firstName} readOnly={true} />
            </div>
            <div>
              <Label>Last Name</Label>
              <Input value={data?.data.lastName} readOnly={true} />

              <Label>Email</Label>
              <Input value={data?.data.email} readOnly={true} />
            </div>
          </section>
          <div className="p-5 gap-3 flex items-center justify-center">
            <Label>Theme</Label>
            <ThemeModeToggle />
          </div>
        </>
      )}
    </div>
  );
};

export default SettingsDashboard;
