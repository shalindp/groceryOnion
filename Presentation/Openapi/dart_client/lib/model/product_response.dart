//
// AUTO-GENERATED FILE, DO NOT MODIFY!
//
// @dart=2.18

// ignore_for_file: unused_element, unused_import
// ignore_for_file: always_put_required_named_parameters_first
// ignore_for_file: constant_identifier_names
// ignore_for_file: lines_longer_than_80_chars

part of openapi.api;

class ProductResponse {
  /// Returns a new [ProductResponse] instance.
  ProductResponse({
    required this.productId,
    required this.barcode,
    required this.name,
    required this.brand,
    required this.storeType,
    required this.imageUrl,
    required this.maxQuantity,
    this.pricingUrls = const [],
  });

  String productId;

  String barcode;

  String name;

  String brand;

  StoreName storeType;

  String imageUrl;

  double maxQuantity;

  List<PricingUrlResponse> pricingUrls;

  @override
  bool operator ==(Object other) => identical(this, other) || other is ProductResponse &&
    other.productId == productId &&
    other.barcode == barcode &&
    other.name == name &&
    other.brand == brand &&
    other.storeType == storeType &&
    other.imageUrl == imageUrl &&
    other.maxQuantity == maxQuantity &&
    _deepEquality.equals(other.pricingUrls, pricingUrls);

  @override
  int get hashCode =>
    // ignore: unnecessary_parenthesis
    (productId.hashCode) +
    (barcode.hashCode) +
    (name.hashCode) +
    (brand.hashCode) +
    (storeType.hashCode) +
    (imageUrl.hashCode) +
    (maxQuantity.hashCode) +
    (pricingUrls.hashCode);

  @override
  String toString() => 'ProductResponse[productId=$productId, barcode=$barcode, name=$name, brand=$brand, storeType=$storeType, imageUrl=$imageUrl, maxQuantity=$maxQuantity, pricingUrls=$pricingUrls]';

  Map<String, dynamic> toJson() {
    final json = <String, dynamic>{};
      json[r'productId'] = this.productId;
      json[r'barcode'] = this.barcode;
      json[r'name'] = this.name;
      json[r'brand'] = this.brand;
      json[r'storeType'] = this.storeType;
      json[r'imageUrl'] = this.imageUrl;
      json[r'maxQuantity'] = this.maxQuantity;
      json[r'pricingUrls'] = this.pricingUrls;
    return json;
  }

  /// Returns a new [ProductResponse] instance and imports its values from
  /// [value] if it's a [Map], null otherwise.
  // ignore: prefer_constructors_over_static_methods
  static ProductResponse? fromJson(dynamic value) {
    if (value is Map) {
      final json = value.cast<String, dynamic>();

      // Ensure that the map contains the required keys.
      // Note 1: the values aren't checked for validity beyond being non-null.
      // Note 2: this code is stripped in release mode!
      assert(() {
        requiredKeys.forEach((key) {
          assert(json.containsKey(key), 'Required key "ProductResponse[$key]" is missing from JSON.');
          assert(json[key] != null, 'Required key "ProductResponse[$key]" has a null value in JSON.');
        });
        return true;
      }());

      return ProductResponse(
        productId: mapValueOfType<String>(json, r'productId')!,
        barcode: mapValueOfType<String>(json, r'barcode')!,
        name: mapValueOfType<String>(json, r'name')!,
        brand: mapValueOfType<String>(json, r'brand')!,
        storeType: StoreName.fromJson(json[r'storeType'])!,
        imageUrl: mapValueOfType<String>(json, r'imageUrl')!,
        maxQuantity: mapValueOfType<double>(json, r'maxQuantity')!,
        pricingUrls: PricingUrlResponse.listFromJson(json[r'pricingUrls']),
      );
    }
    return null;
  }

  static List<ProductResponse> listFromJson(dynamic json, {bool growable = false,}) {
    final result = <ProductResponse>[];
    if (json is List && json.isNotEmpty) {
      for (final row in json) {
        final value = ProductResponse.fromJson(row);
        if (value != null) {
          result.add(value);
        }
      }
    }
    return result.toList(growable: growable);
  }

  static Map<String, ProductResponse> mapFromJson(dynamic json) {
    final map = <String, ProductResponse>{};
    if (json is Map && json.isNotEmpty) {
      json = json.cast<String, dynamic>(); // ignore: parameter_assignments
      for (final entry in json.entries) {
        final value = ProductResponse.fromJson(entry.value);
        if (value != null) {
          map[entry.key] = value;
        }
      }
    }
    return map;
  }

  // maps a json object with a list of ProductResponse-objects as value to a dart map
  static Map<String, List<ProductResponse>> mapListFromJson(dynamic json, {bool growable = false,}) {
    final map = <String, List<ProductResponse>>{};
    if (json is Map && json.isNotEmpty) {
      // ignore: parameter_assignments
      json = json.cast<String, dynamic>();
      for (final entry in json.entries) {
        map[entry.key] = ProductResponse.listFromJson(entry.value, growable: growable,);
      }
    }
    return map;
  }

  /// The list of required keys that must be present in a JSON.
  static const requiredKeys = <String>{
    'productId',
    'barcode',
    'name',
    'brand',
    'storeType',
    'imageUrl',
    'maxQuantity',
    'pricingUrls',
  };
}

