-- name: CreateProducts :copyfrom
insert into store_product (store_name, barcode, name, brand, image_url, max_quantity, image_url, unit_and_size,
                           store_sku)
values (@store_name, @barcode, @name, @brand, @image_url, @max_quantity, @image_url, @unit_and_size, @store_sku);

-- name: GetStoreProducts :many
select sqlc.embed(store_product)
from store_product
where barcode = any (@skus::varchar(255)[])
  and store_name = @store_name
  and is_deleted = false;

-- name: GetStoreProductsByStore :many
select sqlc.embed(store_product)
from store_product
where store_name = @store_name
  and is_deleted = false;

-- name: UpdateStoreProduct :exec
update store_product
set name             = @name,
    brand            = @brand,
    image_url        = @image_url,
    max_quantity     = @max_quantity,
    last_updated_utc = now()
where barcode = @barcode
  and store_name = @store_name;

-- name: CreateCanonicalProducts :copyfrom
insert into product (barcode, brand, name, image_url, unit_and_size, max_quantity)
values (@barcode, @brand, @name, @image_url, @unit_and_size, @max_quantity);

-- name: GetCanonicalProducts :many
select sqlc.embed(product)
from product
where barcode = any (@barcodes::varchar(255)[])
  and is_deleted = false;

-- name: CreateCanonicalStoreProducts :copyfrom
insert into product_store_product (product_id, store_product_id)
values (@product_id, @store_product_id);

-- name: CreateCanonicalProduct :one
insert into product (barcode, brand, name, image_url, unit_and_size, max_quantity)
values (@barcode, @brand, @name, @image_url, @unit_and_size, @max_quantity)
returning *;

-- name: GetAllProducts :many
select p.product_id,
       p.barcode,
       p.image_url,
       p.brand,
       p.name,
       p.unit_and_size,
       p.max_quantity,
       sp.store_sku,
       sp.store_name
from product p
         join product_store_product psp on p.product_id = psp.product_id
         join store_product sp on psp.store_product_id = sp.store_product_id
where p.is_deleted = false
order by p.product_id asc;


-- name: createAppUser :one
insert into app_user (username, password_hash)
values (@username, @password_hash)
returning sqlc.embed(app_user);

-- name: createRefreshToken :one
insert into refresh_token (app_user_id, token, expires_utc)
values (@app_user_id, @token, @expires_utc)
returning sqlc.embed(refresh_token);

-- name: getRefreshToken :one
select sqlc.embed(refresh_token)
from refresh_token
where app_user_id = @app_user_id
  and is_deleted = false
  and expires_utc > now()
limit 1;

-- name: deleteRefreshToken :exec
update refresh_token
set is_deleted = true
where app_user_id = @app_user_id
  and token = @token;

-- name: getAppUser :one
select sqlc.embed(app_user)
from app_user
where username = @username
  and is_deleted = false
limit 1;

-- name: createWoolworthsSession :copyfrom
insert into woolworths_session (session_id, aga, address_id, expires_utc)
values (@session_id, @aga, @address_id, @expires_utc);

-- name: getWoolworthsSession :many
select sqlc.embed(woolworths_session)
from woolworths_session
where address_id = any (@address_ids::int[])
  and expires_utc > now()
order by expires_utc asc;

-- name: createPaknSaveSession :one
insert into paknsave_session (access_token, expires_utc)
values (@access_token, @expires_utc)
returning sqlc.embed(paknsave_session);

-- name: getPaknSaveSession :one
select sqlc.embed(paknsave_session)
from paknsave_session
where expires_utc > now()
limit 1;


-- name: addSelectedStore :copyfrom
insert into selected_stores (app_user_id, store_name, store_id)
values (@app_user_id, @store_name, @store_id);