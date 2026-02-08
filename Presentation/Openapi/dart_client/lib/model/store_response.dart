//
// AUTO-GENERATED FILE, DO NOT MODIFY!
//
// @dart=2.18

// ignore_for_file: unused_element, unused_import
// ignore_for_file: always_put_required_named_parameters_first
// ignore_for_file: constant_identifier_names
// ignore_for_file: lines_longer_than_80_chars

part of openapi.api;

class StoreResponse {
  /// Returns a new [StoreResponse] instance.
  StoreResponse({
    required this.storeId,
    required this.storeRegionName,
    required this.storeName,
  });

  String storeId;

  String storeRegionName;

  StoreName storeName;

  @override
  bool operator ==(Object other) => identical(this, other) || other is StoreResponse &&
    other.storeId == storeId &&
    other.storeRegionName == storeRegionName &&
    other.storeName == storeName;

  @override
  int get hashCode =>
    // ignore: unnecessary_parenthesis
    (storeId.hashCode) +
    (storeRegionName.hashCode) +
    (storeName.hashCode);

  @override
  String toString() => 'StoreResponse[storeId=$storeId, storeRegionName=$storeRegionName, storeName=$storeName]';

  Map<String, dynamic> toJson() {
    final json = <String, dynamic>{};
      json[r'storeId'] = this.storeId;
      json[r'storeRegionName'] = this.storeRegionName;
      json[r'storeName'] = this.storeName;
    return json;
  }

  /// Returns a new [StoreResponse] instance and imports its values from
  /// [value] if it's a [Map], null otherwise.
  // ignore: prefer_constructors_over_static_methods
  static StoreResponse? fromJson(dynamic value) {
    if (value is Map) {
      final json = value.cast<String, dynamic>();

      // Ensure that the map contains the required keys.
      // Note 1: the values aren't checked for validity beyond being non-null.
      // Note 2: this code is stripped in release mode!
      assert(() {
        requiredKeys.forEach((key) {
          assert(json.containsKey(key), 'Required key "StoreResponse[$key]" is missing from JSON.');
          assert(json[key] != null, 'Required key "StoreResponse[$key]" has a null value in JSON.');
        });
        return true;
      }());

      return StoreResponse(
        storeId: mapValueOfType<String>(json, r'storeId')!,
        storeRegionName: mapValueOfType<String>(json, r'storeRegionName')!,
        storeName: StoreName.fromJson(json[r'storeName'])!,
      );
    }
    return null;
  }

  static List<StoreResponse> listFromJson(dynamic json, {bool growable = false,}) {
    final result = <StoreResponse>[];
    if (json is List && json.isNotEmpty) {
      for (final row in json) {
        final value = StoreResponse.fromJson(row);
        if (value != null) {
          result.add(value);
        }
      }
    }
    return result.toList(growable: growable);
  }

  static Map<String, StoreResponse> mapFromJson(dynamic json) {
    final map = <String, StoreResponse>{};
    if (json is Map && json.isNotEmpty) {
      json = json.cast<String, dynamic>(); // ignore: parameter_assignments
      for (final entry in json.entries) {
        final value = StoreResponse.fromJson(entry.value);
        if (value != null) {
          map[entry.key] = value;
        }
      }
    }
    return map;
  }

  // maps a json object with a list of StoreResponse-objects as value to a dart map
  static Map<String, List<StoreResponse>> mapListFromJson(dynamic json, {bool growable = false,}) {
    final map = <String, List<StoreResponse>>{};
    if (json is Map && json.isNotEmpty) {
      // ignore: parameter_assignments
      json = json.cast<String, dynamic>();
      for (final entry in json.entries) {
        map[entry.key] = StoreResponse.listFromJson(entry.value, growable: growable,);
      }
    }
    return map;
  }

  /// The list of required keys that must be present in a JSON.
  static const requiredKeys = <String>{
    'storeId',
    'storeRegionName',
    'storeName',
  };
}

