package com.solid0us.time_quest_log.repositories;

import com.solid0us.time_quest_log.model.Users;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

import java.util.UUID;

@Repository
public interface UserRepository extends JpaRepository<Users, UUID> {
    @Query("Select u from Users u WHERE LOWER(u.username) = LOWER(:username)")
    Users findByUsername(String username);
}
