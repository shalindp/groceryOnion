//
// AUTO-GENERATED FILE, DO NOT MODIFY!
//
// @dart=2.18

// ignore_for_file: unused_element, unused_import
// ignore_for_file: always_put_required_named_parameters_first
// ignore_for_file: constant_identifier_names
// ignore_for_file: lines_longer_than_80_chars

part of openapi.api;

class CreateSessionWithRegionResponse {
  /// Returns a new [CreateSessionWithRegionResponse] instance.
  CreateSessionWithRegionResponse({
    required this.storeName,
    required this.address,
    required this.sessionId,
    required this.aga,
  });

  StoreName storeName;

  String address;

  String sessionId;

  String aga;

  @override
  bool operator ==(Object other) => identical(this, other) || other is CreateSessionWithRegionResponse &&
    other.storeName == storeName &&
    other.address == address &&
    other.sessionId == sessionId &&
    other.aga == aga;

  @override
  int get hashCode =>
    // ignore: unnecessary_parenthesis
    (storeName.hashCode) +
    (address.hashCode) +
    (sessionId.hashCode) +
    (aga.hashCode);

  @override
  String toString() => 'CreateSessionWithRegionResponse[storeName=$storeName, address=$address, sessionId=$sessionId, aga=$aga]';

  Map<String, dynamic> toJson() {
    final json = <String, dynamic>{};
      json[r'storeName'] = this.storeName;
      json[r'address'] = this.address;
      json[r'sessionId'] = this.sessionId;
      json[r'aga'] = this.aga;
    return json;
  }

  /// Returns a new [CreateSessionWithRegionResponse] instance and imports its values from
  /// [value] if it's a [Map], null otherwise.
  // ignore: prefer_constructors_over_static_methods
  static CreateSessionWithRegionResponse? fromJson(dynamic value) {
    if (value is Map) {
      final json = value.cast<String, dynamic>();

      // Ensure that the map contains the required keys.
      // Note 1: the values aren't checked for validity beyond being non-null.
      // Note 2: this code is stripped in release mode!
      assert(() {
        requiredKeys.forEach((key) {
          assert(json.containsKey(key), 'Required key "CreateSessionWithRegionResponse[$key]" is missing from JSON.');
          assert(json[key] != null, 'Required key "CreateSessionWithRegionResponse[$key]" has a null value in JSON.');
        });
        return true;
      }());

      return CreateSessionWithRegionResponse(
        storeName: StoreName.fromJson(json[r'storeName'])!,
        address: mapValueOfType<String>(json, r'address')!,
        sessionId: mapValueOfType<String>(json, r'sessionId')!,
        aga: mapValueOfType<String>(json, r'aga')!,
      );
    }
    return null;
  }

  static List<CreateSessionWithRegionResponse> listFromJson(dynamic json, {bool growable = false,}) {
    final result = <CreateSessionWithRegionResponse>[];
    if (json is List && json.isNotEmpty) {
      for (final row in json) {
        final value = CreateSessionWithRegionResponse.fromJson(row);
        if (value != null) {
          result.add(value);
        }
      }
    }
    return result.toList(growable: growable);
  }

  static Map<String, CreateSessionWithRegionResponse> mapFromJson(dynamic json) {
    final map = <String, CreateSessionWithRegionResponse>{};
    if (json is Map && json.isNotEmpty) {
      json = json.cast<String, dynamic>(); // ignore: parameter_assignments
      for (final entry in json.entries) {
        final value = CreateSessionWithRegionResponse.fromJson(entry.value);
        if (value != null) {
          map[entry.key] = value;
        }
      }
    }
    return map;
  }

  // maps a json object with a list of CreateSessionWithRegionResponse-objects as value to a dart map
  static Map<String, List<CreateSessionWithRegionResponse>> mapListFromJson(dynamic json, {bool growable = false,}) {
    final map = <String, List<CreateSessionWithRegionResponse>>{};
    if (json is Map && json.isNotEmpty) {
      // ignore: parameter_assignments
      json = json.cast<String, dynamic>();
      for (final entry in json.entries) {
        map[entry.key] = CreateSessionWithRegionResponse.listFromJson(entry.value, growable: growable,);
      }
    }
    return map;
  }

  /// The list of required keys that must be present in a JSON.
  static const requiredKeys = <String>{
    'storeName',
    'address',
    'sessionId',
    'aga',
  };
}

