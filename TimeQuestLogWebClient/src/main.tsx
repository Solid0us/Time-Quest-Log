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
import MainDashboard from "./pages/Dashboard/Main/MainDashboard.tsx";
import StatisticsDashboard from "./pages/Dashboard/Statistics/StatisticsDashboard.tsx";
import SettingsDashboard from "./pages/Dashboard/SettingsDashboard.tsx";
import GameLibraryDashboard from "./pages/Dashboard/GameLibrary/GameLibraryDashboard.tsx";
import { AuthProvider } from "./context/auth-provider.tsx";

const queryClient = new QueryClient();

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <QueryClientProvider client={queryClient}>
      <ThemeProvider defaultTheme="dark" storageKey="vite-ui-theme">
        <AuthProvider>
          <BrowserRouter>
            <Routes>
              <Route element={<HomeLayout />}>
                <Route path="/" element={<Home />} />
              </Route>

              <Route path="/dashboard" element={<DashboardLayout />}>
                <Route path="home" element={<MainDashboard />} />
                <Route path="statistics" element={<StatisticsDashboard />} />
                <Route path="settings" element={<SettingsDashboard />} />
                <Route path="library" element={<GameLibraryDashboard />} />
              </Route>
            </Routes>
            <ReactQueryDevtools initialIsOpen={false} />
          </BrowserRouter>
        </AuthProvider>
      </ThemeProvider>
    </QueryClientProvider>
  </StrictMode>
);
