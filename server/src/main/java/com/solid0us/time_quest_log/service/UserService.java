package com.solid0us.time_quest_log.service;

import com.solid0us.time_quest_log.model.*;
import com.solid0us.time_quest_log.repositories.UserRepository;
import org.springframework.beans.factory.annotation.Autowired;
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

    public ServiceResult<UsersDTO> getUserById(UUID id) {
        Users user = userRepository.findById(id).orElse(null);
        List<ErrorDetail> errors = new ArrayList<>();
        if (user == null) {
            errors.add(new ErrorDetail("id", "Could not find user with the given ID."));
            return ServiceResult.failure(errors);
        }
        return ServiceResult.success(new UsersDTO(user));
    }

    public AuthResponse createUser(Users user) {
        if (userRepository.findByUsername(user.getUsername()) != null) {
            return new AuthResponse(user.getUsername(), "Username already exists. Please select a different name.");
        }
        user.setPassword(bCryptPasswordEncoder.encode(user.getPassword()));
        Users createdUser = userRepository.save(user);
        String userName = createdUser.getUsername();
        String refreshToken = generateRefreshToken(createdUser);
        return new AuthResponse(jwtService.generateToken(userName), refreshToken, user);
    }

    public UsersDTO updateUser(String id, Users user) {
        if (users.containsKey(id)) {
            users.put(id, user);
            return new UsersDTO(user);
        }
        return null;
    }

    public Users getUserByUsername(String username) {
        return userRepository.findByUsername(username);
    }

    public boolean deleteUser(String id) {
        return users.remove(id) != null;
    }

    public String verify(Users user) {
        try {
        Authentication authentication =
                authenticationManager.authenticate(new UsernamePasswordAuthenticationToken(user.getUsername(), user.getPassword()));
            if (authentication.isAuthenticated()){
                return jwtService.generateToken(user.getUsername());
            }
            return null;

        } catch (AuthenticationException e){
            System.out.println(e.getStackTrace().toString());
            return null;
        }
    }

    public boolean verifyRefreshToken(String refreshToken) {
        return jwtService.validateRefreshToken(refreshToken);
    }

    public String generateRefreshToken(Users user) {
        return jwtService.generateRefreshToken(user);
    }
}