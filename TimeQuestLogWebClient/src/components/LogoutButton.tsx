import { useNavigate } from "react-router";
import { Button } from "./ui/button";
import useLocalStorage from "@/hooks/useLocalStorage";

const LogoutButton = () => {
  const [jwt, setJwt] = useLocalStorage<string | null>("jwt", null);
  const [refreshToken, setRefreshToken] = useLocalStorage<string | null>(
    "refreshToken",
    null
  );
  const navigate = useNavigate();
  const logout = async () => {
    setJwt(null);
    setRefreshToken(null);
    navigate("/");
    navigate(0);
  };
  return (
    <Button onClick={logout} variant={"destructive"}>
      Logout
    </Button>
  );
};

export default LogoutButton;
