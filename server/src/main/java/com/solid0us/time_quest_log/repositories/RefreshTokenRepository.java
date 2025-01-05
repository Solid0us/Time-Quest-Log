package com.solid0us.time_quest_log.repositories;

import com.solid0us.time_quest_log.model.RefreshTokens;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.UUID;

@Repository
public interface RefreshTokenRepository extends JpaRepository<RefreshTokens, UUID>{
    RefreshTokens findByRefreshToken(String refreshToken);
}
