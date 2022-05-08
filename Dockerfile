# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk
ENV PATH $PATH:/root/.dotnet/tools
RUN dotnet tool install -g dotnet-ef 
COPY . /foreman
WORKDIR /foreman
RUN bash -c 'ls'
RUN ["dotnet","restore"]
RUN ["dotnet", "build"]
RUN ["dotnet","dev-certs","https"]
EXPOSE 5000/tcp
EXPOSE 5001/tcp
RUN chmod +x ./entrypoint.sh
CMD /bin/bash ./entrypoint.sh
