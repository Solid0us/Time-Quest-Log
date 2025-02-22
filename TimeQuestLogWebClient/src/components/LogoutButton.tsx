import { useNavigate } from "react-router";
import { Button } from "./ui/button";
import { useAuth } from "@/hooks/useAuth";

const LogoutButton = () => {
  const { logout } = useAuth();
  const navigate = useNavigate();
  const logoutUser = async () => {
    logout();
    navigate("/");
    navigate(0);
  };
  return (
    <Button onClick={logoutUser} variant={"destructive"}>
      Logout
    </Button>
  );
};

export default LogoutButton;
