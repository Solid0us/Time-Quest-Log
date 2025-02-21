import SideNavbar from "@/pages/Dashboard/SideNavbar";
import { Outlet } from "react-router";

const DashboardLayout = () => {
  return (
    <div className="flex w-full h-screen">
      <SideNavbar />
      <Outlet />
    </div>
  );
};

export default DashboardLayout;
