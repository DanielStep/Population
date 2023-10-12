FROM mcr.microsoft.com/mssql/server:2019-latest

ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=Password123!

COPY setup.sql /usr/src/app/
WORKDIR /usr/src/app

VOLUME /var/opt/mssql/data
COPY ABS_C16_T01_TS_SA_08062021164508583.xls /usr/src/app/ABS_C16_T01_TS_SA_08062021164508583.xls