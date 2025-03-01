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
import { loginUser } from "@/services/authServices";
import { useNavigate } from "react-router-dom";
import { useAuth } from "@/hooks/useAuth";

const formSchema = z.object({
  username: z.string().min(1, {
    message: "Username cannot be empty!",
  }),
  password: z.string().min(1, {
    message: "Password cannot be empty!",
  }),
});

const LoginForm = () => {
  const { login } = useAuth();
  const navigate = useNavigate();

  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      username: "",
      password: "",
    },
  });

  const onSubmit = (values: z.infer<typeof formSchema>) => {
    mutation.mutate(values);
  };

  const loginFn = async (values: z.infer<typeof formSchema>) => {
    const data = await loginUser(values);
    return data;
  };

  const mutation = useMutation({
    mutationFn: loginFn,
    onSuccess: (data) => {
      const { token, refreshToken } = data;
      login(token ?? "", refreshToken ?? "");
      navigate("/dashboard/home", { replace: true });
    },
    onError: (e) => {
      form.setError("root", {
        message: e.message,
      });
    },
  });

  return (
    <Form {...form}>
      <form
        onSubmit={form.handleSubmit(onSubmit)}
        className="flex flex-col items-center gap-3"
      >
        {form.formState.errors.root?.message && (
          <p className="text-destructive">
            {form.formState.errors.root.message}
          </p>
        )}
        <FormField
          control={form.control}
          name="username"
          render={({ field }) => (
            <FormItem className="w-3/4">
              <FormLabel>Username</FormLabel>
              <FormControl>
                <Input placeholder="username" {...field} />
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
                <Input placeholder="password" type="password" {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <Button type="submit" className="w-1/2 mt-5">
          Log in
        </Button>
      </form>
    </Form>
  );
};

export default LoginForm;
