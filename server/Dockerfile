FROM maven:3.9.6-eclipse-temurin-21 AS build
COPY . .
RUN mvn clean package -DskipTests

FROM eclipse-temurin:21-jdk
COPY --from=build /target/*.jar time-quest-log.jar
COPY src/main/resources/application.properties /config/application.properties
EXPOSE 8080
ENTRYPOINT ["java", "-jar", "time-quest-log.jar"]