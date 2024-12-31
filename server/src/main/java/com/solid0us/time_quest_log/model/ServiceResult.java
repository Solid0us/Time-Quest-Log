package com.solid0us.time_quest_log.model;

import java.util.ArrayList;
import java.util.List;

public class ServiceResult<T> {
    private T data;
    private List<ErrorDetail> errors = new ArrayList<>();

    private ServiceResult(T data) {
        this.data = data;
    }

    private ServiceResult(T data, List<ErrorDetail> errors) {
        this.data = data;
        this.errors = errors;
    }

    public static <T> ServiceResult<T> success(T data) {
        return new ServiceResult<>(data, null);
    }

    public static <T> ServiceResult<T> failure(List<ErrorDetail> errors) {
        return new ServiceResult<>(null, errors);
    }

    public T getData() {
        return data;
    }

    public List<ErrorDetail> getErrors() {
        return errors;
    }

    public boolean isSuccess() {
        return errors == null || errors.isEmpty();
    }
}