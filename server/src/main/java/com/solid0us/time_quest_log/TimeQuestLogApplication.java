package com.solid0us.time_quest_log;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.EnableAutoConfiguration;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.autoconfigure.domain.EntityScan;
import org.springframework.context.annotation.ComponentScan;
import org.springframework.data.jpa.repository.config.EnableJpaRepositories;

@SpringBootApplication
@ComponentScan(basePackages = {"com.solid0us.time_quest_log"})
public class TimeQuestLogApplication {

	public static void main(String[] args) {
		SpringApplication.run(TimeQuestLogApplication.class, args);
	}

}
