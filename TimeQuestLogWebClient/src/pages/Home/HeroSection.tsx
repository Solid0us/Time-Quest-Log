import { Button } from "@/components/ui/button";
import { useGetInstaller } from "@/services/installerServices";

const HeroSection = () => {
  const { refetch } = useGetInstaller();

  const downloadInstaller = async () => {
    const response = await refetch();
    const downloadUrl = response.data?.data;
    if (downloadUrl) {
      window.open(downloadUrl, "_blank")?.focus();
    }
  };

  return (
    <div className="w-full h-screen flex flex-col items-center justify-center">
      <div className="flex flex-col items-center gap-y-10">
        <div className="flex items-center gap-5">
          <img
            draggable={false}
            src="/gamepad-logo.png"
            className="size-14 sm:size-16 md:size-auto"
          />
          <h1 className="text-4xl sm:text-6xl md:text-8xl">TimeQuest Log</h1>
        </div>
        <p className="text-xl md:text-2xl max-w-md md:max-w-lg text-center">
          Track your{" "}
          <span className="font-bold text-primary hover:brightness-125 duration-200">
            playtime
          </span>
          , analyze your{" "}
          <span className="font-bold text-primary hover:brightness-125 duration-200">
            habits
          </span>
          , and{" "}
          <span className="font-bold text-primary hover:brightness-125 duration-200">
            level up
          </span>{" "}
          your gaming experience.
        </p>
      </div>
      <div className="absolute flex flex-col items-center gap-y-3 translate-y-44 md:translate-y-56">
        <p className="text-xl md:text-2xl max-w-md md:max-w-lg text-center">
          Install the Windows App Now!
        </p>
        <Button className="h-fit" onClick={downloadInstaller}>
          <img
            src="/iconmonstr-download-19-96.png"
            className="size-6 md:size-8"
          />
        </Button>
      </div>
    </div>
  );
};

export default HeroSection;
