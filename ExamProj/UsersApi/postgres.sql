CREATE TABLE Users (
    Id INT PRIMARY KEY,
    Name VARCHAR(255) NOT NULL
);

INSERT INTO Users (Id, Name) VALUES (1, 'John Doe') RETURNING *;
select * from Users