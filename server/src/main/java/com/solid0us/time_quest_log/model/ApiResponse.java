package com.solid0us.time_quest_log.model;

import java.time.LocalDateTime;
import java.util.List;

public class ApiResponse<T> {
    private String status;
    private String message;
    private T data;
    private LocalDateTime timestamp;
    private List<ErrorDetail> errors;
    private Pagination pagination;

    public ApiResponse(String status, String message) {
        this.status = status;
        this.message = message;
        this.timestamp = LocalDateTime.now();
    }

    public ApiResponse(String status, T data) {
        this.status = status;
        this.data = data;
        this.timestamp = LocalDateTime.now();
    }

    public ApiResponse(String status, String message, T data) {
        this.status = status;
        this.message = message;
        this.data = data;
        this.timestamp = LocalDateTime.now();
    }

    public ApiResponse(String status, String message, T data, List<ErrorDetail> errors) {
        this(status, message, data);
        this.errors = errors;
    }

    public ApiResponse(String status, String message, List<ErrorDetail> errors) {
        this(status, message);
        this.errors = errors;
    }

    public ApiResponse(String status, String message, T data, Pagination pagination) {
        this(status, message, data);
        this.pagination = pagination;
    }

    public static <T> ApiResponse<T> success(String message, T data) {
        return new ApiResponse<>("success", message, data);
    }

    public static <T> ApiResponse<T> success(String message, T data, Pagination pagination) {
        return new ApiResponse<>("success", message, data, pagination);
    }

    public static <T> ApiResponse<T> failure(String message, T data, List<ErrorDetail> errors) {
        return new ApiResponse<>("failure", message, data, errors);
    }

    public static <T> ApiResponse<T> failure(String message, List<ErrorDetail> errors) {
        return new ApiResponse<>("failure", message, errors);
    }

    public String getStatus() {
        return status;
    }

    public void setStatus(String status) {
        this.status = status;
    }

    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }

    public T getData() {
        return data;
    }

    public void setData(T data) {
        this.data = data;
    }

    public LocalDateTime getTimestamp() {
        return timestamp;
    }

    public List<ErrorDetail> getErrors() {
        return errors;
    }

    public void setErrors(List<ErrorDetail> errors) {
        this.errors = errors;
    }

    public Pagination getPagination() {
        return pagination;
    }

    public void setPagination(Pagination pagination) {
        this.pagination = pagination;
    }
}
