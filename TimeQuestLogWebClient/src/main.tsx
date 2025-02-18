import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./index.css";
import { BrowserRouter, Routes, Route } from "react-router";
import HomeLayout from "./components/layouts/HomeLayout.tsx";
import Home from "./pages/Home/Home.tsx";
import { ThemeProvider } from "./context/theme-provider.tsx";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";
import DashboardLayout from "./components/layouts/DashboardLayout.tsx";
import MainDashboard from "./pages/Dashboard/MainDashboard.tsx";

const queryClient = new QueryClient();

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <QueryClientProvider client={queryClient}>
      <ThemeProvider defaultTheme="dark" storageKey="vite-ui-theme">
        <BrowserRouter>
          <Routes>
            <Route element={<HomeLayout />}>
              <Route index element={<Home />}></Route>
            </Route>
          </Routes>
          <Routes>
            <Route element={<DashboardLayout />}>
              <Route path="/dashboard" element={<MainDashboard />}></Route>
            </Route>
          </Routes>
        </BrowserRouter>
      </ThemeProvider>
      <ReactQueryDevtools initialIsOpen={false} />
    </QueryClientProvider>
  </StrictMode>
);
