CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

drop  table if exists Product;
create table Product
(
    id               UUID PRIMARY KEY      DEFAULT uuid_generate_v4(),
    name             varchar(255) not null,
    brand            varchar(255),
    is_deleted       boolean      not null default false,
    created_utc      TIMESTAMPTZ  not null default now(),
    last_updated_utc TIMESTAMPTZ  not null default now()
);