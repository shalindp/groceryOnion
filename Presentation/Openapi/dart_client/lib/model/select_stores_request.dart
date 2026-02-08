//
// AUTO-GENERATED FILE, DO NOT MODIFY!
//
// @dart=2.18

// ignore_for_file: unused_element, unused_import
// ignore_for_file: always_put_required_named_parameters_first
// ignore_for_file: constant_identifier_names
// ignore_for_file: lines_longer_than_80_chars

part of openapi.api;

class SelectStoresRequest {
  /// Returns a new [SelectStoresRequest] instance.
  SelectStoresRequest({
    this.woolworthStoreIds = const [],
    this.paknSaveStoreIds = const [],
  });

  List<String>? woolworthStoreIds;

  List<String>? paknSaveStoreIds;

  @override
  bool operator ==(Object other) => identical(this, other) || other is SelectStoresRequest &&
    _deepEquality.equals(other.woolworthStoreIds, woolworthStoreIds) &&
    _deepEquality.equals(other.paknSaveStoreIds, paknSaveStoreIds);

  @override
  int get hashCode =>
    // ignore: unnecessary_parenthesis
    (woolworthStoreIds == null ? 0 : woolworthStoreIds!.hashCode) +
    (paknSaveStoreIds == null ? 0 : paknSaveStoreIds!.hashCode);

  @override
  String toString() => 'SelectStoresRequest[woolworthStoreIds=$woolworthStoreIds, paknSaveStoreIds=$paknSaveStoreIds]';

  Map<String, dynamic> toJson() {
    final json = <String, dynamic>{};
    if (this.woolworthStoreIds != null) {
      json[r'woolworthStoreIds'] = this.woolworthStoreIds;
    } else {
      json[r'woolworthStoreIds'] = null;
    }
    if (this.paknSaveStoreIds != null) {
      json[r'paknSaveStoreIds'] = this.paknSaveStoreIds;
    } else {
      json[r'paknSaveStoreIds'] = null;
    }
    return json;
  }

  /// Returns a new [SelectStoresRequest] instance and imports its values from
  /// [value] if it's a [Map], null otherwise.
  // ignore: prefer_constructors_over_static_methods
  static SelectStoresRequest? fromJson(dynamic value) {
    if (value is Map) {
      final json = value.cast<String, dynamic>();

      // Ensure that the map contains the required keys.
      // Note 1: the values aren't checked for validity beyond being non-null.
      // Note 2: this code is stripped in release mode!
      assert(() {
        requiredKeys.forEach((key) {
          assert(json.containsKey(key), 'Required key "SelectStoresRequest[$key]" is missing from JSON.');
          assert(json[key] != null, 'Required key "SelectStoresRequest[$key]" has a null value in JSON.');
        });
        return true;
      }());

      return SelectStoresRequest(
        woolworthStoreIds: json[r'woolworthStoreIds'] is Iterable
            ? (json[r'woolworthStoreIds'] as Iterable).cast<String>().toList(growable: false)
            : const [],
        paknSaveStoreIds: json[r'paknSaveStoreIds'] is Iterable
            ? (json[r'paknSaveStoreIds'] as Iterable).cast<String>().toList(growable: false)
            : const [],
      );
    }
    return null;
  }

  static List<SelectStoresRequest> listFromJson(dynamic json, {bool growable = false,}) {
    final result = <SelectStoresRequest>[];
    if (json is List && json.isNotEmpty) {
      for (final row in json) {
        final value = SelectStoresRequest.fromJson(row);
        if (value != null) {
          result.add(value);
        }
      }
    }
    return result.toList(growable: growable);
  }

  static Map<String, SelectStoresRequest> mapFromJson(dynamic json) {
    final map = <String, SelectStoresRequest>{};
    if (json is Map && json.isNotEmpty) {
      json = json.cast<String, dynamic>(); // ignore: parameter_assignments
      for (final entry in json.entries) {
        final value = SelectStoresRequest.fromJson(entry.value);
        if (value != null) {
          map[entry.key] = value;
        }
      }
    }
    return map;
  }

  // maps a json object with a list of SelectStoresRequest-objects as value to a dart map
  static Map<String, List<SelectStoresRequest>> mapListFromJson(dynamic json, {bool growable = false,}) {
    final map = <String, List<SelectStoresRequest>>{};
    if (json is Map && json.isNotEmpty) {
      // ignore: parameter_assignments
      json = json.cast<String, dynamic>();
      for (final entry in json.entries) {
        map[entry.key] = SelectStoresRequest.listFromJson(entry.value, growable: growable,);
      }
    }
    return map;
  }

  /// The list of required keys that must be present in a JSON.
  static const requiredKeys = <String>{
  };
}

