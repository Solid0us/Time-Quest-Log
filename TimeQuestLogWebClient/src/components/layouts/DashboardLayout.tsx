import { useAuth } from "@/hooks/useAuth";
import SideNavbar from "@/pages/Dashboard/SideNavbar";
import { Navigate, Outlet } from "react-router";

const DashboardLayout = () => {
  const { isAuthenticated } = useAuth();
  if (!isAuthenticated) {
    return <Navigate to={"/"} replace />;
  }
  return (
    <div className="flex w-full h-screen">
      <SideNavbar />
      <Outlet />
    </div>
  );
};

export default DashboardLayout;
