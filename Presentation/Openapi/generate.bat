@echo off
openapi-generator-cli generate ^
  -i http://localhost:5112/swagger/v1/swagger.json ^
  -c openapi-dart.yaml
