# grocery_api.api.UserApi

## Load the API package
```dart
import 'package:grocery_api/api.dart';
```

All URIs are relative to *http://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**refreshTokenAsync**](UserApi.md#refreshtokenasync) | **POST** /User/refresh | 
[**signInAsync**](UserApi.md#signinasync) | **POST** /User/sign-in | 
[**signUpAsync**](UserApi.md#signupasync) | **POST** /User/sign-up | 


# **refreshTokenAsync**
> SignInResponse refreshTokenAsync(refreshRequest)



### Example
```dart
import 'package:grocery_api/api.dart';

final api_instance = UserApi();
final refreshRequest = RefreshRequest(); // RefreshRequest | 

try {
    final result = api_instance.refreshTokenAsync(refreshRequest);
    print(result);
} catch (e) {
    print('Exception when calling UserApi->refreshTokenAsync: $e\n');
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **refreshRequest** | [**RefreshRequest**](RefreshRequest.md)|  | [optional] 

### Return type

[**SignInResponse**](SignInResponse.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json, text/json, application/*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **signInAsync**
> SignInResponse signInAsync(signInRequest)



### Example
```dart
import 'package:grocery_api/api.dart';

final api_instance = UserApi();
final signInRequest = SignInRequest(); // SignInRequest | 

try {
    final result = api_instance.signInAsync(signInRequest);
    print(result);
} catch (e) {
    print('Exception when calling UserApi->signInAsync: $e\n');
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **signInRequest** | [**SignInRequest**](SignInRequest.md)|  | [optional] 

### Return type

[**SignInResponse**](SignInResponse.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json, text/json, application/*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **signUpAsync**
> SignInResponse signUpAsync(signUpRequest)



### Example
```dart
import 'package:grocery_api/api.dart';

final api_instance = UserApi();
final signUpRequest = SignUpRequest(); // SignUpRequest | 

try {
    final result = api_instance.signUpAsync(signUpRequest);
    print(result);
} catch (e) {
    print('Exception when calling UserApi->signUpAsync: $e\n');
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **signUpRequest** | [**SignUpRequest**](SignUpRequest.md)|  | [optional] 

### Return type

[**SignInResponse**](SignInResponse.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json, text/json, application/*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

