//
// AUTO-GENERATED FILE, DO NOT MODIFY!
//
// @dart=2.18

// ignore_for_file: unused_element, unused_import
// ignore_for_file: always_put_required_named_parameters_first
// ignore_for_file: constant_identifier_names
// ignore_for_file: lines_longer_than_80_chars

part of openapi.api;


class StoreName {
  /// Instantiate a new enum with the provided [value].
  const StoreName._(this.value);

  /// The underlying value of this enum member.
  final int value;

  @override
  String toString() => value.toString();

  int toJson() => value;

  static const number0 = StoreName._(0);
  static const number1 = StoreName._(1);

  /// List of all possible values in this [enum][StoreName].
  static const values = <StoreName>[
    number0,
    number1,
  ];

  static StoreName? fromJson(dynamic value) => StoreNameTypeTransformer().decode(value);

  static List<StoreName> listFromJson(dynamic json, {bool growable = false,}) {
    final result = <StoreName>[];
    if (json is List && json.isNotEmpty) {
      for (final row in json) {
        final value = StoreName.fromJson(row);
        if (value != null) {
          result.add(value);
        }
      }
    }
    return result.toList(growable: growable);
  }
}

/// Transformation class that can [encode] an instance of [StoreName] to int,
/// and [decode] dynamic data back to [StoreName].
class StoreNameTypeTransformer {
  factory StoreNameTypeTransformer() => _instance ??= const StoreNameTypeTransformer._();

  const StoreNameTypeTransformer._();

  int encode(StoreName data) => data.value;

  /// Decodes a [dynamic value][data] to a StoreName.
  ///
  /// If [allowNull] is true and the [dynamic value][data] cannot be decoded successfully,
  /// then null is returned. However, if [allowNull] is false and the [dynamic value][data]
  /// cannot be decoded successfully, then an [UnimplementedError] is thrown.
  ///
  /// The [allowNull] is very handy when an API changes and a new enum value is added or removed,
  /// and users are still using an old app with the old code.
  StoreName? decode(dynamic data, {bool allowNull = true}) {
    if (data != null) {
      switch (data) {
        case 0: return StoreName.number0;
        case 1: return StoreName.number1;
        default:
          if (!allowNull) {
            throw ArgumentError('Unknown enum value to decode: $data');
          }
      }
    }
    return null;
  }

  /// Singleton [StoreNameTypeTransformer] instance.
  static StoreNameTypeTransformer? _instance;
}

