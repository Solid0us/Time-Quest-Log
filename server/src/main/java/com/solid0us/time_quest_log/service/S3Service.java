package com.solid0us.time_quest_log.service;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;
import software.amazon.awssdk.auth.credentials.AwsBasicCredentials;
import software.amazon.awssdk.auth.credentials.ProfileCredentialsProvider;
import software.amazon.awssdk.auth.credentials.StaticCredentialsProvider;
import software.amazon.awssdk.regions.Region;
import software.amazon.awssdk.services.s3.S3Client;
import software.amazon.awssdk.services.s3.model.GetObjectRequest;
import software.amazon.awssdk.services.s3.presigner.S3Presigner;
import software.amazon.awssdk.services.s3.presigner.model.GetObjectPresignRequest;

import java.time.Duration;

@Service
public class S3Service {

    private S3Client s3Client;
    private  S3Presigner presigner;
    private final String bucketName;

    public S3Service(@Value("${AWS_ACCESS_KEY_ID}") String s3AccessKeyId,
                     @Value("${AWS_SECRET_ACCESS_KEY}") String s3SecretAccessKey,
                     @Value("${INSTALLER_BUCKET_NAME}") String bucketName) {
        this.bucketName = bucketName;
        AwsBasicCredentials awsCreds = AwsBasicCredentials.create(s3AccessKeyId, s3SecretAccessKey);
        this.s3Client = S3Client.builder()
                .region(Region.US_WEST_1)
                .credentialsProvider(StaticCredentialsProvider.create(awsCreds))
                .build();

        this.presigner = S3Presigner.builder()
                .region(Region.US_WEST_1)
                .build();
    }

    public String generatePresignedUrl(String key) {
        GetObjectRequest getObjectRequest = GetObjectRequest.builder()
                .bucket(bucketName)
                .key(key)
                .build();

        GetObjectPresignRequest presignRequest = GetObjectPresignRequest.builder()
                .signatureDuration(Duration.ofMinutes(1))
                .getObjectRequest(getObjectRequest)
                .build();

        return presigner.presignGetObject(presignRequest).url().toString();
    }
}
