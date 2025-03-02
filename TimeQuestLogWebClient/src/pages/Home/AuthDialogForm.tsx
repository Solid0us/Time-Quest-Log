import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import React from "react";

interface AuthDialogFormProps {
  title: string;
  description: string;
  triggerText: string;
  children: React.ReactNode;
}

const AuthDialogForm = ({
  children,
  description,
  triggerText,
  title,
}: AuthDialogFormProps) => {
  return (
    <Dialog>
      <DialogTrigger asChild>
        <Button variant={"outline"}>{triggerText}</Button>
      </DialogTrigger>
      <DialogContent
        onOpenAutoFocus={(e) => e.preventDefault()}
        onInteractOutside={(e) => e.preventDefault()}
      >
        <DialogTitle className="font-bold text-xl text-center">
          {title}
        </DialogTitle>
        <DialogDescription className="text-foreground text-base text-center">
          {description}
        </DialogDescription>
        {children}
      </DialogContent>
    </Dialog>
  );
};

export default AuthDialogForm;
