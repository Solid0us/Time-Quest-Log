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
import useLocalStorage from "@/hooks/useLocalStorage";
import { useNavigate } from "react-router-dom";

const formSchema = z.object({
  username: z.string().min(0, {
    message: "Username cannot be empty!",
  }),
  password: z.string().min(0, {
    message: "Password cannot be empty!",
  }),
});

const LoginForm = () => {
  const [jwt, setJwt] = useLocalStorage<string | null>("jwt", null);
  const [refreshToken, setRefreshToken] = useLocalStorage<string | null>(
    "refreshToken",
    null
  );
  const navigate = useNavigate();

  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      username: "",
    },
  });

  const onSubmit = (values: z.infer<typeof formSchema>) => {
    mutation.mutate(values);
  };

  const login = async (values: z.infer<typeof formSchema>) => {
    const data = await loginUser(values);
    return data;
  };

  const mutation = useMutation({
    mutationFn: login,
    onSuccess: (data) => {
      const { token, refreshToken } = data;
      setJwt(token);
      setRefreshToken(refreshToken);
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
            <FormItem>
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
            <FormItem>
              <FormLabel>Password</FormLabel>
              <FormControl>
                <Input placeholder="password" type="password" {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <Button type="submit" className="w-1/2 mt-5">
          Submit
        </Button>
      </form>
    </Form>
  );
};

export default LoginForm;
