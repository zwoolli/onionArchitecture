DROP TABLE IF EXISTS owner;

CREATE TABLE owner (
    owner_id UUID NOT NULL PRIMARY KEY DEFAULT uuid_generate_v4(),
    name VARCHAR(60) NOT NULL,
    date_of_birth DATE NOT NULL,
    address VARCHAR(100) NOT NULL
);