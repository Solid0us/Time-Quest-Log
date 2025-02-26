package com.solid0us.time_quest_log.model.DTOs;

import com.solid0us.time_quest_log.model.aggregates.HoursPlayedPerYearPerMonthPerGame;
import com.solid0us.time_quest_log.model.aggregates.HoursPlayedPerYearPerMonthPerGenre;

import java.util.List;
import java.util.Map;

public class GameStatsDTO {
    private String userId;
    private double totalHoursPlayed;
    private int totalGames;
    private Map<String, Double> hoursPlayedPerGenre;
    private List<GameHoursDTO> hoursPlayedPerGame;
    private Map<String, Double> hoursPlayedDistributionPerYear;
    private List<HoursPlayedPerYearPerMonthPerGenre> hoursPlayedDistributionPerYearPerGenre;
    private List<HoursPlayedPerYearPerMonthPerGame> hoursPlayedDistributionPerYearPerGame;

    public GameStatsDTO(String userId, double totalHoursPlayed, int totalGames,
                        Map<String, Double> hoursPlayedPerGenre, List<GameHoursDTO> hoursPlayedPerGame,
                        Map<String, Double> hoursPlayedDistributionPerYear,
                        List<HoursPlayedPerYearPerMonthPerGenre> hoursPlayedDistributionPerYearPerGenre,
                        List<HoursPlayedPerYearPerMonthPerGame> hoursPlayedDistributionPerYearPerGame) {
        this.userId = userId;
        this.totalHoursPlayed = totalHoursPlayed;
        this.totalGames = totalGames;
        this.hoursPlayedPerGenre = hoursPlayedPerGenre;
        this.hoursPlayedPerGame = hoursPlayedPerGame;
        this.hoursPlayedDistributionPerYear = hoursPlayedDistributionPerYear;
        this.hoursPlayedDistributionPerYearPerGenre = hoursPlayedDistributionPerYearPerGenre;
        this.hoursPlayedDistributionPerYearPerGame = hoursPlayedDistributionPerYearPerGame;
    }

    public String getUserId() {
        return userId;
    }

    public void setUserId(String userId) {
        this.userId = userId;
    }

    public double getTotalHoursPlayed() {
        return totalHoursPlayed;
    }

    public void setTotalHoursPlayed(int totalHoursPlayed) {
        this.totalHoursPlayed = totalHoursPlayed;
    }

    public int getTotalGames() {
        return totalGames;
    }

    public void setTotalGames(int totalGames) {
        this.totalGames = totalGames;
    }

    public Map<String, Double> getHoursPlayedPerGenre() {
        return hoursPlayedPerGenre;
    }

    public void setHoursPlayedPerGenre(Map<String, Double> hoursPlayedPerGenre) {
        this.hoursPlayedPerGenre = hoursPlayedPerGenre;
    }

    public List<GameHoursDTO> getHoursPlayedPerGame() {
        return hoursPlayedPerGame;
    }

    public void setHoursPlayedPerGame(List<GameHoursDTO> hoursPlayedPerGame) {
        this.hoursPlayedPerGame = hoursPlayedPerGame;
    }

    public Map<String, Double> getHoursPlayedDistributionPerYear() {
        return hoursPlayedDistributionPerYear;
    }

    public void setHoursPlayedDistributionPerYear(Map<String, Double> hoursPlayedDistributionPerYear) {
        this.hoursPlayedDistributionPerYear = hoursPlayedDistributionPerYear;
    }

    public List<HoursPlayedPerYearPerMonthPerGenre> getHoursPlayedDistributionPerYearPerGenre() {
        return hoursPlayedDistributionPerYearPerGenre;
    }

    public void setHoursPlayedDistributionPerYearPerGenre(List<HoursPlayedPerYearPerMonthPerGenre> hoursPlayedDistributionPerYearPerGenre) {
        this.hoursPlayedDistributionPerYearPerGenre = hoursPlayedDistributionPerYearPerGenre;
    }

    public List<HoursPlayedPerYearPerMonthPerGame> getHoursPlayedDistributionPerYearPerGame() {
        return hoursPlayedDistributionPerYearPerGame;
    }

    public void setHoursPlayedDistributionPerYearPerGame(List<HoursPlayedPerYearPerMonthPerGame> hoursPlayedDistributionPerYearPerGame) {
        this.hoursPlayedDistributionPerYearPerGame = hoursPlayedDistributionPerYearPerGame;
    }
}
