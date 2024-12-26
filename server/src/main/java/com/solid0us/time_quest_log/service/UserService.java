package com.solid0us.time_quest_log.service;

import com.solid0us.time_quest_log.model.User;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

@Service
public class UserService {
    private final Map<String, User> users = new HashMap<>();

    public UserService() {
        users.put("123", new User("123", "solid0us"));
    }

    public List<User> getAllUsers() {
        ArrayList<User> newUsers = new ArrayList<User>(users.values());
        return newUsers;
    }

    public User getUserById(String id) {
        return users.get(id);
    }

    public User createUser(User user) {
        users.put(user.getId(), user);
        return user;
    }

    public User updateUser(String id, User user) {
        if (users.containsKey(id)) {
            users.put(id, user);
            return user;
        }
        return null;
    }

    public boolean deleteUser(String id) {
        return users.remove(id) != null;
    }
}