# development
FROM node:12.11.1-alpine AS dev
RUN npm install -g json-server
WORKDIR /srv
EXPOSE 80

# production
FROM dev AS prod
COPY db.json json-server.json routes.json /srv/
ENTRYPOINT ["json-server", "db.json"]
