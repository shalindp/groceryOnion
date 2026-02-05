//
// AUTO-GENERATED FILE, DO NOT MODIFY!
//
// @dart=2.18

// ignore_for_file: unused_element, unused_import
// ignore_for_file: always_put_required_named_parameters_first
// ignore_for_file: constant_identifier_names
// ignore_for_file: lines_longer_than_80_chars

part of openapi.api;

class Product {
  /// Returns a new [Product] instance.
  Product({
    this.id,
    this.sku,
    this.name,
    this.brand,
    this.storeType,
    this.imageUrl,
    this.maxQuantity,
    this.isDeleted,
    this.createdUtc,
    this.lastUpdatedUtc,
    this.searchVector = const [],
  });

  ///
  /// Please note: This property should have been non-nullable! Since the specification file
  /// does not include a default value (using the "default:" property), however, the generated
  /// source code must fall back to having a nullable type.
  /// Consider adding a "default:" property in the specification file to hide this note.
  ///
  String? id;

  String? sku;

  String? name;

  String? brand;

  ///
  /// Please note: This property should have been non-nullable! Since the specification file
  /// does not include a default value (using the "default:" property), however, the generated
  /// source code must fall back to having a nullable type.
  /// Consider adding a "default:" property in the specification file to hide this note.
  ///
  int? storeType;

  String? imageUrl;

  ///
  /// Please note: This property should have been non-nullable! Since the specification file
  /// does not include a default value (using the "default:" property), however, the generated
  /// source code must fall back to having a nullable type.
  /// Consider adding a "default:" property in the specification file to hide this note.
  ///
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

  List<Lexeme>? searchVector;

  @override
  bool operator ==(Object other) => identical(this, other) || other is Product &&
    other.id == id &&
    other.sku == sku &&
    other.name == name &&
    other.brand == brand &&
    other.storeType == storeType &&
    other.imageUrl == imageUrl &&
    other.maxQuantity == maxQuantity &&
    other.isDeleted == isDeleted &&
    other.createdUtc == createdUtc &&
    other.lastUpdatedUtc == lastUpdatedUtc &&
    _deepEquality.equals(other.searchVector, searchVector);

  @override
  int get hashCode =>
    // ignore: unnecessary_parenthesis
    (id == null ? 0 : id!.hashCode) +
    (sku == null ? 0 : sku!.hashCode) +
    (name == null ? 0 : name!.hashCode) +
    (brand == null ? 0 : brand!.hashCode) +
    (storeType == null ? 0 : storeType!.hashCode) +
    (imageUrl == null ? 0 : imageUrl!.hashCode) +
    (maxQuantity == null ? 0 : maxQuantity!.hashCode) +
    (isDeleted == null ? 0 : isDeleted!.hashCode) +
    (createdUtc == null ? 0 : createdUtc!.hashCode) +
    (lastUpdatedUtc == null ? 0 : lastUpdatedUtc!.hashCode) +
    (searchVector == null ? 0 : searchVector!.hashCode);

  @override
  String toString() => 'Product[id=$id, sku=$sku, name=$name, brand=$brand, storeType=$storeType, imageUrl=$imageUrl, maxQuantity=$maxQuantity, isDeleted=$isDeleted, createdUtc=$createdUtc, lastUpdatedUtc=$lastUpdatedUtc, searchVector=$searchVector]';

  Map<String, dynamic> toJson() {
    final json = <String, dynamic>{};
    if (this.id != null) {
      json[r'id'] = this.id;
    } else {
      json[r'id'] = null;
    }
    if (this.sku != null) {
      json[r'sku'] = this.sku;
    } else {
      json[r'sku'] = null;
    }
    if (this.name != null) {
      json[r'name'] = this.name;
    } else {
      json[r'name'] = null;
    }
    if (this.brand != null) {
      json[r'brand'] = this.brand;
    } else {
      json[r'brand'] = null;
    }
    if (this.storeType != null) {
      json[r'storeType'] = this.storeType;
    } else {
      json[r'storeType'] = null;
    }
    if (this.imageUrl != null) {
      json[r'imageUrl'] = this.imageUrl;
    } else {
      json[r'imageUrl'] = null;
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
    if (this.searchVector != null) {
      json[r'searchVector'] = this.searchVector;
    } else {
      json[r'searchVector'] = null;
    }
    return json;
  }

  /// Returns a new [Product] instance and imports its values from
  /// [value] if it's a [Map], null otherwise.
  // ignore: prefer_constructors_over_static_methods
  static Product? fromJson(dynamic value) {
    if (value is Map) {
      final json = value.cast<String, dynamic>();

      // Ensure that the map contains the required keys.
      // Note 1: the values aren't checked for validity beyond being non-null.
      // Note 2: this code is stripped in release mode!
      assert(() {
        requiredKeys.forEach((key) {
          assert(json.containsKey(key), 'Required key "Product[$key]" is missing from JSON.');
          assert(json[key] != null, 'Required key "Product[$key]" has a null value in JSON.');
        });
        return true;
      }());

      return Product(
        id: mapValueOfType<String>(json, r'id'),
        sku: mapValueOfType<String>(json, r'sku'),
        name: mapValueOfType<String>(json, r'name'),
        brand: mapValueOfType<String>(json, r'brand'),
        storeType: mapValueOfType<int>(json, r'storeType'),
        imageUrl: mapValueOfType<String>(json, r'imageUrl'),
        maxQuantity: mapValueOfType<double>(json, r'maxQuantity'),
        isDeleted: mapValueOfType<bool>(json, r'isDeleted'),
        createdUtc: mapDateTime(json, r'createdUtc', r''),
        lastUpdatedUtc: mapDateTime(json, r'lastUpdatedUtc', r''),
        searchVector: Lexeme.listFromJson(json[r'searchVector']),
      );
    }
    return null;
  }

  static List<Product> listFromJson(dynamic json, {bool growable = false,}) {
    final result = <Product>[];
    if (json is List && json.isNotEmpty) {
      for (final row in json) {
        final value = Product.fromJson(row);
        if (value != null) {
          result.add(value);
        }
      }
    }
    return result.toList(growable: growable);
  }

  static Map<String, Product> mapFromJson(dynamic json) {
    final map = <String, Product>{};
    if (json is Map && json.isNotEmpty) {
      json = json.cast<String, dynamic>(); // ignore: parameter_assignments
      for (final entry in json.entries) {
        final value = Product.fromJson(entry.value);
        if (value != null) {
          map[entry.key] = value;
        }
      }
    }
    return map;
  }

  // maps a json object with a list of Product-objects as value to a dart map
  static Map<String, List<Product>> mapListFromJson(dynamic json, {bool growable = false,}) {
    final map = <String, List<Product>>{};
    if (json is Map && json.isNotEmpty) {
      // ignore: parameter_assignments
      json = json.cast<String, dynamic>();
      for (final entry in json.entries) {
        map[entry.key] = Product.listFromJson(entry.value, growable: growable,);
      }
    }
    return map;
  }

  /// The list of required keys that must be present in a JSON.
  static const requiredKeys = <String>{
  };
}

