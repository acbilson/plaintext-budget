FROM node:latest
RUN npm install -g json-server
WORKDIR /data
VOLUME /data
EXPOSE 80
ENTRYPOINT ["json-server", "db.json"]
