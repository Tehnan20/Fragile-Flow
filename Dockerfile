FROM ubuntu:22.04
ENV DEBIAN_FRONTEND=noninteractive
WORKDIR /app

RUN apt-get update && apt-get install -y --no-install-recommends \
    ca-certificates \
    libglib2.0-0 \
    libstdc++6 \
    libgcc-s1 \
    zlib1g \
    libcurl4 \
    libssl3 \
    libicu70 \
    libatomic1 \
    && rm -rf /var/lib/apt/lists/*

COPY server_build/ /app/

RUN chmod +x /app/MyGameServer.x86_64

EXPOSE 7777/tcp

CMD ["/app/MyGameServer.x86_64", "-batchmode", "-nographics"]
