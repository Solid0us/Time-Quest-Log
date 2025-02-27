import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Label } from "@/components/ui/label";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import React from "react";

interface AreaChartContainerProps {
  year: string;
  setYear: (year: string) => void;
  yearSelectList: string[];
  chartTitle: string;
  chartDescription: string;
  children: React.ReactNode;
}

const AreaChartContainer = ({
  chartDescription,
  chartTitle,
  children,
  setYear,
  year,
  yearSelectList,
}: AreaChartContainerProps) => {
  return (
    <Card className="w-full">
      <CardHeader className="flex items-center gap-5 space-y-0 border-b py-5 sm:flex-row">
        <div className="grid flex-1 gap-1 text-center sm:text-left">
          <CardTitle>{chartTitle}</CardTitle>
          <CardDescription>{chartDescription}</CardDescription>
        </div>
        <div className="flex flex-col gap-y-2">
          <Label className="text-center">Year</Label>
          <Select value={year} onValueChange={setYear}>
            <SelectTrigger
              className="w-[160px] rounded-lg sm:ml-auto"
              aria-label="Select time range"
            >
              <SelectValue placeholder="Year" />
            </SelectTrigger>
            <SelectContent className="rounded-xl">
              {yearSelectList.map((elem) => (
                <SelectItem key={elem} value={elem} className="rounded-lg">
                  {elem}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
        </div>
      </CardHeader>
      <CardContent className="px-2 pt-4 sm:px-6 sm:pt-6">
        {children}
      </CardContent>
    </Card>
  );
};

export default AreaChartContainer;
