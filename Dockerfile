######## Build Source ########
FROM ccr.ccs.tencentyun.com/wlniao/dotnet-sdk:8.0.0-alpine as publish
COPY ./ /code
WORKDIR /code
RUN dotnet publish -o /dist --self-contained -c Release -p:PublishAot=true -p:PublishTrimmed=true


######## Build Images ########
FROM ccr.ccs.tencentyun.com/wlniao/aot:8.0-alpine as builder
COPY --chmod=0777 --from=publish /dist/getimer /wln
WORKDIR /wln
ENV WaitSeconds=0
ENV URLS=""
CMD getimer