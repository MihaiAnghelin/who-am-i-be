#!/bin/bash

docker build -t ghcr.io/mihaianghelin/who-am-i-be:master .
docker push ghcr.io/mihaianghelin/who-am-i-be:master

ssh mihui@192.168.0.33 "docker pull ghcr.io/mihaianghelin/who-am-i-be:master && docker compose -f /home/mihui/Docker/docker-compose.who-am-i.yaml up -d"
