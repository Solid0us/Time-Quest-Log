import LogoutButton from "@/components/LogoutButton";
import { getJwtPayload } from "@/utils/jwtUtils";
import DashboardNavItem from "./DashboardNavItem";
import { Link } from "react-router-dom";
import { useAuth } from "@/hooks/useAuth";
import { useState } from "react";
import { useTheme } from "@/context/theme-provider";

const SideNavbar = () => {
  const { jwt } = useAuth();
  const [showNav, setShowNav] = useState(false);
  const { theme } = useTheme();
  return (
    <>
      <div
        className="rounded-full cursor-pointer z-[1000] md:invisible"
        onClick={() => setShowNav((prevState) => !prevState)}
      >
        {showNav ? (
          <img
            className={`fixed left-0 top-0 size-10 ml-2 mt-2 z-50`}
            src={`/iconmonstr-x-mark-circle-lined-96${
              theme === "dark" ? "-dark" : ""
            }.png`}
            alt="Close Menu"
          />
        ) : (
          <img
            className={`fixed left-0 top-0 size-10 ml-2 mt-2 z-50`}
            src={`/iconmonstr-menu-6-96${theme === "dark" ? "-dark" : ""}.png`}
            alt="Open Menu"
          />
        )}
      </div>

      <nav
        className={`z-50 fixed md:static left-0 bg-secondary h-screen min-w-60 flex flex-col p-3 ${
          showNav ? "translate-x-0" : "-translate-x-full"
        } md:translate-x-0 transition-transform duration-300 ease-in-out md:bottom-0 gap-y-5`}
      >
        {jwt !== null && (
          <p className="text-center capitalize font-bold text-2xl md:text-4xl">
            {getJwtPayload(jwt)?.sub}
          </p>
        )}
        <nav className="h-full flex flex-col gap-y-5 pt-5 pb-5 pl-2 pr-2 overflow-y-auto">
          <Link draggable={false} to="/dashboard/home">
            <DashboardNavItem
              relativePath="home"
              lightModeImageUrl="/iconmonstr-dashboard-filled-96.png"
              darkModeImageUrl="/iconmonstr-dashboard-filled-96-dark.png"
              text="Game Sessions"
            />
          </Link>
          <Link draggable={false} to="/dashboard/statistics">
            <DashboardNavItem
              relativePath="statistics"
              lightModeImageUrl="/iconmonstr-chart-5-96.png"
              darkModeImageUrl="/iconmonstr-chart-5-96-dark.png"
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
              lightModeImageUrl="/iconmonstr-gear-3-96.png"
              darkModeImageUrl="/iconmonstr-gear-3-96-dark.png"
              text="Settings"
            />
          </Link>
          <Link className="mt-auto" draggable={false} to="/">
            <DashboardNavItem
              relativePath=""
              lightModeImageUrl="/iconmonstr-home-7-96.png"
              darkModeImageUrl="/iconmonstr-home-7-96-dark.png"
              text="Home"
            />
          </Link>
        </nav>
        <LogoutButton />
      </nav>
    </>
  );
};

export default SideNavbar;
