//
// AUTO-GENERATED FILE, DO NOT MODIFY!
//
// @dart=2.18

// ignore_for_file: unused_element, unused_import
// ignore_for_file: always_put_required_named_parameters_first
// ignore_for_file: constant_identifier_names
// ignore_for_file: lines_longer_than_80_chars

part of openapi.api;


class StoreType {
  /// Instantiate a new enum with the provided [value].
  const StoreType._(this.value);

  /// The underlying value of this enum member.
  final int value;

  @override
  String toString() => value.toString();

  int toJson() => value;

  static const number0 = StoreType._(0);
  static const number1 = StoreType._(1);

  /// List of all possible values in this [enum][StoreType].
  static const values = <StoreType>[
    number0,
    number1,
  ];

  static StoreType? fromJson(dynamic value) => StoreTypeTypeTransformer().decode(value);

  static List<StoreType> listFromJson(dynamic json, {bool growable = false,}) {
    final result = <StoreType>[];
    if (json is List && json.isNotEmpty) {
      for (final row in json) {
        final value = StoreType.fromJson(row);
        if (value != null) {
          result.add(value);
        }
      }
    }
    return result.toList(growable: growable);
  }
}

/// Transformation class that can [encode] an instance of [StoreType] to int,
/// and [decode] dynamic data back to [StoreType].
class StoreTypeTypeTransformer {
  factory StoreTypeTypeTransformer() => _instance ??= const StoreTypeTypeTransformer._();

  const StoreTypeTypeTransformer._();

  int encode(StoreType data) => data.value;

  /// Decodes a [dynamic value][data] to a StoreType.
  ///
  /// If [allowNull] is true and the [dynamic value][data] cannot be decoded successfully,
  /// then null is returned. However, if [allowNull] is false and the [dynamic value][data]
  /// cannot be decoded successfully, then an [UnimplementedError] is thrown.
  ///
  /// The [allowNull] is very handy when an API changes and a new enum value is added or removed,
  /// and users are still using an old app with the old code.
  StoreType? decode(dynamic data, {bool allowNull = true}) {
    if (data != null) {
      switch (data) {
        case 0: return StoreType.number0;
        case 1: return StoreType.number1;
        default:
          if (!allowNull) {
            throw ArgumentError('Unknown enum value to decode: $data');
          }
      }
    }
    return null;
  }

  /// Singleton [StoreTypeTypeTransformer] instance.
  static StoreTypeTypeTransformer? _instance;
}

