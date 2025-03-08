import { useTheme } from "@/context/theme-provider";
import { UserGame } from "@/services/userGameServices";
import { Row } from "@tanstack/react-table";

interface GameModalNavigationProps {
  userGameRow: Row<UserGame>;
  getPrevItem: () => void;
  getNextItem: () => void;
  data: UserGame[];
}

const GameModalNavigation = ({
  getNextItem,
  getPrevItem,
  userGameRow,
  data,
}: GameModalNavigationProps) => {
  const { theme } = useTheme();
  return (
    <div className="flex flex-row gap-5 justify-center">
      <img
        className={`size-10 md:size-16 ${
          userGameRow.index === 0
            ? "opacity-30 pointer-events-none"
            : "cursor-pointer hover:scale-105 duration-100"
        }`}
        src={`/iconmonstr-arrow-left-circle-filled-96${
          theme === "dark" ? "-dark" : ""
        }.png`}
        onClick={getPrevItem}
      />
      <img
        className={`size-10 md:size-16 ${
          userGameRow.index === data.length - 1
            ? "opacity-30 pointer-events-none"
            : "cursor-pointer hover:scale-105 duration-100"
        }`}
        src={`/iconmonstr-arrow-right-circle-filled-96${
          theme === "dark" ? "-dark" : ""
        }.png`}
        onClick={getNextItem}
      />
    </div>
  );
};

export default GameModalNavigation;
