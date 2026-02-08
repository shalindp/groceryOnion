//
// AUTO-GENERATED FILE, DO NOT MODIFY!
//
// @dart=2.18

// ignore_for_file: unused_element, unused_import
// ignore_for_file: always_put_required_named_parameters_first
// ignore_for_file: constant_identifier_names
// ignore_for_file: lines_longer_than_80_chars

part of openapi.api;

class StoreSkuDto {
  /// Returns a new [StoreSkuDto] instance.
  StoreSkuDto({
    this.productId,
    this.storeName,
    this.storeSku,
  });

  ///
  /// Please note: This property should have been non-nullable! Since the specification file
  /// does not include a default value (using the "default:" property), however, the generated
  /// source code must fall back to having a nullable type.
  /// Consider adding a "default:" property in the specification file to hide this note.
  ///
  String? productId;

  ///
  /// Please note: This property should have been non-nullable! Since the specification file
  /// does not include a default value (using the "default:" property), however, the generated
  /// source code must fall back to having a nullable type.
  /// Consider adding a "default:" property in the specification file to hide this note.
  ///
  StoreName? storeName;

  String? storeSku;

  @override
  bool operator ==(Object other) => identical(this, other) || other is StoreSkuDto &&
    other.productId == productId &&
    other.storeName == storeName &&
    other.storeSku == storeSku;

  @override
  int get hashCode =>
    // ignore: unnecessary_parenthesis
    (productId == null ? 0 : productId!.hashCode) +
    (storeName == null ? 0 : storeName!.hashCode) +
    (storeSku == null ? 0 : storeSku!.hashCode);

  @override
  String toString() => 'StoreSkuDto[productId=$productId, storeName=$storeName, storeSku=$storeSku]';

  Map<String, dynamic> toJson() {
    final json = <String, dynamic>{};
    if (this.productId != null) {
      json[r'productId'] = this.productId;
    } else {
      json[r'productId'] = null;
    }
    if (this.storeName != null) {
      json[r'storeName'] = this.storeName;
    } else {
      json[r'storeName'] = null;
    }
    if (this.storeSku != null) {
      json[r'storeSku'] = this.storeSku;
    } else {
      json[r'storeSku'] = null;
    }
    return json;
  }

  /// Returns a new [StoreSkuDto] instance and imports its values from
  /// [value] if it's a [Map], null otherwise.
  // ignore: prefer_constructors_over_static_methods
  static StoreSkuDto? fromJson(dynamic value) {
    if (value is Map) {
      final json = value.cast<String, dynamic>();

      // Ensure that the map contains the required keys.
      // Note 1: the values aren't checked for validity beyond being non-null.
      // Note 2: this code is stripped in release mode!
      assert(() {
        requiredKeys.forEach((key) {
          assert(json.containsKey(key), 'Required key "StoreSkuDto[$key]" is missing from JSON.');
          assert(json[key] != null, 'Required key "StoreSkuDto[$key]" has a null value in JSON.');
        });
        return true;
      }());

      return StoreSkuDto(
        productId: mapValueOfType<String>(json, r'productId'),
        storeName: StoreName.fromJson(json[r'storeName']),
        storeSku: mapValueOfType<String>(json, r'storeSku'),
      );
    }
    return null;
  }

  static List<StoreSkuDto> listFromJson(dynamic json, {bool growable = false,}) {
    final result = <StoreSkuDto>[];
    if (json is List && json.isNotEmpty) {
      for (final row in json) {
        final value = StoreSkuDto.fromJson(row);
        if (value != null) {
          result.add(value);
        }
      }
    }
    return result.toList(growable: growable);
  }

  static Map<String, StoreSkuDto> mapFromJson(dynamic json) {
    final map = <String, StoreSkuDto>{};
    if (json is Map && json.isNotEmpty) {
      json = json.cast<String, dynamic>(); // ignore: parameter_assignments
      for (final entry in json.entries) {
        final value = StoreSkuDto.fromJson(entry.value);
        if (value != null) {
          map[entry.key] = value;
        }
      }
    }
    return map;
  }

  // maps a json object with a list of StoreSkuDto-objects as value to a dart map
  static Map<String, List<StoreSkuDto>> mapListFromJson(dynamic json, {bool growable = false,}) {
    final map = <String, List<StoreSkuDto>>{};
    if (json is Map && json.isNotEmpty) {
      // ignore: parameter_assignments
      json = json.cast<String, dynamic>();
      for (final entry in json.entries) {
        map[entry.key] = StoreSkuDto.listFromJson(entry.value, growable: growable,);
      }
    }
    return map;
  }

  /// The list of required keys that must be present in a JSON.
  static const requiredKeys = <String>{
  };
}

