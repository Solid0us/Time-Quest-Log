package com.solid0us.time_quest_log.repositories;

import com.solid0us.time_quest_log.model.UserGames;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;
import java.util.UUID;

public interface UserGameRepository extends JpaRepository<UserGames, UUID> {
    List<UserGames> findByUser_Id(UUID userId);
}
