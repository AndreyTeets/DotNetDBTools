--ID:#{4B92637E-F6C8-42B8-99CB-4B988D98CAEE}#
CREATE DOMAIN "MyDomain1" AS decimal(6, 1) NOT NULL DEFAULT abs(-33)
    --ID:#{3C7DF430-DDC3-4EE7-93CC-70E7427E7937}#
    CHECK (value = lower(value) || 'CHECK (TRUE)')
    --ID:#{960EFE55-2985-4057-8A83-EF7F5FF6C3CA}#
    CONSTRAINT "MyDomain1_CK2" check (char_length(value) > 3);