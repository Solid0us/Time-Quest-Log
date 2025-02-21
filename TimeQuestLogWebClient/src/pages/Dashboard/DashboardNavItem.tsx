import { useTheme } from "@/context/theme-provider";
import { useLocation } from "react-router-dom";

type DashboardNavItemProps = {
  relativePath: string;
  lightModeImageUrl: string;
  darkModeImageUrl: string;
  text: string;
};

const DashboardNavItem = ({
  relativePath,
  lightModeImageUrl,
  darkModeImageUrl,
  text,
}: DashboardNavItemProps) => {
  const { theme } = useTheme();
  const location = useLocation();

  const isRelativePathMatch = () => {
    const paths = location.pathname.split("/");
    const currentRelativePath = paths[paths.length - 1];
    if (currentRelativePath === relativePath) return true;
    return false;
  };

  return (
    <div
      draggable={false}
      className={`cursor-pointer flex rounded-md p-2 duration-200 hover:ring ring-primary items-center gap-x-3 ${
        isRelativePathMatch() && "ring-[2px] hover:ring-[2px]"
      }`}
    >
      <img
        draggable={false}
        className="max-w-8"
        src={theme === "dark" ? darkModeImageUrl : lightModeImageUrl}
      />
      <p className="text-xl">{text}</p>
    </div>
  );
};

export default DashboardNavItem;
