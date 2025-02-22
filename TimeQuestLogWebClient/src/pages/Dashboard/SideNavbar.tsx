import LogoutButton from "@/components/LogoutButton";
import { getJwtPayload } from "@/utils/jwtUtils";
import DashboardNavItem from "./DashboardNavItem";
import ThemeModeToggle from "@/components/ThemeModeToggle";
import { Link } from "react-router-dom";
import { useAuth } from "@/hooks/useAuth";

const SideNavbar = () => {
  const { jwt } = useAuth();
  return (
    <div className="static left-0 bg-secondary h-screen min-w-60 flex flex-col p-3">
      {jwt !== null && (
        <p className="text-center capitalize font-bold text-2xl md:text-4xl">
          {getJwtPayload(jwt)?.sub}
        </p>
      )}
      <nav className="h-full flex flex-col gap-y-5 pt-5 pb-5 pl-2 pr-2 overflow-y-auto">
        <Link draggable={false} to="/dashboard/home">
          <DashboardNavItem
            relativePath="home"
            lightModeImageUrl="/iconmonstr-home-7-96.png"
            darkModeImageUrl="/iconmonstr-home-7-96-dark.png"
            text="Main"
          />
        </Link>
        <Link draggable={false} to="/dashboard/statistics">
          <DashboardNavItem
            relativePath="statistics"
            lightModeImageUrl="/iconmonstr-bar-chart-thin-96.png"
            darkModeImageUrl="/iconmonstr-bar-chart-thin-240-dark.png"
            text="Statistics"
          />
        </Link>
        <Link draggable={false} to="/dashboard/library">
          <DashboardNavItem
            relativePath="library"
            lightModeImageUrl="/iconmonstr-gamepad-17-96.png"
            darkModeImageUrl="/iconmonstr-gamepad-17-96-dark.png"
            text="Game Library"
          />
        </Link>
        <Link draggable={false} to="/dashboard/settings">
          <DashboardNavItem
            relativePath="settings"
            lightModeImageUrl="/iconmonstr-gear-thin-96.png"
            darkModeImageUrl="/iconmonstr-gear-thin-96-dark.png"
            text="Settings"
          />
        </Link>
      </nav>
      <ThemeModeToggle />
      <LogoutButton />
    </div>
  );
};

export default SideNavbar;
