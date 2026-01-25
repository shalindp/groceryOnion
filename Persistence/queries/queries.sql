-- name: CreateProduct :one
insert into Product (name, brand)
values (@name, @brand) returning sqlc.embed(Product);



-- name: CreateProducts :copyfrom
insert into Product (name, brand)
values (@name, @brand);