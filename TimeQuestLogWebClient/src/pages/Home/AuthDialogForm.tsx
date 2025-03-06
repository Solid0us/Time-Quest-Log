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
  openModal?: boolean;
}

const AuthDialogForm = ({
  children,
  description,
  triggerText,
  title,
  openModal,
}: AuthDialogFormProps) => {
  const returnToHomePage = (e: boolean) => {
    if (e === false) {
      window.location.href = "/";
    }
  };
  return (
    <Dialog
      onOpenChange={(e) => returnToHomePage(e.valueOf())}
      open={openModal}
    >
      {openModal === undefined && (
        <DialogTrigger asChild>
          <Button variant={"outline"}>{triggerText}</Button>
        </DialogTrigger>
      )}
      <DialogContent onInteractOutside={(e) => e.preventDefault()}>
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
