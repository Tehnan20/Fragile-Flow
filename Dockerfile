# FROM ubuntu:22.04
# ENV DEBIAN_FRONTEND=noninteractive
# WORKDIR /app

# RUN apt-get update && apt-get install -y --no-install-recommends \
#     ca-certificates \
#     libglib2.0-0 \
#     libstdc++6 \
#     libgcc-s1 \
#     zlib1g \
#     libcurl4 \
#     libssl3 \
#     libicu70 \
#     libatomic1 \
#     && rm -rf /var/lib/apt/lists/*

# COPY server_build/ /app/

# RUN chmod +x /app/MyGameServer.x86_64

# EXPOSE 7777/tcp

# CMD ["/app/MyGameServer.x86_64", "-batchmode", "-nographics"]
# Use Ubuntu 22.04 as the base image
FROM ubuntu:22.04

# Install required Unity runtime libraries
# (libx11-6 and libasound2 stop Unity from crashing when probing hardware)
RUN apt-get update && DEBIAN_FRONTEND=noninteractive apt-get install -y \
    ca-certificates \
    libcap2 \
    libx11-6 \
    libasound2 \
    && rm -rf /var/lib/apt/lists/*

# Set the working directory inside the container
WORKDIR /app

# Copy the server_build folder from GitHub Actions into the container
COPY server_build/ /app/server_build/

# Make the Unity server executable
RUN chmod +x /app/server_build/MyGameServer.x86_64

# Expose KCP UDP port 7777
EXPOSE 7777/udp

# Run the server with all necessary headless flags
# NEW: Added -disable-audio to prevent the signo:11 crash
CMD ["/app/server_build/MyGameServer.x86_64", "-batchmode", "-nographics", "-disable-audio"]