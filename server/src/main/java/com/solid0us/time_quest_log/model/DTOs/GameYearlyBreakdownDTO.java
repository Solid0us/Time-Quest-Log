package com.solid0us.time_quest_log.model.DTOs;

import java.util.Map;

public class GameYearlyBreakdownDTO {
    private int gameId;
    private String gameTitle;
    private Map<String, Double> yearlyBreakdown; // Year -> Hours
    private Map<String, Map<String, Double>> monthlyBreakdownPerYear; // Year -> (Month -> Hours)

    public GameYearlyBreakdownDTO(int gameId, String gameTitle, Map<String, Double> yearlyBreakdown, Map<String, Map<String, Double>> monthlyBreakdownPerYear) {
        this.gameId = gameId;
        this.gameTitle = gameTitle;
        this.yearlyBreakdown = yearlyBreakdown;
        this.monthlyBreakdownPerYear = monthlyBreakdownPerYear;
    }

    public int getGameId() {
        return gameId;
    }

    public void setGameId(int gameId) {
        this.gameId = gameId;
    }

    public String getGameTitle() {
        return gameTitle;
    }

    public void setGameTitle(String gameTitle) {
        this.gameTitle = gameTitle;
    }

    public Map<String, Double> getYearlyBreakdown() {
        return yearlyBreakdown;
    }

    public void setYearlyBreakdown(Map<String, Double> yearlyBreakdown) {
        this.yearlyBreakdown = yearlyBreakdown;
    }

    public Map<String, Map<String, Double>> getMonthlyBreakdownPerYear() {
        return monthlyBreakdownPerYear;
    }

    public void setMonthlyBreakdownPerYear(Map<String, Map<String, Double>> monthlyBreakdownPerYear) {
        this.monthlyBreakdownPerYear = monthlyBreakdownPerYear;
    }
}
