//
// AUTO-GENERATED FILE, DO NOT MODIFY!
//
// @dart=2.18

// ignore_for_file: unused_element, unused_import
// ignore_for_file: always_put_required_named_parameters_first
// ignore_for_file: constant_identifier_names
// ignore_for_file: lines_longer_than_80_chars

part of openapi.api;


class UserApi {
  UserApi([ApiClient? apiClient]) : apiClient = apiClient ?? defaultApiClient;

  final ApiClient apiClient;

  /// Performs an HTTP 'POST /User/refresh' operation and returns the [Response].
  /// Parameters:
  ///
  /// * [RefreshRequest] refreshRequest:
  Future<Response> refreshTokenAsyncWithHttpInfo({ RefreshRequest? refreshRequest, }) async {
    // ignore: prefer_const_declarations
    final path = r'/User/refresh';

    // ignore: prefer_final_locals
    Object? postBody = refreshRequest;

    final queryParams = <QueryParam>[];
    final headerParams = <String, String>{};
    final formParams = <String, String>{};

    const contentTypes = <String>['application/json', 'text/json', 'application/*+json'];


    return apiClient.invokeAPI(
      path,
      'POST',
      queryParams,
      postBody,
      headerParams,
      formParams,
      contentTypes.isEmpty ? null : contentTypes.first,
    );
  }

  /// Parameters:
  ///
  /// * [RefreshRequest] refreshRequest:
  Future<SignInResponse?> refreshTokenAsync({ RefreshRequest? refreshRequest, }) async {
    final response = await refreshTokenAsyncWithHttpInfo( refreshRequest: refreshRequest, );
    if (response.statusCode >= HttpStatus.badRequest) {
      throw ApiException(response.statusCode, await _decodeBodyBytes(response));
    }
    // When a remote server returns no body with a status of 204, we shall not decode it.
    // At the time of writing this, `dart:convert` will throw an "Unexpected end of input"
    // FormatException when trying to decode an empty string.
    if (response.body.isNotEmpty && response.statusCode != HttpStatus.noContent) {
      return await apiClient.deserializeAsync(await _decodeBodyBytes(response), 'SignInResponse',) as SignInResponse;
    
    }
    return null;
  }

  /// Performs an HTTP 'POST /User/sign-in' operation and returns the [Response].
  /// Parameters:
  ///
  /// * [SignInRequest] signInRequest:
  Future<Response> signInAsyncWithHttpInfo({ SignInRequest? signInRequest, }) async {
    // ignore: prefer_const_declarations
    final path = r'/User/sign-in';

    // ignore: prefer_final_locals
    Object? postBody = signInRequest;

    final queryParams = <QueryParam>[];
    final headerParams = <String, String>{};
    final formParams = <String, String>{};

    const contentTypes = <String>['application/json', 'text/json', 'application/*+json'];


    return apiClient.invokeAPI(
      path,
      'POST',
      queryParams,
      postBody,
      headerParams,
      formParams,
      contentTypes.isEmpty ? null : contentTypes.first,
    );
  }

  /// Parameters:
  ///
  /// * [SignInRequest] signInRequest:
  Future<SignInResponse?> signInAsync({ SignInRequest? signInRequest, }) async {
    final response = await signInAsyncWithHttpInfo( signInRequest: signInRequest, );
    if (response.statusCode >= HttpStatus.badRequest) {
      throw ApiException(response.statusCode, await _decodeBodyBytes(response));
    }
    // When a remote server returns no body with a status of 204, we shall not decode it.
    // At the time of writing this, `dart:convert` will throw an "Unexpected end of input"
    // FormatException when trying to decode an empty string.
    if (response.body.isNotEmpty && response.statusCode != HttpStatus.noContent) {
      return await apiClient.deserializeAsync(await _decodeBodyBytes(response), 'SignInResponse',) as SignInResponse;
    
    }
    return null;
  }

  /// Performs an HTTP 'POST /User/sign-up' operation and returns the [Response].
  /// Parameters:
  ///
  /// * [SignUpRequest] signUpRequest:
  Future<Response> signUpAsyncWithHttpInfo({ SignUpRequest? signUpRequest, }) async {
    // ignore: prefer_const_declarations
    final path = r'/User/sign-up';

    // ignore: prefer_final_locals
    Object? postBody = signUpRequest;

    final queryParams = <QueryParam>[];
    final headerParams = <String, String>{};
    final formParams = <String, String>{};

    const contentTypes = <String>['application/json', 'text/json', 'application/*+json'];


    return apiClient.invokeAPI(
      path,
      'POST',
      queryParams,
      postBody,
      headerParams,
      formParams,
      contentTypes.isEmpty ? null : contentTypes.first,
    );
  }

  /// Parameters:
  ///
  /// * [SignUpRequest] signUpRequest:
  Future<SignInResponse?> signUpAsync({ SignUpRequest? signUpRequest, }) async {
    final response = await signUpAsyncWithHttpInfo( signUpRequest: signUpRequest, );
    if (response.statusCode >= HttpStatus.badRequest) {
      throw ApiException(response.statusCode, await _decodeBodyBytes(response));
    }
    // When a remote server returns no body with a status of 204, we shall not decode it.
    // At the time of writing this, `dart:convert` will throw an "Unexpected end of input"
    // FormatException when trying to decode an empty string.
    if (response.body.isNotEmpty && response.statusCode != HttpStatus.noContent) {
      return await apiClient.deserializeAsync(await _decodeBodyBytes(response), 'SignInResponse',) as SignInResponse;
    
    }
    return null;
  }
}
