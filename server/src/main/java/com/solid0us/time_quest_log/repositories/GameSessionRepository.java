package com.solid0us.time_quest_log.repositories;

import com.solid0us.time_quest_log.model.GameSessions;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.UUID;

@Repository
public interface GameSessionRepository extends JpaRepository<GameSessions, UUID>{
    List<GameSessions> findByUser_Id(UUID userId);
}
