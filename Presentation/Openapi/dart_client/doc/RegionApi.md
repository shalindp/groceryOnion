# grocery_api.api.RegionApi

## Load the API package
```dart
import 'package:grocery_api/api.dart';
```

All URIs are relative to *http://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**createSessionWithRegionsAsync**](RegionApi.md#createsessionwithregionsasync) | **POST** /Region/create-session | 
[**getAllRegions**](RegionApi.md#getallregions) | **GET** /Region | 


# **createSessionWithRegionsAsync**
> List<CreateSessionWithRegionResponse> createSessionWithRegionsAsync(createSessionWithRegionId)



### Example
```dart
import 'package:grocery_api/api.dart';

final api_instance = RegionApi();
final createSessionWithRegionId = [List<CreateSessionWithRegionId>()]; // List<CreateSessionWithRegionId> | 

try {
    final result = api_instance.createSessionWithRegionsAsync(createSessionWithRegionId);
    print(result);
} catch (e) {
    print('Exception when calling RegionApi->createSessionWithRegionsAsync: $e\n');
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **createSessionWithRegionId** | [**List<CreateSessionWithRegionId>**](CreateSessionWithRegionId.md)|  | 

### Return type

[**List<CreateSessionWithRegionResponse>**](CreateSessionWithRegionResponse.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json, text/json, application/*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

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

