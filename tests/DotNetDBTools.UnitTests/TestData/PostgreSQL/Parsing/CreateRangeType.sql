--ID:#{3B48FE5B-E812-4359-96A6-0FEA4613CBB2}#
CREATE TYPE "MyRangeType1" AS RANGE
(
    subtype = floAT8,
    SUBTYPE_OPCLASS = "TIMESTAMP_OPS",
    collation = "C",
    CANONICAL = "some_func",
    subtype_diff = float8mi,
    MULTIRANGE_TYPE_NAME = "MyRangeType1_multirange"
);