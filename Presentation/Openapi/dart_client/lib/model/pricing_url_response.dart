//
// AUTO-GENERATED FILE, DO NOT MODIFY!
//
// @dart=2.18

// ignore_for_file: unused_element, unused_import
// ignore_for_file: always_put_required_named_parameters_first
// ignore_for_file: constant_identifier_names
// ignore_for_file: lines_longer_than_80_chars

part of openapi.api;

class PricingUrlResponse {
  /// Returns a new [PricingUrlResponse] instance.
  PricingUrlResponse({
    this.storeName,
    this.sku,
    this.pricingUrl,
  });

  ///
  /// Please note: This property should have been non-nullable! Since the specification file
  /// does not include a default value (using the "default:" property), however, the generated
  /// source code must fall back to having a nullable type.
  /// Consider adding a "default:" property in the specification file to hide this note.
  ///
  StoreName? storeName;

  String? sku;

  String? pricingUrl;

  @override
  bool operator ==(Object other) => identical(this, other) || other is PricingUrlResponse &&
    other.storeName == storeName &&
    other.sku == sku &&
    other.pricingUrl == pricingUrl;

  @override
  int get hashCode =>
    // ignore: unnecessary_parenthesis
    (storeName == null ? 0 : storeName!.hashCode) +
    (sku == null ? 0 : sku!.hashCode) +
    (pricingUrl == null ? 0 : pricingUrl!.hashCode);

  @override
  String toString() => 'PricingUrlResponse[storeName=$storeName, sku=$sku, pricingUrl=$pricingUrl]';

  Map<String, dynamic> toJson() {
    final json = <String, dynamic>{};
    if (this.storeName != null) {
      json[r'storeName'] = this.storeName;
    } else {
      json[r'storeName'] = null;
    }
    if (this.sku != null) {
      json[r'sku'] = this.sku;
    } else {
      json[r'sku'] = null;
    }
    if (this.pricingUrl != null) {
      json[r'pricingUrl'] = this.pricingUrl;
    } else {
      json[r'pricingUrl'] = null;
    }
    return json;
  }

  /// Returns a new [PricingUrlResponse] instance and imports its values from
  /// [value] if it's a [Map], null otherwise.
  // ignore: prefer_constructors_over_static_methods
  static PricingUrlResponse? fromJson(dynamic value) {
    if (value is Map) {
      final json = value.cast<String, dynamic>();

      // Ensure that the map contains the required keys.
      // Note 1: the values aren't checked for validity beyond being non-null.
      // Note 2: this code is stripped in release mode!
      assert(() {
        requiredKeys.forEach((key) {
          assert(json.containsKey(key), 'Required key "PricingUrlResponse[$key]" is missing from JSON.');
          assert(json[key] != null, 'Required key "PricingUrlResponse[$key]" has a null value in JSON.');
        });
        return true;
      }());

      return PricingUrlResponse(
        storeName: StoreName.fromJson(json[r'storeName']),
        sku: mapValueOfType<String>(json, r'sku'),
        pricingUrl: mapValueOfType<String>(json, r'pricingUrl'),
      );
    }
    return null;
  }

  static List<PricingUrlResponse> listFromJson(dynamic json, {bool growable = false,}) {
    final result = <PricingUrlResponse>[];
    if (json is List && json.isNotEmpty) {
      for (final row in json) {
        final value = PricingUrlResponse.fromJson(row);
        if (value != null) {
          result.add(value);
        }
      }
    }
    return result.toList(growable: growable);
  }

  static Map<String, PricingUrlResponse> mapFromJson(dynamic json) {
    final map = <String, PricingUrlResponse>{};
    if (json is Map && json.isNotEmpty) {
      json = json.cast<String, dynamic>(); // ignore: parameter_assignments
      for (final entry in json.entries) {
        final value = PricingUrlResponse.fromJson(entry.value);
        if (value != null) {
          map[entry.key] = value;
        }
      }
    }
    return map;
  }

  // maps a json object with a list of PricingUrlResponse-objects as value to a dart map
  static Map<String, List<PricingUrlResponse>> mapListFromJson(dynamic json, {bool growable = false,}) {
    final map = <String, List<PricingUrlResponse>>{};
    if (json is Map && json.isNotEmpty) {
      // ignore: parameter_assignments
      json = json.cast<String, dynamic>();
      for (final entry in json.entries) {
        map[entry.key] = PricingUrlResponse.listFromJson(entry.value, growable: growable,);
      }
    }
    return map;
  }

  /// The list of required keys that must be present in a JSON.
  static const requiredKeys = <String>{
  };
}

