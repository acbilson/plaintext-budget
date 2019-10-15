FROM node:12.11.1 AS build
# step 1: mount node_modules folder so I don't need to rebuild dependencies on every run
# step 2: run npm restore or npm update (or both?)
# step 3: run npm build with options. Send to /dist
# step 4: (optional) deploy to a new server, perhaps hosting as a static file 
WORKDIR .
CMD ["ls", "."]
