# Choose ubuntu version
FROM mcr.microsoft.com/mssql/server:2019-latest
USER root

# Create app directory
RUN mkdir -p /usr/src/app
WORKDIR /usr/src/app

# Copy initialization scripts
COPY . /usr/src/app
             
# Set environment variables, not to have to write them with docker run command
# Note: make sure that your password matches what is in the run-initialization script 
ENV SA_PASSWORD h9uHBC8h98h@3snds
ENV ACCEPT_EULA Y
ENV MSSQL_PID Express

# Grant permissions for the import-data script to be executable
RUN chmod +x /usr/src/app/db-init.sh

# Expose port 1433 in case accessing from other container
# Expose port externally from docker-compose.yml
EXPOSE 1433

# Switch back to mssql user and run the entrypoint script
USER mssql
CMD /bin/bash ./entrypoint.sh