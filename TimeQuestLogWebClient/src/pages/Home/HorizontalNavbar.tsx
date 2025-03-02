import ThemeModeToggle from "../../components/ThemeModeToggle";
import { Button } from "../../components/ui/button";
import LoginForm from "./LoginForm";
import SignupForm from "./SignupForm";
import LogoutButton from "@/components/LogoutButton";
import { useAuth } from "@/hooks/useAuth";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import AuthDialogForm from "./AuthDialogForm";

const HorizontalNavbar = () => {
  const { refreshToken } = useAuth();
  return (
    <div className="z-50 w-full bg-secondary pt-3 pb-3 pl-2 pr-2 flex items-center justify-between fixed top-0">
      <div className="flex md:hidden">
        <DropdownMenu>
          <DropdownMenuTrigger asChild>
            <Button variant="outline" size="icon">
              <img src="/iconmonstr-menu-6-96-dark.png" />
            </Button>
          </DropdownMenuTrigger>
          <DropdownMenuContent align="end">
            {refreshToken === null ? (
              <div className="flex flex-col gap-3">
                <AuthDialogForm
                  title="Register"
                  description="Please enter your credentials!"
                  triggerText="Register"
                >
                  <SignupForm />
                </AuthDialogForm>
                <AuthDialogForm
                  title="Sign In"
                  description="Welcome back! Please sign in with your username and password."
                  triggerText="Log In"
                >
                  <LoginForm />
                </AuthDialogForm>
              </div>
            ) : (
              <div className="flex flex-col gap-3">
                <a href="/dashboard/home">
                  <Button variant={"outline"}>Go To Dashboard</Button>
                </a>

                <LogoutButton />
              </div>
            )}
            <div className="pt-3">
              <ThemeModeToggle />
            </div>
          </DropdownMenuContent>
        </DropdownMenu>
      </div>
      <div className="flex gap-2 items-center">
        <img draggable={false} src="gamepad-logo.png" className="size-6" />
        <h1 className="font-bold text-xl">TimeQuest Log</h1>
      </div>
      <div className="hidden md:flex gap-2 items-center">
        {refreshToken === null ? (
          <>
            <AuthDialogForm
              title="Register"
              description="Please enter your credentials!"
              triggerText="Register"
            >
              <SignupForm />
            </AuthDialogForm>
            <AuthDialogForm
              title="Sign In"
              description="Welcome back! Please sign in with your username and password."
              triggerText="Log In"
            >
              <LoginForm />
            </AuthDialogForm>
          </>
        ) : (
          <>
            <a href="/dashboard/home">
              <Button variant={"outline"}>Go To Dashboard</Button>
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
