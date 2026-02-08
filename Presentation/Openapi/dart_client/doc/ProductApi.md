# grocery_api.api.ProductApi

## Load the API package
```dart
import 'package:grocery_api/api.dart';
```

All URIs are relative to *http://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**getCategories**](ProductApi.md#getcategories) | **GET** /Product/categories | 
[**productPriceAsync**](ProductApi.md#productpriceasync) | **POST** /Product/price | 
[**searchProducts**](ProductApi.md#searchproducts) | **GET** /Product/search | 
[**syncCanonicalProducts**](ProductApi.md#synccanonicalproducts) | **POST** /Product/sync/canonical | 
[**syncWoolworths**](ProductApi.md#syncwoolworths) | **POST** /Product/sync | 


# **getCategories**
> List<Categoery> getCategories()



### Example
```dart
import 'package:grocery_api/api.dart';

final api_instance = ProductApi();

try {
    final result = api_instance.getCategories();
    print(result);
} catch (e) {
    print('Exception when calling ProductApi->getCategories: $e\n');
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**List<Categoery>**](Categoery.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **productPriceAsync**
> List<ProductsPriceResponse> productPriceAsync(productsPriceRequest)



### Example
```dart
import 'package:grocery_api/api.dart';

final api_instance = ProductApi();
final productsPriceRequest = [List<ProductsPriceRequest>()]; // List<ProductsPriceRequest> | 

try {
    final result = api_instance.productPriceAsync(productsPriceRequest);
    print(result);
} catch (e) {
    print('Exception when calling ProductApi->productPriceAsync: $e\n');
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **productsPriceRequest** | [**List<ProductsPriceRequest>**](ProductsPriceRequest.md)|  | [optional] 

### Return type

[**List<ProductsPriceResponse>**](ProductsPriceResponse.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json, text/json, application/*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **searchProducts**
> List<ProductResponse> searchProducts(term, limit, skip)



### Example
```dart
import 'package:grocery_api/api.dart';

final api_instance = ProductApi();
final term = term_example; // String | 
final limit = 56; // int | 
final skip = 56; // int | 

try {
    final result = api_instance.searchProducts(term, limit, skip);
    print(result);
} catch (e) {
    print('Exception when calling ProductApi->searchProducts: $e\n');
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **term** | **String**|  | [optional] 
 **limit** | **int**|  | [optional] 
 **skip** | **int**|  | [optional] 

### Return type

[**List<ProductResponse>**](ProductResponse.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **syncCanonicalProducts**
> bool syncCanonicalProducts()



### Example
```dart
import 'package:grocery_api/api.dart';

final api_instance = ProductApi();

try {
    final result = api_instance.syncCanonicalProducts();
    print(result);
} catch (e) {
    print('Exception when calling ProductApi->syncCanonicalProducts: $e\n');
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

**bool**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **syncWoolworths**
> bool syncWoolworths(storeName)



### Example
```dart
import 'package:grocery_api/api.dart';

final api_instance = ProductApi();
final storeName = [List<StoreName>()]; // List<StoreName> | 

try {
    final result = api_instance.syncWoolworths(storeName);
    print(result);
} catch (e) {
    print('Exception when calling ProductApi->syncWoolworths: $e\n');
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **storeName** | [**List<StoreName>**](StoreName.md)|  | [optional] 

### Return type

**bool**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json, text/json, application/*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

