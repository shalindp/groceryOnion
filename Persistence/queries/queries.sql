-- name: CreateProduct :one
insert into Product (name, brand, sku, store_type, image_url, max_quantity)
values (@name, @brand, @sku, @store_type, @image_url, @max_quanitity)
returning sqlc.embed(Product);

-- name: CreateProducts :copyfrom
insert into Product (name, brand, sku, store_type, image_url, max_quantity)
values (@name, @brand, @sku, @store_type, @image_url, @max_quantity);

-- name: GetWoolworthsProducts :many
select *
from product
where sku = any(@skus::varchar(255)[]) and store_type = 0 and is_deleted = false;

-- name: UpdateProduct :exec
update product set
    name = @name,
    brand = @brand,
    image_url = @image_url,
    max_quantity = @max_quantity,
    last_updated_utc = now()
where sku = @sku and store_type = @store_type;

-- name: CreateProductPrice :exec
insert into Product_Price (product_id, product_sku, store_type, region_id, original_price, sale_price, multi_buy_price)
values (@product_id, @product_sku, @store_type, @region_id, @original_price, @sale_price, @multi_buy_price);

-- name: SearchProducts :many
SELECT sqlc.embed(Product)
FROM Product
WHERE is_deleted = false
  AND search_vector @@ plainto_tsquery('english', @query)
ORDER BY ts_rank_cd(search_vector, plainto_tsquery('english', @query)) DESC, id asc 
LIMIT sqlc.narg('limit')::int OFFSET sqlc.narg('offset')::int;

