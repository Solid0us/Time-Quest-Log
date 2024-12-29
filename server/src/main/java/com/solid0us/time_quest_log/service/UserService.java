package com.solid0us.time_quest_log.service;

import com.solid0us.time_quest_log.model.AuthResponse;
import com.solid0us.time_quest_log.model.Users;
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

    public UserService() {

    }

    public List<Users> getAllUsers() {
        ArrayList<Users> newUsers = new ArrayList<Users>(users.values());
        return newUsers;
    }

    public Optional<Users> getUserById(UUID id) {
        return userRepository.findById(id);
    }

    public AuthResponse createUser(Users user) {
        if (userRepository.findByUsername(user.getUsername()) != null) {
            return new AuthResponse(null, user.getUsername(), "Username already exists. Please select a different name.");
        }
        user.setPassword(bCryptPasswordEncoder.encode(user.getPassword()));
        Users createdUser = userRepository.save(user);
        String userName = createdUser.getUsername();
        return new AuthResponse(jwtService.generateToken(userName), userName);
    }

    public Users updateUser(String id, Users user) {
        if (users.containsKey(id)) {
            users.put(id, user);
            return user;
        }
        return null;
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
}