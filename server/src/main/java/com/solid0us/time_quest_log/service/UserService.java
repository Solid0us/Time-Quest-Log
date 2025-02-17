package com.solid0us.time_quest_log.service;

import com.solid0us.time_quest_log.model.*;
import com.solid0us.time_quest_log.repositories.UserRepository;
import jakarta.transaction.Transactional;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.AuthenticationException;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.stereotype.Service;


import java.util.*;

@Service
public class UserService  {

    @Autowired
    private UserRepository userRepository;

    @Autowired
    AuthenticationManager authenticationManager;

    @Autowired
    private JWTService jwtService;

    private BCryptPasswordEncoder bCryptPasswordEncoder = new BCryptPasswordEncoder(12);

    private final Map<String, Users> users = new HashMap<>();

    public ServiceResult<List<UsersDTO>> getAllUsers() {
        ArrayList<Users> newUsers = new ArrayList<Users>(users.values());
        ArrayList<UsersDTO> usersDTO = new ArrayList<>();
        for (Users user : newUsers) {
            usersDTO.add(new UsersDTO(user));
        }
        return ServiceResult.success(usersDTO);
    }

    @Transactional(rollbackOn = Exception.class)
    public ServiceResult<UsersDTO> getUserById(UUID id) {
        Users user = userRepository.findById(id).orElse(null);
        List<ErrorDetail> errors = new ArrayList<>();
        if (user == null) {
            errors.add(new ErrorDetail("id", "Could not find user with the given ID."));
            return ServiceResult.failure(errors);
        }
        return ServiceResult.success(new UsersDTO(user));
    }

    @Transactional(rollbackOn = Exception.class)
    public AuthResponse createUser(Users user) {
        if (userRepository.findByUsername(user.getUsername()) != null) {
            return new AuthResponse(user.getUsername(), "Username already exists. Please select a different name.");
        }
        user.setPassword(bCryptPasswordEncoder.encode(user.getPassword()));
        Users createdUser = userRepository.save(user);
        String refreshToken = generateRefreshToken(createdUser);
        return new AuthResponse(jwtService.generateToken(user), refreshToken, user);
    }

    public UsersDTO updateUser(String id, Users user) {
        if (users.containsKey(id)) {
            users.put(id, user);
            return new UsersDTO(user);
        }
        return null;
    }

    @Transactional(rollbackOn = Exception.class)
    public Users getUserByUsername(String username) {
        return userRepository.findByUsername(username);
    }

    public boolean deleteUser(String id) {
        return users.remove(id) != null;
    }

    public AuthResponse verify(Users requestedUser) {
        Users existingUser = getUserByUsername(requestedUser.getUsername());
        String jwtToken = null;
        String refreshToken = null;
        if (existingUser == null) {
            return new AuthResponse(requestedUser.getUsername(), "Could not find username in the database");
        }
        try {
        Authentication authentication =
                authenticationManager.authenticate(new UsernamePasswordAuthenticationToken(requestedUser.getUsername(), requestedUser.getPassword()));
            if (authentication.isAuthenticated()){
                jwtToken = jwtService.generateToken(existingUser);
                refreshToken = generateRefreshToken(existingUser);
            }
        } catch (AuthenticationException e){
            System.out.println(e.getStackTrace().toString());
        }
        if (jwtToken == null || refreshToken == null){
            return new AuthResponse(requestedUser.getUsername(), "Invalid username or password");
        }
        return new AuthResponse(jwtToken, refreshToken, existingUser);
    }

    public RefreshJwtResponse refreshToken(String refreshToken) {
        try {
            String username = jwtService.extractUsername(refreshToken, true);
            Users user = getUserByUsername(username);
            if (user == null || !verifyRefreshToken(refreshToken)) {
                return new RefreshJwtResponse(null);
            }
            String newAccessToken = jwtService.generateToken(user);
            return new RefreshJwtResponse(newAccessToken);
        } catch (Exception e) {
            System.out.println("Could not parse refresh token");
            return new RefreshJwtResponse(null);
        }
    }

    public boolean verifyRefreshToken(String refreshToken) {
        return jwtService.validateRefreshToken(refreshToken);
    }

    public String generateRefreshToken(Users user) {
        return jwtService.generateRefreshToken(user);
    }
}