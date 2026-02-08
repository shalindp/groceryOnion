//
// AUTO-GENERATED FILE, DO NOT MODIFY!
//
// @dart=2.18

// ignore_for_file: unused_element, unused_import
// ignore_for_file: always_put_required_named_parameters_first
// ignore_for_file: constant_identifier_names
// ignore_for_file: lines_longer_than_80_chars

part of openapi.api;

class SignInResponse {
  /// Returns a new [SignInResponse] instance.
  SignInResponse({
    required this.username,
    required this.token,
    required this.refreshToken,
    required this.tokenExpirationInSeconds,
  });

  String username;

  String token;

  String refreshToken;

  int tokenExpirationInSeconds;

  @override
  bool operator ==(Object other) => identical(this, other) || other is SignInResponse &&
    other.username == username &&
    other.token == token &&
    other.refreshToken == refreshToken &&
    other.tokenExpirationInSeconds == tokenExpirationInSeconds;

  @override
  int get hashCode =>
    // ignore: unnecessary_parenthesis
    (username.hashCode) +
    (token.hashCode) +
    (refreshToken.hashCode) +
    (tokenExpirationInSeconds.hashCode);

  @override
  String toString() => 'SignInResponse[username=$username, token=$token, refreshToken=$refreshToken, tokenExpirationInSeconds=$tokenExpirationInSeconds]';

  Map<String, dynamic> toJson() {
    final json = <String, dynamic>{};
      json[r'username'] = this.username;
      json[r'token'] = this.token;
      json[r'refreshToken'] = this.refreshToken;
      json[r'tokenExpirationInSeconds'] = this.tokenExpirationInSeconds;
    return json;
  }

  /// Returns a new [SignInResponse] instance and imports its values from
  /// [value] if it's a [Map], null otherwise.
  // ignore: prefer_constructors_over_static_methods
  static SignInResponse? fromJson(dynamic value) {
    if (value is Map) {
      final json = value.cast<String, dynamic>();

      // Ensure that the map contains the required keys.
      // Note 1: the values aren't checked for validity beyond being non-null.
      // Note 2: this code is stripped in release mode!
      assert(() {
        requiredKeys.forEach((key) {
          assert(json.containsKey(key), 'Required key "SignInResponse[$key]" is missing from JSON.');
          assert(json[key] != null, 'Required key "SignInResponse[$key]" has a null value in JSON.');
        });
        return true;
      }());

      return SignInResponse(
        username: mapValueOfType<String>(json, r'username')!,
        token: mapValueOfType<String>(json, r'token')!,
        refreshToken: mapValueOfType<String>(json, r'refreshToken')!,
        tokenExpirationInSeconds: mapValueOfType<int>(json, r'tokenExpirationInSeconds')!,
      );
    }
    return null;
  }

  static List<SignInResponse> listFromJson(dynamic json, {bool growable = false,}) {
    final result = <SignInResponse>[];
    if (json is List && json.isNotEmpty) {
      for (final row in json) {
        final value = SignInResponse.fromJson(row);
        if (value != null) {
          result.add(value);
        }
      }
    }
    return result.toList(growable: growable);
  }

  static Map<String, SignInResponse> mapFromJson(dynamic json) {
    final map = <String, SignInResponse>{};
    if (json is Map && json.isNotEmpty) {
      json = json.cast<String, dynamic>(); // ignore: parameter_assignments
      for (final entry in json.entries) {
        final value = SignInResponse.fromJson(entry.value);
        if (value != null) {
          map[entry.key] = value;
        }
      }
    }
    return map;
  }

  // maps a json object with a list of SignInResponse-objects as value to a dart map
  static Map<String, List<SignInResponse>> mapListFromJson(dynamic json, {bool growable = false,}) {
    final map = <String, List<SignInResponse>>{};
    if (json is Map && json.isNotEmpty) {
      // ignore: parameter_assignments
      json = json.cast<String, dynamic>();
      for (final entry in json.entries) {
        map[entry.key] = SignInResponse.listFromJson(entry.value, growable: growable,);
      }
    }
    return map;
  }

  /// The list of required keys that must be present in a JSON.
  static const requiredKeys = <String>{
    'username',
    'token',
    'refreshToken',
    'tokenExpirationInSeconds',
  };
}

