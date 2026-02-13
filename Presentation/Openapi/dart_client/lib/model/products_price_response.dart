//
// AUTO-GENERATED FILE, DO NOT MODIFY!
//
// @dart=2.18

// ignore_for_file: unused_element, unused_import
// ignore_for_file: always_put_required_named_parameters_first
// ignore_for_file: constant_identifier_names
// ignore_for_file: lines_longer_than_80_chars

part of openapi.api;

class ProductsPriceResponse {
  /// Returns a new [ProductsPriceResponse] instance.
  ProductsPriceResponse({
    required this.productId,
    required this.storeName,
    required this.storeId,
    required this.storeSku,
    required this.price,
    this.multiBuys = const [],
  });

  String productId;

  StoreName storeName;

  String storeId;

  String storeSku;

  double price;

  List<ProductMultiBuyResponse> multiBuys;

  @override
  bool operator ==(Object other) => identical(this, other) || other is ProductsPriceResponse &&
    other.productId == productId &&
    other.storeName == storeName &&
    other.storeId == storeId &&
    other.storeSku == storeSku &&
    other.price == price &&
    _deepEquality.equals(other.multiBuys, multiBuys);

  @override
  int get hashCode =>
    // ignore: unnecessary_parenthesis
    (productId.hashCode) +
    (storeName.hashCode) +
    (storeId.hashCode) +
    (storeSku.hashCode) +
    (price.hashCode) +
    (multiBuys.hashCode);

  @override
  String toString() => 'ProductsPriceResponse[productId=$productId, storeName=$storeName, storeId=$storeId, storeSku=$storeSku, price=$price, multiBuys=$multiBuys]';

  Map<String, dynamic> toJson() {
    final json = <String, dynamic>{};
      json[r'productId'] = this.productId;
      json[r'storeName'] = this.storeName;
      json[r'storeId'] = this.storeId;
      json[r'storeSku'] = this.storeSku;
      json[r'price'] = this.price;
      json[r'multiBuys'] = this.multiBuys;
    return json;
  }

  /// Returns a new [ProductsPriceResponse] instance and imports its values from
  /// [value] if it's a [Map], null otherwise.
  // ignore: prefer_constructors_over_static_methods
  static ProductsPriceResponse? fromJson(dynamic value) {
    if (value is Map) {
      final json = value.cast<String, dynamic>();

      // Ensure that the map contains the required keys.
      // Note 1: the values aren't checked for validity beyond being non-null.
      // Note 2: this code is stripped in release mode!
      assert(() {
        requiredKeys.forEach((key) {
          assert(json.containsKey(key), 'Required key "ProductsPriceResponse[$key]" is missing from JSON.');
          assert(json[key] != null, 'Required key "ProductsPriceResponse[$key]" has a null value in JSON.');
        });
        return true;
      }());

      return ProductsPriceResponse(
        productId: mapValueOfType<String>(json, r'productId')!,
        storeName: StoreName.fromJson(json[r'storeName'])!,
        storeId: mapValueOfType<String>(json, r'storeId')!,
        storeSku: mapValueOfType<String>(json, r'storeSku')!,
        price: mapValueOfType<double>(json, r'price')!,
        multiBuys: ProductMultiBuyResponse.listFromJson(json[r'multiBuys']),
      );
    }
    return null;
  }

  static List<ProductsPriceResponse> listFromJson(dynamic json, {bool growable = false,}) {
    final result = <ProductsPriceResponse>[];
    if (json is List && json.isNotEmpty) {
      for (final row in json) {
        final value = ProductsPriceResponse.fromJson(row);
        if (value != null) {
          result.add(value);
        }
      }
    }
    return result.toList(growable: growable);
  }

  static Map<String, ProductsPriceResponse> mapFromJson(dynamic json) {
    final map = <String, ProductsPriceResponse>{};
    if (json is Map && json.isNotEmpty) {
      json = json.cast<String, dynamic>(); // ignore: parameter_assignments
      for (final entry in json.entries) {
        final value = ProductsPriceResponse.fromJson(entry.value);
        if (value != null) {
          map[entry.key] = value;
        }
      }
    }
    return map;
  }

  // maps a json object with a list of ProductsPriceResponse-objects as value to a dart map
  static Map<String, List<ProductsPriceResponse>> mapListFromJson(dynamic json, {bool growable = false,}) {
    final map = <String, List<ProductsPriceResponse>>{};
    if (json is Map && json.isNotEmpty) {
      // ignore: parameter_assignments
      json = json.cast<String, dynamic>();
      for (final entry in json.entries) {
        map[entry.key] = ProductsPriceResponse.listFromJson(entry.value, growable: growable,);
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
    'price',
    'multiBuys',
  };
}

