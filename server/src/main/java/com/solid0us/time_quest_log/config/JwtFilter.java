package com.solid0us.time_quest_log.config;

import com.solid0us.time_quest_log.service.JWTService;
import com.solid0us.time_quest_log.service.MyUserDetailsService;
import jakarta.servlet.FilterChain;
import jakarta.servlet.ServletException;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.web.authentication.WebAuthenticationDetailsSource;
import org.springframework.stereotype.Component;
import org.springframework.web.filter.OncePerRequestFilter;

import java.io.IOException;

@Component
public class JwtFilter extends OncePerRequestFilter {

    @Autowired
    private JWTService jwtService;

    @Autowired
    private MyUserDetailsService userDetailsService;

    @Override
    protected void doFilterInternal(HttpServletRequest request, HttpServletResponse response,
                                    FilterChain filterChain) throws ServletException, IOException {
        try {
            addSecurityHeaders(response);

            String token = extractTokenFromRequest(request);
            if (token == null) {
                filterChain.doFilter(request, response);
                return;
            }

            processTokenAuthentication(request, token);
            filterChain.doFilter(request, response);

        } catch (JwtAuthenticationException e) {
            handleAuthenticationFailure(response, e.getMessage());
        } catch (Exception e) {
            handleUnexpectedError(response);
        }
    }

    private void addSecurityHeaders(HttpServletResponse response) {
        response.setHeader("Cache-Control", "no-cache, no-store, must-revalidate");
        response.setHeader("Pragma", "no-cache");
        response.setHeader("Expires", "0");
    }

    private String extractTokenFromRequest(HttpServletRequest request) throws JwtAuthenticationException {
        String authHeader = request.getHeader("Authorization");

        if (authHeader == null || !authHeader.startsWith("Bearer ")) {
            return null;
        }

        String token = authHeader.substring(7);
        if (token.isEmpty()) {
            throw new JwtAuthenticationException("Empty token provided");
        }

        return token;
    }

    private void processTokenAuthentication(HttpServletRequest request, String token)
            throws JwtAuthenticationException {
        String username = extractAndValidateUsername(token);

        if (username != null && isAuthenticationRequired()) {
            UserDetails userDetails = loadAndValidateUserDetails(username, token);
            setSecurityContext(request, userDetails);
        }
    }

    private String extractAndValidateUsername(String token) throws JwtAuthenticationException {
        try {
            return jwtService.extractUsername(token, false);
        } catch (Exception e) {
            throw new JwtAuthenticationException("Failed to extract username from token: " + e.getMessage());
        }
    }

    private boolean isAuthenticationRequired() {
        return SecurityContextHolder.getContext().getAuthentication() == null;
    }

    private UserDetails loadAndValidateUserDetails(String username, String token)
            throws JwtAuthenticationException {
        UserDetails userDetails = userDetailsService.loadUserByUsername(username);

        if (!jwtService.validateToken(token, userDetails)) {
            throw new JwtAuthenticationException("Invalid token");
        }

        return userDetails;
    }

    private void setSecurityContext(HttpServletRequest request, UserDetails userDetails) {
        UsernamePasswordAuthenticationToken authToken =
                new UsernamePasswordAuthenticationToken(userDetails, null, userDetails.getAuthorities());
        authToken.setDetails(new WebAuthenticationDetailsSource().buildDetails(request));
        SecurityContextHolder.getContext().setAuthentication(authToken);
    }

    private void handleAuthenticationFailure(HttpServletResponse response, String message)
            throws IOException {
        response.setStatus(HttpServletResponse.SC_UNAUTHORIZED);
        response.getWriter().write("Authentication failed: " + message);
    }

    private void handleUnexpectedError(HttpServletResponse response) throws IOException {
        response.setStatus(HttpServletResponse.SC_INTERNAL_SERVER_ERROR);
        response.getWriter().write("Internal server error");
    }

    // Custom exception class
    private static class JwtAuthenticationException extends Exception {
        public JwtAuthenticationException(String message) {
            super(message);
        }
    }
}
