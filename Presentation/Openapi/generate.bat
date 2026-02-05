@echo off
openapi-generator-cli generate ^
  -i http://192.168.0.100:5112/swagger/v1/swagger.json ^
  -c openapi-dart.yaml
