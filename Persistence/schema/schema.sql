CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

drop table if exists Product;
create table Product
(
    id               UUID PRIMARY KEY        DEFAULT uuid_generate_v4(),
    sku              varchar(255)   not null,
    name             varchar(255)   not null,
    brand            varchar(255),
    store_type       smallint       not null,
    image_url        varchar(512),
    max_quantity     numeric(10, 2) not null,
    is_deleted       boolean        not null default false,
    created_utc      TIMESTAMPTZ    not null default now(),
    last_updated_utc TIMESTAMPTZ    not null default now()
);

-- optional: define type if needed
create type sku_store_pair as (sku text, store_name text);
