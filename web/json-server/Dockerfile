FROM node:12.11.1 AS build
WORKDIR /src
COPY . .
RUN node -v && npm -v
RUN npm install
RUN npm run publish

FROM nginx:alpine AS deploy
# Remove default nginx website
RUN rm -rf /usr/share/nginx/html/*

# Add default nginx config
COPY  ./nginx/default.conf /etc/nginx/conf.d/default.conf

## From 'builder' copy website to default nginx public folder
COPY --from=build /src/dist /usr/share/nginx/html
EXPOSE 80
ENTRYPOINT ["nginx", "-g", "daemon off;"]
