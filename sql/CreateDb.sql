CREATE DATABASE onion_architecture;
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
CREATE COLLATION case_insensitive (provider = icu, locale = 'und-u-ks-level2', deterministic = false);