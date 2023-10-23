FROM mcr.microsoft.com/dotnet/aspnet:7.0

COPY ConnectFourWeb/bin/Debug/net7.0/publish/ /webapp/

ENTRYPOINT [ "dotnet", "/webapp/ConnectFourWeb.dll", "--contentroot", "/webapp/" ]
CMD [ "--urls", "http://*:5000" ]
