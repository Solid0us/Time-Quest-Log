package com.solid0us.time_quest_log.service;

import io.jsonwebtoken.Claims;
import io.jsonwebtoken.io.Decoders;
import io.jsonwebtoken.security.Keys;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.stereotype.Service;
import java.util.Date;
import java.util.HashMap;
import java.util.Map;
import java.util.function.Function;
import io.jsonwebtoken.Jwts;
import javax.crypto.SecretKey;

@Service
public class JWTService {

    @Autowired
    private MyUserDetailsService userDetailsService;

    @Value("${JWT_SECRET}")
    private String secretKey;

    @Value("${REFRESH_TOKEN_SECRET}")
    private String refreshTokenSecret;

    public String generateToken(String username) {
        Map<String, Object> claims = new HashMap<>();
        return Jwts.builder()
                .claims()
                .add(claims)
                .subject(username.toLowerCase())
                .issuedAt(new Date(System.currentTimeMillis()))
                .expiration(new Date(System.currentTimeMillis() + 60 * 60 * 1000))
                .and()
                .signWith(getKey(false))
                .compact();
    }

    public String generateRefreshToken(String username) {
        Map<String, Object> claims = new HashMap<>();
        return Jwts.builder()
                .claims()
                .add(claims)
                .subject(username.toLowerCase())
                .issuedAt(new Date(System.currentTimeMillis()))
                .expiration(new Date(System.currentTimeMillis() + 1000 * 60 * 60 * 24 * 7))
                .and()
                .signWith(getKey(true))
                .compact();
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

    public boolean validateRefreshToken(String token, String username) {
        UserDetails userDetails = userDetailsService.loadUserByUsername(username);
        return username.equals(userDetails.getUsername()) && !isTokenExpired(token, true);
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
