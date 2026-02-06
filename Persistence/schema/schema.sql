create extension if not exists "uuid-ossp";
drop table if exists product_store_product;
drop table if exists product;
drop table if exists store_product;
drop table if exists refresh_token;
drop table if exists app_user;


create table product
(
    product_id       uuid primary key      default uuid_generate_v4(),

    barcode          varchar(255) not null,
    brand            varchar(255) not null,
    name             varchar(255) not null,

    image_url        varchar(512) not null default '',
    unit_and_size    varchar(255),
    max_quantity     float,

    is_deleted       boolean      not null default false,
    created_utc      timestamptz  not null default now(),
    last_updated_utc timestamptz  not null default now()
);

create table store_product
(
    store_product_id uuid primary key      default uuid_generate_v4(),
    store_name       varchar(200) not null,

    barcode          varchar(255) not null,
    store_sku        varchar(255) not null,

    name             varchar(255) not null,
    brand            varchar(255),

    image_url        varchar(512) not null default '',

    unit_and_size    varchar(255),
    max_quantity     float,

    is_deleted       boolean      not null default false,
    created_utc      timestamptz  not null default now(),
    last_updated_utc timestamptz  not null default now(),

    constraint storeNameCheck check (store_name in ('woolworths', 'paknsave', 'newworld'))
);

create table product_store_product
(
    product_id       uuid not null,
    store_product_id uuid not null,

    constraint fk_productId foreign key (product_id)
        references product (product_id)
        on delete cascade,
    constraint fk_store_productId foreign key (store_product_id)
        references store_product (store_product_id)
        on delete cascade
);

create table app_user
(
    app_user_id      uuid primary key      default uuid_generate_v4(),
    username            varchar(255) not null unique,
    password_hash    varchar(255) not null,

    is_deleted       boolean      not null default false,
    created_utc      timestamptz  not null default now(),
    last_updated_utc timestamptz  not null default now()
);

insert into app_user (username, password_hash)
values ('admin', 'admin');

create table refresh_token
(
    app_user_id      uuid         not null,
    token            varchar(255) not null,
    expires_utc      timestamptz  not null,

    is_deleted       boolean      not null default false,
    created_utc      timestamptz  not null default now(),
    last_updated_utc timestamptz  not null default now(),

    constraint fk_userId foreign key (app_user_id)
        references app_user (app_user_id)
        on delete cascade
);

