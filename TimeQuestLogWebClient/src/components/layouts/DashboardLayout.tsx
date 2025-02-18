import { Outlet } from "react-router";

const DashboardLayout = () => {
  return (
    <div className="w-full h-screen">
      This is the dashboard view!
      <Outlet />
    </div>
  );
};

export default DashboardLayout;
