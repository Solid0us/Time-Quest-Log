package com.solid0us.time_quest_log.controller;
import com.solid0us.time_quest_log.model.ApiResponse;
import com.solid0us.time_quest_log.model.AuthResponse;
import com.solid0us.time_quest_log.model.ServiceResult;
import com.solid0us.time_quest_log.model.Users;
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
    
    private final UserService userService;

    @Autowired
    public UserController(UserService userService) {
        this.userService = userService;
    }

    @GetMapping("/")
    public ResponseEntity<List<Users>> getAllUsers() {
        return ResponseEntity.ok(userService.getAllUsers());
    }

    @GetMapping("/{id}")
    public ResponseEntity<ApiResponse<?>> getUserById(@PathVariable String id) {
        ServiceResult<Users> user = userService.getUserById(UUID.fromString(id));
        if (user.getData() != null) {
            return ResponseEntity.ok().body(ApiResponse.success("", user.getData()));
        }
        return ResponseEntity.status(404).body(ApiResponse.failure("Could not find user.", user.getErrors()));
    }

    @PutMapping("/{id}")
    public ResponseEntity<Users> updateUser(@PathVariable String id, @RequestBody Users user) {
        Users updatedUser = userService.updateUser(id, user);
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
    public ResponseEntity<AuthResponse> login(@RequestBody Users user) {
        String token = userService.verify(user);
        if (token == null) {
            return ResponseEntity
                    .status(HttpStatus.UNAUTHORIZED)
                    .body(new AuthResponse(token, user.getUsername(), "Invalid username or password."));
        }
        return ResponseEntity.ok().body(new AuthResponse(token, user.getUsername()));
    }
}