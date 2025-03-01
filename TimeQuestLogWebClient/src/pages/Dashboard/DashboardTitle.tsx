interface DashboardTitleProps {
  text: string;
}

const DashboardTitle = ({ text }: DashboardTitleProps) => {
  return (
    <h1 className="text-center font-bold text-2xl md:text-4xl p-2">{text}</h1>
  );
};

export default DashboardTitle;
