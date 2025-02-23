package com.solid0us.time_quest_log.controller;
import com.solid0us.time_quest_log.model.*;
import com.solid0us.time_quest_log.model.DTOs.UsersDTO;
import com.solid0us.time_quest_log.service.JWTService;
import com.solid0us.time_quest_log.service.UserService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.UUID;

@RestController
@RequestMapping("/api/v1/users")
public class UserController {

    @Autowired
    private final UserService userService;

    @Autowired
    private JWTService jwtService;

    @Autowired
    public UserController(UserService userService) {
        this.userService = userService;
    }

    @GetMapping({"", "/"})
    public ResponseEntity<ServiceResult<List<UsersDTO>>> getAllUsers() {
        return ResponseEntity.ok(userService.getAllUsers());
    }

    @GetMapping("/{id}")
    public ResponseEntity<ApiResponse<?>> getUserById(@PathVariable String id) {
        try {
            ServiceResult<UsersDTO> user = userService.getUserById(UUID.fromString(id));
            if (user.isSuccess()) {
                return ResponseEntity.ok().body(ApiResponse.success("", user.getData()));
            }
            return ResponseEntity.status(404).body(ApiResponse.failure("Could not find user.", user.getErrors()));
        } catch (IllegalArgumentException e){
            return ResponseEntity.status(400).body(ApiResponse.failure("Invalid UUID format.", null));
        }
    }

    @PutMapping("/{id}")
    public ResponseEntity<UsersDTO> updateUser(@PathVariable String id, @RequestBody Users user) {
        UsersDTO updatedUser = userService.updateUser(id, user);
        if (updatedUser != null) {
            return ResponseEntity.ok(updatedUser);
        }
        return ResponseEntity.notFound().build();
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<Void> deleteUser(@PathVariable String id) {
        boolean deleted = userService.deleteUser(id);
        if (deleted) {
            return ResponseEntity.status(HttpStatus.NO_CONTENT).body(null);
        }
        return ResponseEntity.notFound().build();
    }

    @PostMapping("/register")
    public ResponseEntity<AuthResponse> register(@RequestBody Users user) {
        AuthResponse authResponse = userService.createUser(user);
        if (authResponse.getError() != null) {
            return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(authResponse);
        }
        return ResponseEntity.status(HttpStatus.CREATED).body(authResponse);
    }

    @PostMapping("/login")
    public ResponseEntity<AuthResponse> login(@RequestBody Users requestedUser) {
        AuthResponse tokenResponse = userService.verify(requestedUser);
        if (tokenResponse.getError() != null) {
            return ResponseEntity
                    .status(HttpStatus.UNAUTHORIZED)
                    .body(tokenResponse);
        }
        return ResponseEntity.ok().body(tokenResponse);
    }

    @PostMapping("/refresh")
    public ResponseEntity<RefreshJwtResponse> refresh(@RequestBody RefreshTokenBody refreshTokenBody) {
        RefreshJwtResponse refreshJwtResponse = userService.refreshToken(refreshTokenBody.getRefreshToken());
        if (refreshJwtResponse.getToken() != null) {
            return ResponseEntity
                    .status(HttpStatus.CREATED)
                    .body(refreshJwtResponse);
        }
        return ResponseEntity
                .status(HttpStatus.UNAUTHORIZED)
                .body(new RefreshJwtResponse(null));
    }
}