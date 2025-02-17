import { Outlet } from "react-router";

const HomeLayout = () => {
  return (
    <div className="w-full h-screen">
      <Outlet />
    </div>
  );
};

export default HomeLayout;
