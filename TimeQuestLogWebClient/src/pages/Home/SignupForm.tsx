import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { useMutation } from "@tanstack/react-query";
import { registerUser } from "@/services/authServices";
import { useNavigate } from "react-router-dom";
import { useAuth } from "@/hooks/useAuth";

const formSchema = z
  .object({
    username: z
      .string()
      .min(8, {
        message: "Username must be at least 8 characters long!",
      })
      .superRefine((val, ctx) => {
        if (!val.match(/^[a-zA-Z0-9]+$/)) {
          ctx.addIssue({
            message:
              "Username cannot contain whitespace or non-alphanumeric characters.",
            code: z.ZodIssueCode.invalid_string,
            validation: "regex",
          });
        }
      }),
    email: z.string().email("Must be a valid email"),
    firstName: z
      .string()
      .min(1, {
        message: "First name cannot be empty!",
      })
      .superRefine((val, ctx) => {
        if (!val.match(/^[a-zA-Z]+([ '-][a-zA-Z]+)*$/)) {
          ctx.addIssue({
            message: "Enter a valid first name.",
            code: z.ZodIssueCode.invalid_string,
            validation: "regex",
          });
        }
      }),
    lastName: z
      .string()
      .min(1, {
        message: "Last name cannot be empty!",
      })
      .superRefine((val, ctx) => {
        if (!val.match(/^[a-zA-Z]+([ '-][a-zA-Z]+)*$/)) {
          ctx.addIssue({
            message: "Enter a valid last name.",
            code: z.ZodIssueCode.invalid_string,
            validation: "regex",
          });
        }
      }),
    password: z.string().min(8, {
      message: "Password must be at least 8 characters long!",
    }),
    confirmPassword: z.string().min(8, {
      message: "Password must be at least 8 characters long!",
    }),
  })
  .superRefine((val, ctx) => {
    if (val.password !== val.confirmPassword) {
      ctx.addIssue({
        code: z.ZodIssueCode.custom,
        message: "Passwords must match",
        path: ["confirmPassword"],
      });
    }
  });

const SignupForm = () => {
  const { login } = useAuth();
  const navigate = useNavigate();
  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      username: "",
      email: "",
      firstName: "",
      lastName: "",
      password: "",
      confirmPassword: "",
    },
  });
  const onSubmit = (values: z.infer<typeof formSchema>) => {
    mutation.mutate(values);
  };
  const mutation = useMutation({
    mutationFn: registerUser,
    onSuccess: (data) => {
      const { token, refreshToken } = data;
      login(token ?? "", refreshToken ?? "");
      navigate("/dashboard", { replace: true });
    },
    onError: (e) => {
      alert(e);
    },
  });
  return (
    <Form {...form}>
      <form
        onSubmit={form.handleSubmit(onSubmit)}
        className="flex flex-col items-center gap-3"
      >
        <FormField
          control={form.control}
          name="username"
          render={({ field }) => (
            <FormItem className="w-3/4">
              <FormLabel>Username</FormLabel>
              <FormControl>
                <Input className="w-full" placeholder="Username" {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name="email"
          render={({ field }) => (
            <FormItem className="w-3/4">
              <FormLabel>Email</FormLabel>
              <FormControl>
                <Input className="w-full" placeholder="Email" {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name="firstName"
          render={({ field }) => (
            <FormItem className="w-3/4">
              <FormLabel>First Name</FormLabel>
              <FormControl>
                <Input placeholder="First Name" {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name="lastName"
          render={({ field }) => (
            <FormItem className="w-3/4">
              <FormLabel>Last Name</FormLabel>
              <FormControl>
                <Input placeholder="Last Name" {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name="password"
          render={({ field }) => (
            <FormItem className="w-3/4">
              <FormLabel>Password</FormLabel>
              <FormControl>
                <Input placeholder="Password" type="password" {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name="confirmPassword"
          render={({ field }) => (
            <FormItem className="w-3/4">
              <FormLabel>Confirm Password</FormLabel>
              <FormControl>
                <Input
                  placeholder="Confirm Password"
                  type="password"
                  {...field}
                />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <Button type="submit" className="w-1/2 mt-5">
          Sign up
        </Button>
      </form>
    </Form>
  );
};

export default SignupForm;
