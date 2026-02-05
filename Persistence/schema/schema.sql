create extension if not exists "uuid-ossp";
drop table if exists canonical_store_product;
drop table if exists store_product;
drop table if exists canonical_product;

create table canonical_product
(
    canonical_product_id uuid primary key      default uuid_generate_v4(),

    barcode              varchar(255) not null,
    brand                varchar(255) not null,
    name                 varchar(255) not null,

    size_value           float        not null,
    size_unit            varchar(10)  not null,

    is_deleted           boolean      not null default false,
    created_utc          timestamptz  not null default now(),
    last_updated_utc     timestamptz  not null default now()
);

create table store_product
(
    store_product_id     uuid primary key        default uuid_generate_v4(),
    store_name           varchar(200)   not null,

    barcode              varchar(255)   not null,

    name                 varchar(255)   not null,
    brand                varchar(255),

    image_url            varchar(512)   not null default '',

    unit_and_size        varchar(255),
    max_quantity         numeric(10, 2) not null,

    is_deleted           boolean        not null default false,
    created_utc          timestamptz    not null default now(),
    last_updated_utc     timestamptz    not null default now(),

    constraint storeNameCheck check (store_name in ('woolworths', 'paknsave', 'newworld'))
);

create table canonical_store_product
(
    canonical_product_id uuid not null,
    store_product_id     uuid not null,

    constraint fk_canonical_productId foreign key (canonical_product_id)
        references canonical_product (canonical_product_id)
        on delete cascade,
    constraint fk_store_productId foreign key (store_product_id)
        references store_product (store_product_id)
        on delete cascade
);

-- update canonical_product
-- set search_vector = to_tsvector('english',
--                                 coalesce(name_normalised, '') || ' ' || coalesce(brand_normalised, ''));
-- 
-- create index idx_product_search_vector on canonical_product using gin (search_vector);
-- 
-- create function product_search_vector_trigger() returns trigger as
-- $$
-- begin
--     new.search_vector :=
--             to_tsvector('english',
--                         coalesce(new.name_normalised, '') || ' ' || coalesce(new.brand_normalised, ''));
--     return new;
-- end
-- $$ language plpgsql;
-- 
-- create trigger tsvectorupdate
--     before insert or update
--     on canonical_product
--     for each row
-- execute function product_search_vector_trigger();
