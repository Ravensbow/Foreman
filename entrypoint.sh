#!/bin/bash

set -e
run_cmd="dotnet run --project ./Foreman/Server --server.urls http://+:5000;https://+:5001"

until dotnet ef database update --project ./Foreman/Server; do
>&2 echo "SQL Server is starting up"
sleep 1
done

>&2 echo "SQL Server is up - executing command"
exec $run_cmd

