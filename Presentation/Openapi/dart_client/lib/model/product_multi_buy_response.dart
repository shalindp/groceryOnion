//
// AUTO-GENERATED FILE, DO NOT MODIFY!
//
// @dart=2.18

// ignore_for_file: unused_element, unused_import
// ignore_for_file: always_put_required_named_parameters_first
// ignore_for_file: constant_identifier_names
// ignore_for_file: lines_longer_than_80_chars

part of openapi.api;

class ProductMultiBuyResponse {
  /// Returns a new [ProductMultiBuyResponse] instance.
  ProductMultiBuyResponse({
    required this.priceWhenQuantityIsMet,
    required this.quantityRequired,
  });

  double priceWhenQuantityIsMet;

  double quantityRequired;

  @override
  bool operator ==(Object other) => identical(this, other) || other is ProductMultiBuyResponse &&
    other.priceWhenQuantityIsMet == priceWhenQuantityIsMet &&
    other.quantityRequired == quantityRequired;

  @override
  int get hashCode =>
    // ignore: unnecessary_parenthesis
    (priceWhenQuantityIsMet.hashCode) +
    (quantityRequired.hashCode);

  @override
  String toString() => 'ProductMultiBuyResponse[priceWhenQuantityIsMet=$priceWhenQuantityIsMet, quantityRequired=$quantityRequired]';

  Map<String, dynamic> toJson() {
    final json = <String, dynamic>{};
      json[r'priceWhenQuantityIsMet'] = this.priceWhenQuantityIsMet;
      json[r'quantityRequired'] = this.quantityRequired;
    return json;
  }

  /// Returns a new [ProductMultiBuyResponse] instance and imports its values from
  /// [value] if it's a [Map], null otherwise.
  // ignore: prefer_constructors_over_static_methods
  static ProductMultiBuyResponse? fromJson(dynamic value) {
    if (value is Map) {
      final json = value.cast<String, dynamic>();

      // Ensure that the map contains the required keys.
      // Note 1: the values aren't checked for validity beyond being non-null.
      // Note 2: this code is stripped in release mode!
      assert(() {
        requiredKeys.forEach((key) {
          assert(json.containsKey(key), 'Required key "ProductMultiBuyResponse[$key]" is missing from JSON.');
          assert(json[key] != null, 'Required key "ProductMultiBuyResponse[$key]" has a null value in JSON.');
        });
        return true;
      }());

      return ProductMultiBuyResponse(
        priceWhenQuantityIsMet: mapValueOfType<double>(json, r'priceWhenQuantityIsMet')!,
        quantityRequired: mapValueOfType<double>(json, r'quantityRequired')!,
      );
    }
    return null;
  }

  static List<ProductMultiBuyResponse> listFromJson(dynamic json, {bool growable = false,}) {
    final result = <ProductMultiBuyResponse>[];
    if (json is List && json.isNotEmpty) {
      for (final row in json) {
        final value = ProductMultiBuyResponse.fromJson(row);
        if (value != null) {
          result.add(value);
        }
      }
    }
    return result.toList(growable: growable);
  }

  static Map<String, ProductMultiBuyResponse> mapFromJson(dynamic json) {
    final map = <String, ProductMultiBuyResponse>{};
    if (json is Map && json.isNotEmpty) {
      json = json.cast<String, dynamic>(); // ignore: parameter_assignments
      for (final entry in json.entries) {
        final value = ProductMultiBuyResponse.fromJson(entry.value);
        if (value != null) {
          map[entry.key] = value;
        }
      }
    }
    return map;
  }

  // maps a json object with a list of ProductMultiBuyResponse-objects as value to a dart map
  static Map<String, List<ProductMultiBuyResponse>> mapListFromJson(dynamic json, {bool growable = false,}) {
    final map = <String, List<ProductMultiBuyResponse>>{};
    if (json is Map && json.isNotEmpty) {
      // ignore: parameter_assignments
      json = json.cast<String, dynamic>();
      for (final entry in json.entries) {
        map[entry.key] = ProductMultiBuyResponse.listFromJson(entry.value, growable: growable,);
      }
    }
    return map;
  }

  /// The list of required keys that must be present in a JSON.
  static const requiredKeys = <String>{
    'priceWhenQuantityIsMet',
    'quantityRequired',
  };
}

