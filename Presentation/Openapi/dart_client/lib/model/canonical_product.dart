//
// AUTO-GENERATED FILE, DO NOT MODIFY!
//
// @dart=2.18

// ignore_for_file: unused_element, unused_import
// ignore_for_file: always_put_required_named_parameters_first
// ignore_for_file: constant_identifier_names
// ignore_for_file: lines_longer_than_80_chars

part of openapi.api;

class CanonicalProduct {
  /// Returns a new [CanonicalProduct] instance.
  CanonicalProduct({
    this.canonicalProductId,
    this.barcode,
    this.brand,
    this.name,
    this.imageUrl,
    this.unitAndSize,
    this.maxQuantity,
    this.isDeleted,
    this.createdUtc,
    this.lastUpdatedUtc,
  });

  ///
  /// Please note: This property should have been non-nullable! Since the specification file
  /// does not include a default value (using the "default:" property), however, the generated
  /// source code must fall back to having a nullable type.
  /// Consider adding a "default:" property in the specification file to hide this note.
  ///
  String? canonicalProductId;

  String? barcode;

  String? brand;

  String? name;

  String? imageUrl;

  String? unitAndSize;

  double? maxQuantity;

  ///
  /// Please note: This property should have been non-nullable! Since the specification file
  /// does not include a default value (using the "default:" property), however, the generated
  /// source code must fall back to having a nullable type.
  /// Consider adding a "default:" property in the specification file to hide this note.
  ///
  bool? isDeleted;

  ///
  /// Please note: This property should have been non-nullable! Since the specification file
  /// does not include a default value (using the "default:" property), however, the generated
  /// source code must fall back to having a nullable type.
  /// Consider adding a "default:" property in the specification file to hide this note.
  ///
  DateTime? createdUtc;

  ///
  /// Please note: This property should have been non-nullable! Since the specification file
  /// does not include a default value (using the "default:" property), however, the generated
  /// source code must fall back to having a nullable type.
  /// Consider adding a "default:" property in the specification file to hide this note.
  ///
  DateTime? lastUpdatedUtc;

  @override
  bool operator ==(Object other) => identical(this, other) || other is CanonicalProduct &&
    other.canonicalProductId == canonicalProductId &&
    other.barcode == barcode &&
    other.brand == brand &&
    other.name == name &&
    other.imageUrl == imageUrl &&
    other.unitAndSize == unitAndSize &&
    other.maxQuantity == maxQuantity &&
    other.isDeleted == isDeleted &&
    other.createdUtc == createdUtc &&
    other.lastUpdatedUtc == lastUpdatedUtc;

  @override
  int get hashCode =>
    // ignore: unnecessary_parenthesis
    (canonicalProductId == null ? 0 : canonicalProductId!.hashCode) +
    (barcode == null ? 0 : barcode!.hashCode) +
    (brand == null ? 0 : brand!.hashCode) +
    (name == null ? 0 : name!.hashCode) +
    (imageUrl == null ? 0 : imageUrl!.hashCode) +
    (unitAndSize == null ? 0 : unitAndSize!.hashCode) +
    (maxQuantity == null ? 0 : maxQuantity!.hashCode) +
    (isDeleted == null ? 0 : isDeleted!.hashCode) +
    (createdUtc == null ? 0 : createdUtc!.hashCode) +
    (lastUpdatedUtc == null ? 0 : lastUpdatedUtc!.hashCode);

  @override
  String toString() => 'CanonicalProduct[canonicalProductId=$canonicalProductId, barcode=$barcode, brand=$brand, name=$name, imageUrl=$imageUrl, unitAndSize=$unitAndSize, maxQuantity=$maxQuantity, isDeleted=$isDeleted, createdUtc=$createdUtc, lastUpdatedUtc=$lastUpdatedUtc]';

  Map<String, dynamic> toJson() {
    final json = <String, dynamic>{};
    if (this.canonicalProductId != null) {
      json[r'canonicalProductId'] = this.canonicalProductId;
    } else {
      json[r'canonicalProductId'] = null;
    }
    if (this.barcode != null) {
      json[r'barcode'] = this.barcode;
    } else {
      json[r'barcode'] = null;
    }
    if (this.brand != null) {
      json[r'brand'] = this.brand;
    } else {
      json[r'brand'] = null;
    }
    if (this.name != null) {
      json[r'name'] = this.name;
    } else {
      json[r'name'] = null;
    }
    if (this.imageUrl != null) {
      json[r'imageUrl'] = this.imageUrl;
    } else {
      json[r'imageUrl'] = null;
    }
    if (this.unitAndSize != null) {
      json[r'unitAndSize'] = this.unitAndSize;
    } else {
      json[r'unitAndSize'] = null;
    }
    if (this.maxQuantity != null) {
      json[r'maxQuantity'] = this.maxQuantity;
    } else {
      json[r'maxQuantity'] = null;
    }
    if (this.isDeleted != null) {
      json[r'isDeleted'] = this.isDeleted;
    } else {
      json[r'isDeleted'] = null;
    }
    if (this.createdUtc != null) {
      json[r'createdUtc'] = this.createdUtc!.toUtc().toIso8601String();
    } else {
      json[r'createdUtc'] = null;
    }
    if (this.lastUpdatedUtc != null) {
      json[r'lastUpdatedUtc'] = this.lastUpdatedUtc!.toUtc().toIso8601String();
    } else {
      json[r'lastUpdatedUtc'] = null;
    }
    return json;
  }

  /// Returns a new [CanonicalProduct] instance and imports its values from
  /// [value] if it's a [Map], null otherwise.
  // ignore: prefer_constructors_over_static_methods
  static CanonicalProduct? fromJson(dynamic value) {
    if (value is Map) {
      final json = value.cast<String, dynamic>();

      // Ensure that the map contains the required keys.
      // Note 1: the values aren't checked for validity beyond being non-null.
      // Note 2: this code is stripped in release mode!
      assert(() {
        requiredKeys.forEach((key) {
          assert(json.containsKey(key), 'Required key "CanonicalProduct[$key]" is missing from JSON.');
          assert(json[key] != null, 'Required key "CanonicalProduct[$key]" has a null value in JSON.');
        });
        return true;
      }());

      return CanonicalProduct(
        canonicalProductId: mapValueOfType<String>(json, r'canonicalProductId'),
        barcode: mapValueOfType<String>(json, r'barcode'),
        brand: mapValueOfType<String>(json, r'brand'),
        name: mapValueOfType<String>(json, r'name'),
        imageUrl: mapValueOfType<String>(json, r'imageUrl'),
        unitAndSize: mapValueOfType<String>(json, r'unitAndSize'),
        maxQuantity: mapValueOfType<double>(json, r'maxQuantity'),
        isDeleted: mapValueOfType<bool>(json, r'isDeleted'),
        createdUtc: mapDateTime(json, r'createdUtc', r''),
        lastUpdatedUtc: mapDateTime(json, r'lastUpdatedUtc', r''),
      );
    }
    return null;
  }

  static List<CanonicalProduct> listFromJson(dynamic json, {bool growable = false,}) {
    final result = <CanonicalProduct>[];
    if (json is List && json.isNotEmpty) {
      for (final row in json) {
        final value = CanonicalProduct.fromJson(row);
        if (value != null) {
          result.add(value);
        }
      }
    }
    return result.toList(growable: growable);
  }

  static Map<String, CanonicalProduct> mapFromJson(dynamic json) {
    final map = <String, CanonicalProduct>{};
    if (json is Map && json.isNotEmpty) {
      json = json.cast<String, dynamic>(); // ignore: parameter_assignments
      for (final entry in json.entries) {
        final value = CanonicalProduct.fromJson(entry.value);
        if (value != null) {
          map[entry.key] = value;
        }
      }
    }
    return map;
  }

  // maps a json object with a list of CanonicalProduct-objects as value to a dart map
  static Map<String, List<CanonicalProduct>> mapListFromJson(dynamic json, {bool growable = false,}) {
    final map = <String, List<CanonicalProduct>>{};
    if (json is Map && json.isNotEmpty) {
      // ignore: parameter_assignments
      json = json.cast<String, dynamic>();
      for (final entry in json.entries) {
        map[entry.key] = CanonicalProduct.listFromJson(entry.value, growable: growable,);
      }
    }
    return map;
  }

  /// The list of required keys that must be present in a JSON.
  static const requiredKeys = <String>{
  };
}

