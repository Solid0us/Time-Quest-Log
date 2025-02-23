package com.solid0us.time_quest_log.model.DTOs;

import java.util.Map;

public class GenreYearlyBreakdownDTO {
    private int genreId;
    private String genreName;
    private Map<String, Double> yearlyBreakdown;
    private Map<String, Map<String, Double>> monthlyBreakdownPerYear;

    public GenreYearlyBreakdownDTO(int genreId, String genreName, Map<String, Double> yearlyBreakdown, Map<String, Map<String, Double>> monthlyBreakdownPerYear) {
        this.genreId = genreId;
        this.genreName = genreName;
        this.yearlyBreakdown = yearlyBreakdown;
        this.monthlyBreakdownPerYear = monthlyBreakdownPerYear;
    }

    public int getGenreId() {
        return genreId;
    }

    public void setGenreId(int genreId) {
        this.genreId = genreId;
    }

    public String getGenreName() {
        return genreName;
    }

    public void setGenreName(String genreName) {
        this.genreName = genreName;
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
