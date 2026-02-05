-- name: CreateProducts :copyfrom
insert into store_product (store_name, barcode, name, brand, image_url, max_quantity, image_url, unit_and_size)
values (@store_name, @barcode, @name, @brand, @image_url, @max_quantity, @image_url, @unit_and_size);

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
       sp.store_name
from product p
         join product_store_product psp on p.product_id = psp.product_id
         join store_product sp on psp.store_product_id = sp.store_product_id
where p.is_deleted = false
order by p.product_id asc;

