package com.solid0us.time_quest_log.controller;

import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/")
public class HomeController {
    @GetMapping({"", "/"})
    public String getAllUsers() {
        return "<h1>Hello and welcome to the TimeQuest App!</h1>";
    }
}
