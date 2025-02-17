import ThemeModeToggle from "./ThemeModeToggle";
import { Button } from "./ui/button";

const HorizontalNavbar = () => {
  return (
    <div className="z-50 w-full bg-secondary pt-3 pb-3 pl-2 pr-2 flex items-center justify-between fixed top-0">
      <div className="flex gap-2">
        <img draggable={false} src="gamepad-logo.png" className="size-8" />
        <h1 className="font-bold text-xl">TimeQuest Log</h1>
      </div>
      <div className="flex gap-2 items-center">
        <Button variant={"outline"}>Register</Button>
        <Button>Log in</Button>
        <ThemeModeToggle />
      </div>
    </div>
  );
};

export default HorizontalNavbar;
