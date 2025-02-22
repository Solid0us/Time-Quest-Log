import ThemeModeToggle from "../../components/ThemeModeToggle";
import { Button } from "../../components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogTrigger,
} from "@/components/ui/dialog";
import LoginForm from "./LoginForm";
import { DialogTitle } from "@radix-ui/react-dialog";
import SignupForm from "./SignupForm";
import LogoutButton from "@/components/LogoutButton";
import { useAuth } from "@/hooks/useAuth";

const HorizontalNavbar = () => {
  const { refreshToken } = useAuth();
  return (
    <div className="z-50 w-full bg-secondary pt-3 pb-3 pl-2 pr-2 flex items-center justify-between fixed top-0">
      <div className="flex gap-2 items-center">
        <img draggable={false} src="gamepad-logo.png" className="size-6" />
        <h1 className="font-bold text-xl">TimeQuest Log</h1>
      </div>
      <div className="flex gap-2 items-center">
        {refreshToken === null ? (
          <>
            <Dialog>
              <DialogTrigger asChild>
                <Button variant={"outline"}>Register</Button>
              </DialogTrigger>
              <DialogContent
                onOpenAutoFocus={(e) => e.preventDefault()}
                onInteractOutside={(e) => e.preventDefault()}
              >
                <DialogTitle className="font-bold text-xl text-center">
                  Register
                </DialogTitle>
                <DialogDescription className="text-foreground text-base text-center">
                  Please enter your credentials!
                </DialogDescription>
                <SignupForm />
              </DialogContent>
            </Dialog>
            <Dialog>
              <DialogTrigger asChild>
                <Button>Log in</Button>
              </DialogTrigger>
              <DialogContent
                onOpenAutoFocus={(e) => e.preventDefault()}
                onInteractOutside={(e) => e.preventDefault()}
              >
                <DialogTitle className="font-bold text-xl text-center">
                  Sign In
                </DialogTitle>
                <DialogDescription className="text-foreground text-base text-center">
                  Welcome back! Please sign in with your username and password.
                </DialogDescription>
                <LoginForm />
              </DialogContent>
            </Dialog>
          </>
        ) : (
          <>
            <a href="/dashboard/home">
              <Button>Go To Dashboard</Button>
            </a>
            <LogoutButton />
          </>
        )}

        <ThemeModeToggle />
      </div>
    </div>
  );
};

export default HorizontalNavbar;
