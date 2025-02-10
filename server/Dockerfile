FROM maven:3.9.6-eclipse-temurin-21 AS build
COPY . .
# Run maven build with environment variables
RUN export $(cat .env | xargs) && \
    mvn clean package -DskipTests

FROM eclipse-temurin:21-jdk

# Create directory for the app
WORKDIR /app

# Copy jar and necessary files
COPY --from=build /target/*.jar app.jar
COPY .env .env

# Set environment variable to read .env file
ENV SPRING_CONFIG_IMPORT=optional:file:.env

# Spring specific environment variables
ENV SPRING_JPA_HIBERNATE_DDL_AUTO=update
ENV SPRING_JPA_PROPERTIES_HIBERNATE_DIALECT=org.hibernate.dialect.PostgreSQLDialect
ENV SPRING_JPA_SHOW_SQL=true

EXPOSE 8080
ENTRYPOINT ["java", "-jar", "app.jar"]
