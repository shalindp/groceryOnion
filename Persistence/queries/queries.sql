-- name: CreateProducts :copyfrom
insert into store_product (store_name, barcode, name, brand, image_url, max_quantity, image_url, unit_and_size)
values (@store_name, @barcode, @name, @brand, @image_url, @max_quantity, @image_url, @unit_and_size);

-- name: SearchProducts :many
SELECT sqlc.embed(canonical_product)
FROM canonical_product
WHERE is_deleted = false
  AND search_vector @@ plainto_tsquery('english', @query)
ORDER BY ts_rank_cd(search_vector, plainto_tsquery('english', @query)) DESC, canonical_product_id asc
LIMIT sqlc.narg('limit')::int OFFSET sqlc.narg('offset')::int;

-- name: GetStoreProducts :many
select sqlc.embed(store_product)
from store_product
where barcode = any(@skus::varchar(255)[]) and store_name = @store_name and is_deleted = false;

-- name: GetStoreProductsByStore :many
select sqlc.embed(store_product)
from store_product
where store_name = @store_name and is_deleted = false;

-- name: UpdateStoreProduct :exec
update store_product set
                   name = @name,
                   brand = @brand,
                   image_url = @image_url,
                   max_quantity = @max_quantity,
                   last_updated_utc = now()
where barcode = @barcode and store_name = @store_name;