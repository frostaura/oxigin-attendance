# Frontend.Dockerfile (for React/TypeScript Vite project)
FROM node:20-alpine AS build
WORKDIR /app
COPY src/frontend/package*.json ./
RUN npm install
COPY src/frontend/ ./
ENV NODE_OPTIONS=--max_old_space_size=2048
RUN npm run build

FROM nginx:alpine
COPY --from=build /app/build /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
