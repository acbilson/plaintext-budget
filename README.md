# Plaintext Budget

A budget system made entirely from text files inspired by the Plaintext movement and todo.txt. In its current iteration,
the production version runs two Docker containers configurable to work locally or in AWS Fargate.

# Docker Components

- .NET Core 2.1 served with Kestrel
- Angular frontend served with nginx
- Json-server for testing served with nginx

# Build Instructions

The PowerShell script builds multiple configurations using docker-compose. For example, to build the development version
of the web server, type:

```./build.ps1 build dev web```

To build the backend server for testing the front-end:

```./build.ps1 build dev json-server```

It is also possible to generate production versions, but I recommend against that today because this application is not
in a release state yet.

# Development Instructions

To develop the web frontend, you'll want to build a new Docker image for both the web server and the mock
json-server backend.

```./build.ps1 build dev json-server```
```./build.ps1 build dev web```

Then start those containers:

```./build.ps1 start dev json-server```
```./build.ps1 start dev web```

You can see the json-server at http://localhost:5000 and the web frontend at http://localhost:4200

I've had sporadic success setting up this configuration to recompile my changes dynamically, so it's often necessary to
restart the server to view my changes.

```./build.ps1 restart dev web```

Finally, when you're done for the day, spin down your Docker containers:

```./build.ps1 stop dev web```

# Deploy Instructions

Requires an AWS account. Will add details at a later date.
