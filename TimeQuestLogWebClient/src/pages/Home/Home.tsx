import HorizontalNavbar from "@/pages/Home/HorizontalNavbar";
import HeroSection from "./HeroSection";

const Home = () => {
  return (
    <div className="flex flex-col items-center justify-center">
      <HorizontalNavbar />
      <HeroSection />
    </div>
  );
};

export default Home;
