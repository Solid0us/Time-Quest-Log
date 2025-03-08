package com.solid0us.time_quest_log.repositories;

import com.solid0us.time_quest_log.model.DTOs.GameHoursDTO;
import com.solid0us.time_quest_log.model.DTOs.GameSessionAggregateStatsDTO;
import com.solid0us.time_quest_log.model.GameSessions;
import com.solid0us.time_quest_log.model.aggregates.HoursPlayedByGenre;
import com.solid0us.time_quest_log.model.aggregates.HoursPlayedPerYear;
import com.solid0us.time_quest_log.model.aggregates.HoursPlayedPerYearPerMonthPerGame;
import com.solid0us.time_quest_log.model.aggregates.HoursPlayedPerYearPerMonthPerGenre;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.UUID;

@Repository
public interface GameSessionRepository extends JpaRepository<GameSessions, UUID> {

    List<GameSessions> findByUser_Id(UUID userId);

    @Query(value = """
            SELECT COALESCE(SUM(EXTRACT(EPOCH FROM (end_time - start_time)) / 3600)::double precision, 0) AS total_hours_played
           FROM game_sessions
           WHERE user_id = :userId
            AND end_time IS NOT NULL;
    """, nativeQuery = true)
    Double findTotalPlaytimeInHoursByUserId(@Param("userId") UUID userId);

    @Query(value = """
          SELECT gen.id AS id, gen.name AS name, COALESCE(SUM(EXTRACT(EPOCH FROM (gs.end_time - gs.start_time)) / 3600)::double precision, 0) AS hoursPlayed
          FROM game_sessions gs
          JOIN games g ON gs.game_id = g.id
          JOIN game_genre g_gen ON g_gen.game_id = g.id
          JOIN genres gen ON gen.id = g_gen.genre_id
          WHERE gs.user_id = :userId
          AND gs.end_time IS NOT NULL
          GROUP BY gen.id;
    """, nativeQuery = true)
    List<HoursPlayedByGenre> findTotalPlaytimeByGenre(@Param("userId") UUID userId);

    @Query(value = """
          SELECT g.id AS gameId, g.name as gameTitle,
                COALESCE(SUM(EXTRACT(EPOCH FROM (gs.end_time - gs.start_time)) / 3600)::double precision, 0) AS hoursPlayed
          FROM game_sessions gs
          JOIN games g ON g.id = gs.game_id
          WHERE gs.user_id = :userId
          AND gs.end_time IS NOT NULL
          GROUP BY g.id;
    """, nativeQuery = true)
    List<GameHoursDTO> findHoursPlayedPerGame(@Param("userId") UUID userId);

    @Query(value = """
          SELECT EXTRACT(YEAR FROM gs.start_time)::varchar(255) AS year,
                COALESCE(SUM(EXTRACT(EPOCH FROM (gs.end_time - gs.start_time)) / 3600)::double precision, 0) AS hoursPlayed
          FROM game_sessions gs
          WHERE gs.user_id = :userId
          AND gs.end_time IS NOT NULL
          GROUP BY EXTRACT(YEAR FROM gs.start_time);
    """, nativeQuery = true)
    List<HoursPlayedPerYear> findHoursPlayedPerYear(@Param("userId") UUID userId);

    @Query(value = """
          SELECT gen.id AS genreId,
                 gen.name AS genreName,
                 EXTRACT(YEAR FROM gs.start_time)::varchar(255) AS year,
                 EXTRACT(MONTH FROM gs.start_time)::varchar(255) AS month,
                 COALESCE(SUM(EXTRACT(EPOCH FROM (gs.end_time - gs.start_time)) / 3600)::double precision, 0) AS hoursPlayed
          FROM game_sessions gs
          JOIN games g ON gs.game_id = g.id
          JOIN game_genre g_gen ON g.id = g_gen.game_id
          JOIN genres gen ON g_gen.genre_id = gen.id
          WHERE gs.user_id = :userId
          AND gs.end_time IS NOT NULL
          GROUP BY gen.id, gen.name, EXTRACT(YEAR FROM gs.start_time), EXTRACT(MONTH FROM gs.start_time);
    """, nativeQuery = true)
    List<HoursPlayedPerYearPerMonthPerGenre> findHoursPlayedPerYearPerMonthPerGenre(@Param("userId") UUID userId);

    @Query(value = """
           SELECT g.id as gameId,
                  g.name AS gameTitle,
                  EXTRACT(YEAR FROM gs.start_time)::varchar(255) AS year,
                  EXTRACT(MONTH FROM gs.start_time)::varchar(255) AS month,
                  COALESCE(SUM(EXTRACT(EPOCH FROM (gs.end_time - gs.start_time)) / 3600)::double precision, 0) AS hoursPlayed
           FROM game_sessions gs
           JOIN games g ON gs.game_id = g.id
           WHERE gs.user_id = :userId
           AND gs.end_time IS NOT NULL
           GROUP BY EXTRACT(YEAR FROM gs.start_time), EXTRACT(MONTH FROM gs.start_time)::varchar(255), g.name, g.id
    """, nativeQuery = true)
    List<HoursPlayedPerYearPerMonthPerGame> findHoursPlayedPerYearPerMonthPerGame(@Param("userId") UUID userId);

    @Query(value = """
          SELECT
            SUM(EXTRACT(EPOCH FROM (gs.end_time - gs.start_time)) / 3600)::double precision AS hoursPlayed,
            AVG(EXTRACT(EPOCH FROM (gs.end_time - gs.start_time)) / 3600)::double precision AS avgSessionTime,
            MAX(EXTRACT(EPOCH FROM (gs.end_time - gs.start_time)) / 3600)::double precision AS longestSessionTime,
            (SELECT to_char(gs2.start_time, 'YYYY-MM-DD"T"HH24:MI:SS.MS+00:00')
               FROM game_sessions gs2
               WHERE gs2.user_id = :userId
                 AND gs2.game_id = :gameId
               ORDER BY (gs2.end_time - gs2.start_time) DESC
               LIMIT 1) AS longestSessionDate,
            to_char(MAX(gs.start_time), 'YYYY-MM-DD"T"HH24:MI:SS.MS+00:00') AS lastPlayed,
            to_char(MIN(gs.start_time), 'YYYY-MM-DD"T"HH24:MI:SS.MS+00:00') AS firstTimePlayed
        FROM game_sessions gs
        WHERE gs.user_id = :userId
        AND gs.game_id = :gameId;
    """, nativeQuery = true)
    List<GameSessionAggregateStatsDTO> findUserGameAggregateSessionStats(@Param("userId") UUID userId, @Param("gameId") int gameId);

}