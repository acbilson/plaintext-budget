# development
FROM node:12.11.1 AS dev
WORKDIR /srv/PTB.Web
VOLUME /srv
EXPOSE 80

# production - build
FROM dev AS build
COPY . .
RUN node -v && npm -v
RUN npm install
RUN npm run publish

# production - deploy
FROM nginx:alpine AS prod
# Remove default nginx website
RUN rm -rf /usr/share/nginx/html/*

# Add default nginx config
COPY  ./nginx/default.conf /etc/nginx/conf.d/default.conf

## From 'builder' copy website to default nginx public folder
COPY --from=build /srv/dist /usr/share/nginx/html
EXPOSE 80
ENTRYPOINT ["nginx", "-g", "daemon off;"]
