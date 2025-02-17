const HeroSection = () => {
  return (
    <div className="w-full h-screen flex flex-col items-center justify-center gap-y-10">
      <div className="flex items-center gap-5">
        <img
          draggable={false}
          src="/gamepad-logo.png"
          className="size-14 sm:size-16 md:size-auto"
        />
        <h1 className="text-4xl sm:text-6xl md:text-8xl">TimeQuest Log</h1>
      </div>
      <p className="text-xl md:text-2xl max-w-lg md:max-w-xl text-center">
        Track your <span className="font-bold text-primary">playtime</span>,
        analyze your <span className="font-bold text-primary">habits</span>, and{" "}
        <span className="font-bold text-primary">level up</span> your gaming
        experience.
      </p>
    </div>
  );
};

export default HeroSection;
