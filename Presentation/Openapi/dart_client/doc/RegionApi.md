# grocery_api.api.RegionApi

## Load the API package
```dart
import 'package:grocery_api/api.dart';
```

All URIs are relative to *http://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**getAllRegions**](RegionApi.md#getallregions) | **GET** /Region | 


# **getAllRegions**
> List<WoolworthsGetRegionsResult> getAllRegions()



### Example
```dart
import 'package:grocery_api/api.dart';

final api_instance = RegionApi();

try {
    final result = api_instance.getAllRegions();
    print(result);
} catch (e) {
    print('Exception when calling RegionApi->getAllRegions: $e\n');
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**List<WoolworthsGetRegionsResult>**](WoolworthsGetRegionsResult.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

