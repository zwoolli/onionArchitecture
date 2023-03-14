DROP TABLE IF EXISTS account;

CREATE TABLE account (
    account_id UUID NOT NULL PRIMARY KEY DEFAULT uuid_generate_v4(),
    owner_id UUID NOT NULL REFERENCES owner(owner_id),
    account_type VARCHAR(50) NOT NULL,
    date_created DATE NOT NULL
);