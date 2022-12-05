FROM mcr.microsoft.com/mssql/server:2019-latest
WORKDIR /app/database

COPY . /app/database

ENV ACCEPT_EULA Y
ENV SA_PASSWORD h9uHBC8h98h@3snds
ENV MSSQL_PID Express

EXPOSE 1433

CMD /bin/bash ./entrypoint.sh