package com.solid0us.time_quest_log.service;

import com.solid0us.time_quest_log.model.ErrorDetail;
import com.solid0us.time_quest_log.model.GameSessions;
import com.solid0us.time_quest_log.model.ServiceResult;
import com.solid0us.time_quest_log.repositories.GameSessionRepository;
import org.hibernate.exception.ConstraintViolationException;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.dao.DataIntegrityViolationException;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

@Service
public class GameSessionService {
    @Autowired
    private GameSessionRepository gameSessionRepository;

    public ServiceResult<GameSessions> createGameSession(GameSessions gameSession) {
        List<ErrorDetail> errorDetails = new ArrayList<>();
        try {
            return ServiceResult.success(gameSessionRepository.save(gameSession));
        }  catch (DataIntegrityViolationException e) {
            errorDetails.add(new ErrorDetail("input", "There was an ID conflict when creating a game session."));
        } catch (ConstraintViolationException e){
            errorDetails.add(new ErrorDetail("input", "One or more entities do not exist."));
        }
        catch (Exception e) {
            errorDetails.add(new ErrorDetail("input", "Unknown error occurred while saving game session."));
        }
        return ServiceResult.failure(errorDetails);
    }

    public ServiceResult<List<GameSessions>> getGameSessions(String userId) {
        List<ErrorDetail> errorDetails = new ArrayList<>();
        List<GameSessions> gameSessions;
        try {
            if (userId == null) {
                gameSessions = gameSessionRepository.findAll();
            } else {
                gameSessions = gameSessionRepository.findByUser_Id(UUID.fromString(userId));
            }
            return ServiceResult.success(gameSessions);
        } catch (Exception e) {
            errorDetails.add(new ErrorDetail("input", "Unknown error occurred while retrieving game sessions."));
        }
        return ServiceResult.failure(errorDetails);
    }

}
