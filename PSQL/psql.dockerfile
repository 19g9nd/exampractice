FROM postgres:latest
ENV POSTGRES_USER=postgres
ENV POSTGRES_PASSWORD=Pass1234!
ENV POSTGRES_DB=mydb 
EXPOSE 5432
# docker exec -it mypostgresdb psql -U postgres where -U is username