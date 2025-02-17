package com.solid0us.time_quest_log.service;

import com.solid0us.time_quest_log.model.RefreshTokens;
import com.solid0us.time_quest_log.model.Users;
import com.solid0us.time_quest_log.repositories.RefreshTokenRepository;
import io.jsonwebtoken.Claims;
import io.jsonwebtoken.io.Decoders;
import io.jsonwebtoken.security.Keys;
import jakarta.transaction.Transactional;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.stereotype.Service;

import java.sql.Timestamp;
import java.util.Date;
import java.util.HashMap;
import java.util.Map;
import java.util.function.Function;
import io.jsonwebtoken.Jwts;
import javax.crypto.SecretKey;

@Service
public class JWTService {
    private final long JWT_EXPIRATION_MILLISECONDS = 1000 * 60 * 60;
    private final long REFRESH_TOKEN_EXPIRATION_MILLISECONDS = 1000 * 60 * 60 * 24 * 7;

    @Autowired
    private MyUserDetailsService userDetailsService;

    @Autowired
    private RefreshTokenRepository refreshTokenRepository;

    @Value("${JWT_SECRET}")
    private String secretKey;

    @Value("${REFRESH_TOKEN_SECRET}")
    private String refreshTokenSecret;

    public String generateToken(Users user) {
        Map<String, Object> claims = new HashMap<>();
        return Jwts.builder()
                .claims()
                .add(claims)
                .add("id",user.getId().toString())
                .subject(user.getUsername().toLowerCase())
                .issuedAt(new Date(System.currentTimeMillis()))
                .expiration(new Date(System.currentTimeMillis() + JWT_EXPIRATION_MILLISECONDS))
                .and()
                .signWith(getKey(false))
                .compact();
    }

    @Transactional(rollbackOn = Exception.class)
    public String generateRefreshToken(Users user) {
        Map<String, Object> claims = new HashMap<>();
        long expirationTime = System.currentTimeMillis() + REFRESH_TOKEN_EXPIRATION_MILLISECONDS;
        String refreshToken  = Jwts.builder()
                .claims()
                .add(claims)
                .add("id",user.getId().toString())
                .subject(user.getUsername().toLowerCase())
                .issuedAt(new Date(System.currentTimeMillis()))
                .expiration(new Date(expirationTime))
                .and()
                .signWith(getKey(true))
                .compact();
        refreshTokenRepository.save(new RefreshTokens(
                user,
                refreshToken,
                new Timestamp(expirationTime)
        ));
        return refreshToken;
    }

    private SecretKey getKey(boolean isRefreshToken) {
        byte[] keyBytes = isRefreshToken ? Decoders.BASE64.decode(refreshTokenSecret) : Decoders.BASE64.decode(secretKey);
        return Keys.hmacShaKeyFor(keyBytes);
    }

    public String extractUsername(String token, boolean isRefreshToken) {
        return extractClaim(token, Claims::getSubject, isRefreshToken);
    }

    public boolean validateToken(String token, UserDetails userDetails) {
        final String userName = extractUserName(token, false).toLowerCase();
        return (userName.equals(userDetails.getUsername().toLowerCase()) && !isTokenExpired(token, false));
    }

    @Transactional(rollbackOn = Exception.class)
    public boolean validateRefreshToken(String token) {
        RefreshTokens existingRefreshToken = refreshTokenRepository.findByRefreshToken(token);
        return existingRefreshToken != null
                && !isTokenExpired(existingRefreshToken.getRefreshToken(), true)
                && !existingRefreshToken.isRevoked();
    }

    public String extractUserName(String token, boolean isRefreshToken) {
        return extractClaim(token, Claims::getSubject, isRefreshToken);
    }

    private <T> T extractClaim(String token, Function<Claims, T> claimResolver, boolean isRefreshToken) {
        final Claims claims = extractAllClaims(token, isRefreshToken);
        return claimResolver.apply(claims);
    }

    private Claims extractAllClaims(String token, boolean isRefreshToken) {
        return Jwts.parser()
                .verifyWith(getKey(isRefreshToken))
                .build()
                .parseSignedClaims(token)
                .getPayload();
    }

    private boolean isTokenExpired(String token, boolean isRefreshToken) {
        return extractExpiration(token, isRefreshToken).before(new Date());
    }

    private Date extractExpiration(String token, boolean isRefreshToken) {
        return extractClaim(token, Claims::getExpiration, isRefreshToken);
    }
}
