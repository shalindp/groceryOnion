//
// AUTO-GENERATED FILE, DO NOT MODIFY!
//
// @dart=2.18

// ignore_for_file: unused_element, unused_import
// ignore_for_file: always_put_required_named_parameters_first
// ignore_for_file: constant_identifier_names
// ignore_for_file: lines_longer_than_80_chars

part of openapi.api;

class CreateSessionWithRegionId {
  /// Returns a new [CreateSessionWithRegionId] instance.
  CreateSessionWithRegionId({
    this.storeName,
    this.regionId,
  });

  ///
  /// Please note: This property should have been non-nullable! Since the specification file
  /// does not include a default value (using the "default:" property), however, the generated
  /// source code must fall back to having a nullable type.
  /// Consider adding a "default:" property in the specification file to hide this note.
  ///
  StoreName? storeName;

  ///
  /// Please note: This property should have been non-nullable! Since the specification file
  /// does not include a default value (using the "default:" property), however, the generated
  /// source code must fall back to having a nullable type.
  /// Consider adding a "default:" property in the specification file to hide this note.
  ///
  int? regionId;

  @override
  bool operator ==(Object other) => identical(this, other) || other is CreateSessionWithRegionId &&
    other.storeName == storeName &&
    other.regionId == regionId;

  @override
  int get hashCode =>
    // ignore: unnecessary_parenthesis
    (storeName == null ? 0 : storeName!.hashCode) +
    (regionId == null ? 0 : regionId!.hashCode);

  @override
  String toString() => 'CreateSessionWithRegionId[storeName=$storeName, regionId=$regionId]';

  Map<String, dynamic> toJson() {
    final json = <String, dynamic>{};
    if (this.storeName != null) {
      json[r'storeName'] = this.storeName;
    } else {
      json[r'storeName'] = null;
    }
    if (this.regionId != null) {
      json[r'regionId'] = this.regionId;
    } else {
      json[r'regionId'] = null;
    }
    return json;
  }

  /// Returns a new [CreateSessionWithRegionId] instance and imports its values from
  /// [value] if it's a [Map], null otherwise.
  // ignore: prefer_constructors_over_static_methods
  static CreateSessionWithRegionId? fromJson(dynamic value) {
    if (value is Map) {
      final json = value.cast<String, dynamic>();

      // Ensure that the map contains the required keys.
      // Note 1: the values aren't checked for validity beyond being non-null.
      // Note 2: this code is stripped in release mode!
      assert(() {
        requiredKeys.forEach((key) {
          assert(json.containsKey(key), 'Required key "CreateSessionWithRegionId[$key]" is missing from JSON.');
          assert(json[key] != null, 'Required key "CreateSessionWithRegionId[$key]" has a null value in JSON.');
        });
        return true;
      }());

      return CreateSessionWithRegionId(
        storeName: StoreName.fromJson(json[r'storeName']),
        regionId: mapValueOfType<int>(json, r'regionId'),
      );
    }
    return null;
  }

  static List<CreateSessionWithRegionId> listFromJson(dynamic json, {bool growable = false,}) {
    final result = <CreateSessionWithRegionId>[];
    if (json is List && json.isNotEmpty) {
      for (final row in json) {
        final value = CreateSessionWithRegionId.fromJson(row);
        if (value != null) {
          result.add(value);
        }
      }
    }
    return result.toList(growable: growable);
  }

  static Map<String, CreateSessionWithRegionId> mapFromJson(dynamic json) {
    final map = <String, CreateSessionWithRegionId>{};
    if (json is Map && json.isNotEmpty) {
      json = json.cast<String, dynamic>(); // ignore: parameter_assignments
      for (final entry in json.entries) {
        final value = CreateSessionWithRegionId.fromJson(entry.value);
        if (value != null) {
          map[entry.key] = value;
        }
      }
    }
    return map;
  }

  // maps a json object with a list of CreateSessionWithRegionId-objects as value to a dart map
  static Map<String, List<CreateSessionWithRegionId>> mapListFromJson(dynamic json, {bool growable = false,}) {
    final map = <String, List<CreateSessionWithRegionId>>{};
    if (json is Map && json.isNotEmpty) {
      // ignore: parameter_assignments
      json = json.cast<String, dynamic>();
      for (final entry in json.entries) {
        map[entry.key] = CreateSessionWithRegionId.listFromJson(entry.value, growable: growable,);
      }
    }
    return map;
  }

  /// The list of required keys that must be present in a JSON.
  static const requiredKeys = <String>{
  };
}

