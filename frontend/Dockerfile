# Frontend Dockerfile
FROM node:18-alpine as build

# Set working directory
WORKDIR /app

# Copy package.json and package-lock.json
COPY package*.json ./

# Install dependencies
RUN npm ci

# Copy all files
COPY . .

# Create env-config.js at build time to capture build-time environment variables
RUN echo "window.ENV = { \
    BackendApi__Url: '${BackendApi__Url:-http://backend:8080}', \
    ASPNETCORE_ENVIRONMENT: '${ASPNETCORE_ENVIRONMENT:-Production}' \
};" > public/env-config.js

# Build the app
RUN npm run build

# Production stage
FROM nginx:alpine as production

# Copy built assets from the build stage
COPY --from=build /app/dist /usr/share/nginx/html

# Add script to update env-config.js at runtime with actual environment variables
COPY docker-entrypoint.sh /
RUN chmod +x /docker-entrypoint.sh

# Copy nginx config
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

# Use custom entrypoint script
ENTRYPOINT ["/docker-entrypoint.sh"]
CMD ["nginx", "-g", "daemon off;"]