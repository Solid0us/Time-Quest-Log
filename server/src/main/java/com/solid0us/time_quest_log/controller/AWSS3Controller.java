package com.solid0us.time_quest_log.controller;


import com.solid0us.time_quest_log.model.ApiResponse;
import com.solid0us.time_quest_log.service.S3Service;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/api/v1/aws")
public class AWSS3Controller {

    @Autowired
    private S3Service s3Service;

    @GetMapping({"/s3/installers", "s3/installers"})
    public ResponseEntity<?> getGames(
            @RequestParam(required = false) String name
    ) {
        String presignedUrl = s3Service.generatePresignedUrl("TimeQuestLogSetup.exe");
        return ResponseEntity.ok(new ApiResponse<String>("success", "", presignedUrl));
    }
}
