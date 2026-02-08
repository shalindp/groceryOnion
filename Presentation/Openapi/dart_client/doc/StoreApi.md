# grocery_api.api.StoreApi

## Load the API package
```dart
import 'package:grocery_api/api.dart';
```

All URIs are relative to *http://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**selectStoresAsync**](StoreApi.md#selectstoresasync) | **POST** /Store/select | 
[**storesAsync**](StoreApi.md#storesasync) | **GET** /Store | 


# **selectStoresAsync**
> bool selectStoresAsync(selectStoresRequest)



### Example
```dart
import 'package:grocery_api/api.dart';

final api_instance = StoreApi();
final selectStoresRequest = SelectStoresRequest(); // SelectStoresRequest | 

try {
    final result = api_instance.selectStoresAsync(selectStoresRequest);
    print(result);
} catch (e) {
    print('Exception when calling StoreApi->selectStoresAsync: $e\n');
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **selectStoresRequest** | [**SelectStoresRequest**](SelectStoresRequest.md)|  | [optional] 

### Return type

**bool**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json, text/json, application/*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **storesAsync**
> List<StoreResponse> storesAsync()



### Example
```dart
import 'package:grocery_api/api.dart';

final api_instance = StoreApi();

try {
    final result = api_instance.storesAsync();
    print(result);
} catch (e) {
    print('Exception when calling StoreApi->storesAsync: $e\n');
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**List<StoreResponse>**](StoreResponse.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

