//
// AUTO-GENERATED FILE, DO NOT MODIFY!
//
// @dart=2.18

// ignore_for_file: unused_element, unused_import
// ignore_for_file: always_put_required_named_parameters_first
// ignore_for_file: constant_identifier_names
// ignore_for_file: lines_longer_than_80_chars

part of openapi.api;

class WoolworthsChangeRegionResult {
  /// Returns a new [WoolworthsChangeRegionResult] instance.
  WoolworthsChangeRegionResult({
    this.address,
    this.sessionId,
    this.aga,
  });

  String? address;

  String? sessionId;

  String? aga;

  @override
  bool operator ==(Object other) => identical(this, other) || other is WoolworthsChangeRegionResult &&
    other.address == address &&
    other.sessionId == sessionId &&
    other.aga == aga;

  @override
  int get hashCode =>
    // ignore: unnecessary_parenthesis
    (address == null ? 0 : address!.hashCode) +
    (sessionId == null ? 0 : sessionId!.hashCode) +
    (aga == null ? 0 : aga!.hashCode);

  @override
  String toString() => 'WoolworthsChangeRegionResult[address=$address, sessionId=$sessionId, aga=$aga]';

  Map<String, dynamic> toJson() {
    final json = <String, dynamic>{};
    if (this.address != null) {
      json[r'address'] = this.address;
    } else {
      json[r'address'] = null;
    }
    if (this.sessionId != null) {
      json[r'sessionId'] = this.sessionId;
    } else {
      json[r'sessionId'] = null;
    }
    if (this.aga != null) {
      json[r'aga'] = this.aga;
    } else {
      json[r'aga'] = null;
    }
    return json;
  }

  /// Returns a new [WoolworthsChangeRegionResult] instance and imports its values from
  /// [value] if it's a [Map], null otherwise.
  // ignore: prefer_constructors_over_static_methods
  static WoolworthsChangeRegionResult? fromJson(dynamic value) {
    if (value is Map) {
      final json = value.cast<String, dynamic>();

      // Ensure that the map contains the required keys.
      // Note 1: the values aren't checked for validity beyond being non-null.
      // Note 2: this code is stripped in release mode!
      assert(() {
        requiredKeys.forEach((key) {
          assert(json.containsKey(key), 'Required key "WoolworthsChangeRegionResult[$key]" is missing from JSON.');
          assert(json[key] != null, 'Required key "WoolworthsChangeRegionResult[$key]" has a null value in JSON.');
        });
        return true;
      }());

      return WoolworthsChangeRegionResult(
        address: mapValueOfType<String>(json, r'address'),
        sessionId: mapValueOfType<String>(json, r'sessionId'),
        aga: mapValueOfType<String>(json, r'aga'),
      );
    }
    return null;
  }

  static List<WoolworthsChangeRegionResult> listFromJson(dynamic json, {bool growable = false,}) {
    final result = <WoolworthsChangeRegionResult>[];
    if (json is List && json.isNotEmpty) {
      for (final row in json) {
        final value = WoolworthsChangeRegionResult.fromJson(row);
        if (value != null) {
          result.add(value);
        }
      }
    }
    return result.toList(growable: growable);
  }

  static Map<String, WoolworthsChangeRegionResult> mapFromJson(dynamic json) {
    final map = <String, WoolworthsChangeRegionResult>{};
    if (json is Map && json.isNotEmpty) {
      json = json.cast<String, dynamic>(); // ignore: parameter_assignments
      for (final entry in json.entries) {
        final value = WoolworthsChangeRegionResult.fromJson(entry.value);
        if (value != null) {
          map[entry.key] = value;
        }
      }
    }
    return map;
  }

  // maps a json object with a list of WoolworthsChangeRegionResult-objects as value to a dart map
  static Map<String, List<WoolworthsChangeRegionResult>> mapListFromJson(dynamic json, {bool growable = false,}) {
    final map = <String, List<WoolworthsChangeRegionResult>>{};
    if (json is Map && json.isNotEmpty) {
      // ignore: parameter_assignments
      json = json.cast<String, dynamic>();
      for (final entry in json.entries) {
        map[entry.key] = WoolworthsChangeRegionResult.listFromJson(entry.value, growable: growable,);
      }
    }
    return map;
  }

  /// The list of required keys that must be present in a JSON.
  static const requiredKeys = <String>{
  };
}

