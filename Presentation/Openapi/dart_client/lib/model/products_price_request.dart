//
// AUTO-GENERATED FILE, DO NOT MODIFY!
//
// @dart=2.18

// ignore_for_file: unused_element, unused_import
// ignore_for_file: always_put_required_named_parameters_first
// ignore_for_file: constant_identifier_names
// ignore_for_file: lines_longer_than_80_chars

part of openapi.api;

class ProductsPriceRequest {
  /// Returns a new [ProductsPriceRequest] instance.
  ProductsPriceRequest({
    required this.productId,
    required this.storeName,
    required this.storeId,
    required this.storeSku,
  });

  String productId;

  StoreName storeName;

  String storeId;

  String storeSku;

  @override
  bool operator ==(Object other) => identical(this, other) || other is ProductsPriceRequest &&
    other.productId == productId &&
    other.storeName == storeName &&
    other.storeId == storeId &&
    other.storeSku == storeSku;

  @override
  int get hashCode =>
    // ignore: unnecessary_parenthesis
    (productId.hashCode) +
    (storeName.hashCode) +
    (storeId.hashCode) +
    (storeSku.hashCode);

  @override
  String toString() => 'ProductsPriceRequest[productId=$productId, storeName=$storeName, storeId=$storeId, storeSku=$storeSku]';

  Map<String, dynamic> toJson() {
    final json = <String, dynamic>{};
      json[r'productId'] = this.productId;
      json[r'storeName'] = this.storeName;
      json[r'storeId'] = this.storeId;
      json[r'storeSku'] = this.storeSku;
    return json;
  }

  /// Returns a new [ProductsPriceRequest] instance and imports its values from
  /// [value] if it's a [Map], null otherwise.
  // ignore: prefer_constructors_over_static_methods
  static ProductsPriceRequest? fromJson(dynamic value) {
    if (value is Map) {
      final json = value.cast<String, dynamic>();

      // Ensure that the map contains the required keys.
      // Note 1: the values aren't checked for validity beyond being non-null.
      // Note 2: this code is stripped in release mode!
      assert(() {
        requiredKeys.forEach((key) {
          assert(json.containsKey(key), 'Required key "ProductsPriceRequest[$key]" is missing from JSON.');
          assert(json[key] != null, 'Required key "ProductsPriceRequest[$key]" has a null value in JSON.');
        });
        return true;
      }());

      return ProductsPriceRequest(
        productId: mapValueOfType<String>(json, r'productId')!,
        storeName: StoreName.fromJson(json[r'storeName'])!,
        storeId: mapValueOfType<String>(json, r'storeId')!,
        storeSku: mapValueOfType<String>(json, r'storeSku')!,
      );
    }
    return null;
  }

  static List<ProductsPriceRequest> listFromJson(dynamic json, {bool growable = false,}) {
    final result = <ProductsPriceRequest>[];
    if (json is List && json.isNotEmpty) {
      for (final row in json) {
        final value = ProductsPriceRequest.fromJson(row);
        if (value != null) {
          result.add(value);
        }
      }
    }
    return result.toList(growable: growable);
  }

  static Map<String, ProductsPriceRequest> mapFromJson(dynamic json) {
    final map = <String, ProductsPriceRequest>{};
    if (json is Map && json.isNotEmpty) {
      json = json.cast<String, dynamic>(); // ignore: parameter_assignments
      for (final entry in json.entries) {
        final value = ProductsPriceRequest.fromJson(entry.value);
        if (value != null) {
          map[entry.key] = value;
        }
      }
    }
    return map;
  }

  // maps a json object with a list of ProductsPriceRequest-objects as value to a dart map
  static Map<String, List<ProductsPriceRequest>> mapListFromJson(dynamic json, {bool growable = false,}) {
    final map = <String, List<ProductsPriceRequest>>{};
    if (json is Map && json.isNotEmpty) {
      // ignore: parameter_assignments
      json = json.cast<String, dynamic>();
      for (final entry in json.entries) {
        map[entry.key] = ProductsPriceRequest.listFromJson(entry.value, growable: growable,);
      }
    }
    return map;
  }

  /// The list of required keys that must be present in a JSON.
  static const requiredKeys = <String>{
    'productId',
    'storeName',
    'storeId',
    'storeSku',
  };
}

