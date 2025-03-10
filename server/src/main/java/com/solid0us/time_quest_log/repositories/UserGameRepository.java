package com.solid0us.time_quest_log.repositories;

import com.solid0us.time_quest_log.model.DTOs.UserGameWithHoursDTO;
import com.solid0us.time_quest_log.model.Games;
import com.solid0us.time_quest_log.model.UserGames;
import com.solid0us.time_quest_log.model.Users;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

import java.util.List;
import java.util.Optional;
import java.util.UUID;

public interface UserGameRepository extends JpaRepository<UserGames, UUID> {
    List<UserGames> findByUser_Id(UUID userId);

    Optional<UserGames> findByUserAndGame(Users user, Games game);


    @Query(value = """
           SELECT COUNT(DISTINCT game_id)
           FROM user_games
           WHERE user_id = :userId;
    """, nativeQuery = true)
    int findDistinctGamesByUserId(@Param("userId") UUID userId);

    @Query(value = """
       SELECT
           g.id AS gameId,
           g.name AS gameName,
           g.cover_url AS coverUrl,
           ug.exe_name AS exeName,
           genres.genre_list AS genres,
           COALESCE(hours.total_hours, 0) AS hoursPlayed
       FROM
           user_games ug
       JOIN
           games g ON g.id = ug.game_id
       JOIN
           users u ON u.id = ug.user_id
       LEFT JOIN (
           SELECT
               game_id,
               SUM(EXTRACT(EPOCH FROM (end_time - start_time)) / 3600)::double precision AS total_hours
           FROM
               game_sessions
           WHERE
              user_id = :userId
           GROUP BY
               game_id
       ) hours ON hours.game_id = g.id
       LEFT JOIN (
           SELECT
               g_gen.game_id,
               STRING_AGG(DISTINCT gen.name, ', ') AS genre_list
           FROM
               game_genre g_gen
           JOIN
               genres gen ON gen.id = g_gen.genre_id
           GROUP BY
               g_gen.game_id
       ) genres ON genres.game_id = g.id
       WHERE
           u.id = :userId
       ORDER BY g.name
""", nativeQuery = true)
    List<UserGameWithHoursDTO> getUserGameWithHoursPlayed(@Param("userId") UUID userId);
}
