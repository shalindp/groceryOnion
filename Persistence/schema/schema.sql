CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

drop table if exists Product_Price;
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

CREATE TABLE Product_Price
(
    product_id      UUID           NOT NULL
        REFERENCES Product (id) ON DELETE CASCADE,

    product_sku     varchar(255)   NOT NULL,
    store_type      smallint       NOT NULL,
    region_id       integer        NOT NULL,

    original_price  numeric(10, 2) NOT NULL,
    sale_price      numeric(10, 2),
    multi_buy_price numeric(10, 2),

    created_utc      timestamptz NOT NULL DEFAULT now(),
    last_updated_utc timestamptz NOT NULL DEFAULT now(),

    -- 👇 composite identity for price rows
    CONSTRAINT pk_product_price
        PRIMARY KEY (product_id, product_sku, store_type, region_id)
);

ALTER TABLE Product
    ADD COLUMN search_vector tsvector;

-- populate it for existing rows
UPDATE Product
SET search_vector = to_tsvector('english', coalesce(name, '') || ' ' || coalesce(brand, '') || ' ' || coalesce(sku, ''));

CREATE INDEX idx_product_search_vector ON Product USING GIN(search_vector);

CREATE FUNCTION product_search_vector_trigger() RETURNS trigger AS $$
BEGIN
    NEW.search_vector :=
            to_tsvector('english', coalesce(NEW.name, '') || ' ' || coalesce(NEW.brand, '') || ' ' || coalesce(NEW.sku, ''));
    RETURN NEW;
END
$$ LANGUAGE plpgsql;

CREATE TRIGGER tsvectorupdate BEFORE INSERT OR UPDATE
    ON Product FOR EACH ROW EXECUTE FUNCTION product_search_vector_trigger();
