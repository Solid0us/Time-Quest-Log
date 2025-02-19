const HeroSection = () => {
  return (
    <div className="w-full h-screen flex flex-col items-center justify-center gap-y-10 ">
      <div className="flex items-center gap-5">
        <img
          draggable={false}
          src="/gamepad-logo.png"
          className="size-14 sm:size-16 md:size-auto"
        />
        <h1 className="text-4xl sm:text-6xl md:text-8xl">TimeQuest Log</h1>
      </div>
      <p className="text-xl md:text-2xl max-w-md md:max-w-lg text-center hover:scale-105 duration-200">
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
  );
};

export default HeroSection;
