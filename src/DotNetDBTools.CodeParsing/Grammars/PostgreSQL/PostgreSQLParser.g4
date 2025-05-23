/*
 This file includes work originally covered by the following copyright and permission notices:

 Licensed to the Apache Software Foundation (ASF) under one
 or more contributor license agreements.  See the NOTICE file
 distributed with this work for additional information
 regarding copyright ownership.  The ASF licenses this file
 to you under the Apache License, Version 2.0 (the
 "License"); you may not use this file except in compliance
 with the License.  You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

 Unless required by applicable law or agreed to in writing, software
 distributed under the License is distributed on an "AS IS" BASIS,
 WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 See the License for the specific language governing permissions and
 limitations under the License.
 */

parser grammar PostgreSQLParser;

options {
    language=CSharp;
    tokenVocab=PostgreSQLLexer;
}

@header {
}

// to start parsing, it is recommended to use only rules with EOF
// this eliminates the ambiguous parsing options and speeds up the process
/******* Start symbols *******/

sql
    : BOM? SEMI_COLON* statement (SEMI_COLON+ statement)* SEMI_COLON* EOF
    ;

sql_function_def
    : (function_body | SEMI_COLON* function_statement? (SEMI_COLON+ function_statement)* SEMI_COLON*) EOF
    ;

plpgsql_function_def
    : comp_options? function_block SEMI_COLON? EOF
    ;

dndbt_sqldef_create_statement
    : ( create_table_statement
      | create_view_statement
      | create_index_statement
      | create_trigger_statement
      | create_sequence_statement
      | create_type_statement
      | create_domain_statement
      | create_function_statement)
      SEMI_COLON? EOF
    ;

/******* END Start symbols *******/

statement
    : data_statement
    | schema_statement
    | script_statement
    ;

data_statement
    : select_stmt
    | insert_stmt
    | update_stmt
    | merge_stmt
    | delete_stmt
    ;

plpgsql_data_statement
    : plpgsql_select_stmt
    | insert_stmt
    | update_stmt
    | merge_stmt
    | delete_stmt
    ;

script_statement
    : script_transaction
    | script_additional
    ;

script_transaction
    : (START TRANSACTION | BEGIN (WORK | TRANSACTION)?) (transaction_mode (COMMA transaction_mode)*)?
    | (COMMIT | END | ABORT | ROLLBACK) (WORK | TRANSACTION)? (AND NO? CHAIN)?
    | (COMMIT PREPARED | PREPARE TRANSACTION) Character_String_Literal
    | (SAVEPOINT | RELEASE SAVEPOINT?) identifier
    | ROLLBACK PREPARED Character_String_Literal
    | ROLLBACK (WORK | TRANSACTION)? TO SAVEPOINT? identifier
    | lock_table
    ;

transaction_mode
    : ISOLATION LEVEL (SERIALIZABLE | REPEATABLE READ | READ COMMITTED | READ UNCOMMITTED)
    | READ WRITE
    | READ ONLY
    | NOT? DEFERRABLE
    ;

lock_table
    : LOCK TABLE? only_table_multiply (COMMA only_table_multiply)* (IN lock_mode MODE)? NOWAIT?
    ;

lock_mode
    : (ROW | ACCESS) SHARE
    | ROW EXCLUSIVE
    | SHARE (ROW | UPDATE) EXCLUSIVE
    | SHARE
    | ACCESS? EXCLUSIVE
    ;

script_additional
    : additional_statement
    | VACUUM vacuum_mode table_cols_list?
    | (FETCH | MOVE) fetch_move_direction? (FROM | IN)? identifier
    | CLOSE (identifier | ALL)
    | CALL function_call
    | DISCARD (ALL | PLANS | SEQUENCES | TEMPORARY | TEMP)
    | declare_statement
    | execute_statement
    | explain_statement
    | show_statement
    ;

additional_statement
    : anonymous_block
    | LISTEN identifier
    | UNLISTEN (identifier | MULTIPLY)
    | ANALYZE (LEFT_PAREN analyze_mode (COMMA analyze_mode)* RIGHT_PAREN | VERBOSE)? table_cols_list?
    | CLUSTER VERBOSE? (identifier ON schema_qualified_name | schema_qualified_name (USING identifier)?)?
    | CHECKPOINT
    | LOAD Character_String_Literal
    | DEALLOCATE PREPARE? (identifier | ALL)
    | REINDEX (LEFT_PAREN VERBOSE RIGHT_PAREN)? (INDEX | TABLE | SCHEMA | DATABASE | SYSTEM) CONCURRENTLY? schema_qualified_name
    | RESET ((identifier DOT)? identifier | TIME ZONE | SESSION AUTHORIZATION | ALL)
    | REFRESH MATERIALIZED VIEW CONCURRENTLY? schema_qualified_name (WITH NO? DATA)?
    | PREPARE identifier (LEFT_PAREN data_type (COMMA data_type)* RIGHT_PAREN)? AS data_statement
    | REASSIGN OWNED BY user_name (COMMA user_name)* TO user_name
    | copy_statement
    | truncate_stmt
    | notify_stmt
    ;

explain_statement
    : EXPLAIN (ANALYZE? VERBOSE? | LEFT_PAREN explain_option (COMMA explain_option)* RIGHT_PAREN) explain_query;

explain_query
    : data_statement
    | execute_statement
    | declare_statement
    | (create_table_as_statement | create_view_statement)
    ;

execute_statement
    : EXECUTE identifier (LEFT_PAREN vex (COMMA vex)* RIGHT_PAREN)?
    ;

declare_statement
    : DECLARE identifier BINARY? INSENSITIVE? (NO? SCROLL)? CURSOR ((WITH | WITHOUT) HOLD)? FOR select_stmt
    ;

show_statement
    : SHOW ((identifier DOT)? identifier | ALL | TIME ZONE | TRANSACTION ISOLATION LEVEL | SESSION AUTHORIZATION)
    ;

explain_option
    : (ANALYZE | VERBOSE | COSTS | SETTINGS | BUFFERS | WAL | TIMING | SUMMARY) boolean_value?
    | FORMAT (TEXT | XML | JSON | YAML)
    ;

user_name
    : identifier | CURRENT_ROLE | CURRENT_USER | SESSION_USER
    ;

table_cols_list
    : table_cols (COMMA table_cols)*
    ;

table_cols
    : schema_qualified_name (LEFT_PAREN identifier (COMMA identifier)* RIGHT_PAREN)?
    ;

vacuum_mode
    : LEFT_PAREN vacuum_option (COMMA vacuum_option)* RIGHT_PAREN
    | FULL? FREEZE? VERBOSE? ANALYZE?
    ;

vacuum_option
    : (FULL | FREEZE | VERBOSE | ANALYZE | DISABLE_PAGE_SKIPPING | SKIP_LOCKED | INDEX_CLEANUP | TRUNCATE) boolean_value?
    | PARALLEL NUMBER_LITERAL
    ;

analyze_mode
    : (VERBOSE | SKIP_LOCKED) boolean_value?
    ;

boolean_value
    : TRUE
    | FALSE
    | OFF
    | ON
    | NUMBER_LITERAL
    | character_string // 'true', 'false', 'on', 'off'
    ;

fetch_move_direction
    : NEXT
    | PRIOR
    | FIRST
    | LAST
    | (ABSOLUTE | RELATIVE)? signed_number_literal
    | ALL
    | FORWARD (NUMBER_LITERAL | ALL)?
    | BACKWARD (NUMBER_LITERAL | ALL)?
    ;

schema_statement
    : schema_create
    | schema_alter
    | schema_drop
    ;

schema_create
    : create_access_method_statement
    | create_aggregate_statement
    | create_cast_statement
    | create_collation_statement
    | create_conversion_statement
    | create_database_statement
    | create_domain_statement
    | create_event_trigger_statement
    | create_extension_statement
    | create_foreign_data_wrapper_statement
    | create_foreign_table_statement
    | create_fts_configuration_statement
    | create_fts_dictionary_statement
    | create_fts_parser_statement
    | create_fts_template_statement
    | create_function_statement
    | create_group_statement
    | create_index_statement
    | create_language_statement
    | create_operator_class_statement
    | create_operator_family_statement
    | create_operator_statement
    | create_policy_statement
    | create_publication_statement
    | create_rewrite_statement
    | create_schema_statement
    | create_sequence_statement
    | create_server_statement
    | create_statistics_statement
    | create_subscription_statement
    | create_table_as_statement
    | create_table_statement
    | create_tablespace_statement
    | create_transform_statement
    | create_trigger_statement
    | create_type_statement
    | create_user_mapping_statement
    | create_user_or_role_statement
    | create_view_statement

    | comment_on_statement
    | rule_common
    | schema_import
    | security_label
    | set_statement
    ;

schema_alter
    : ALTER (alter_aggregate_statement
    | alter_collation_statement
    | alter_conversion_statement
    | alter_default_privileges_statement
    | alter_database_statement
    | alter_domain_statement
    | alter_event_trigger_statement
    | alter_extension_statement
    | alter_foreign_data_wrapper
    | alter_fts_statement
    | alter_function_statement
    | alter_group_statement
    | alter_index_statement
    | alter_language_statement
    | alter_materialized_view_statement
    | alter_operator_class_statement
    | alter_operator_family_statement
    | alter_operator_statement
    | alter_owner_statement
    | alter_policy_statement
    | alter_publication_statement
    | alter_rule_statement
    | alter_schema_statement
    | alter_sequence_statement
    | alter_server_statement
    | alter_statistics_statement
    | alter_subscription_statement
    | alter_table_statement
    | alter_tablespace_statement
    | alter_trigger_statement
    | alter_type_statement
    | alter_user_mapping_statement
    | alter_user_or_role_statement
    | alter_view_statement)
    ;

schema_drop
    : DROP (drop_cast_statement
    | drop_database_statement
    | drop_function_statement
    | drop_operator_class_statement
    | drop_operator_family_statement
    | drop_operator_statement
    | drop_owned_statement
    | drop_policy_statement
    | drop_rule_statement
    | drop_statements
    | drop_trigger_statement
    | drop_user_mapping_statement)
    ;

schema_import
    : IMPORT FOREIGN SCHEMA name=identifier
    ((LIMIT TO | EXCEPT) LEFT_PAREN identifier_list RIGHT_PAREN)?
    FROM SERVER identifier INTO identifier
    define_foreign_options?
    ;

alter_function_statement
    : (FUNCTION | PROCEDURE | ROUTINE) schema_qualified_name function_args?
      ((function_actions_common | RESET ((identifier DOT)? identifier | ALL))+ RESTRICT?
    | rename_to
    | set_schema
    | NO? DEPENDS ON EXTENSION identifier)
    ;

alter_aggregate_statement
    : AGGREGATE function_parameters (rename_to | set_schema)
    ;

alter_extension_statement
    : EXTENSION identifier alter_extension_action
    ;

alter_extension_action
    : set_schema
    | UPDATE (TO (identifier | character_string))?
    | (ADD | DROP) extension_member_object
    ;

extension_member_object
    : ACCESS METHOD schema_qualified_name
    | AGGREGATE function_parameters
    | CAST LEFT_PAREN cast_name RIGHT_PAREN
    | COLLATION identifier
    | CONVERSION identifier
    | DOMAIN schema_qualified_name
    | EVENT TRIGGER identifier
    | FOREIGN DATA WRAPPER identifier
    | FOREIGN TABLE schema_qualified_name
    | FUNCTION function_parameters
    | MATERIALIZED? VIEW schema_qualified_name
    | OPERATOR operator_name
    | OPERATOR CLASS schema_qualified_name USING identifier
    | OPERATOR FAMILY schema_qualified_name USING identifier
    | PROCEDURAL? LANGUAGE identifier
    | PROCEDURE function_parameters
    | ROUTINE function_parameters
    | SCHEMA identifier
    | SEQUENCE schema_qualified_name
    | SERVER identifier
    | TABLE schema_qualified_name
    | TEXT SEARCH CONFIGURATION schema_qualified_name
    | TEXT SEARCH DICTIONARY schema_qualified_name
    | TEXT SEARCH PARSER schema_qualified_name
    | TEXT SEARCH TEMPLATE schema_qualified_name
    | TRANSFORM FOR identifier LANGUAGE identifier
    | TYPE schema_qualified_name
    ;

alter_schema_statement
    : SCHEMA identifier rename_to
    ;

alter_language_statement
    : PROCEDURAL? LANGUAGE name=identifier (rename_to | owner_to)
    ;

alter_table_statement
    : FOREIGN? TABLE if_exists? ONLY? name=schema_qualified_name MULTIPLY?(
        table_action (COMMA table_action)*
        | RENAME COLUMN? identifier TO identifier
        | set_schema
        | rename_to
        | RENAME CONSTRAINT identifier TO identifier
        | ATTACH PARTITION child=schema_qualified_name for_values_bound
        | DETACH PARTITION child=schema_qualified_name (CONCURRENTLY | FINALIZE)?)
    ;

table_action
    : ADD COLUMN? if_not_exists? table_column_definition
    | DROP COLUMN? if_exists? column=identifier cascade_restrict?
    | ALTER COLUMN? column=identifier column_action
    | ADD tabl_constraint=constraint_common (NOT not_valid=VALID)?
    | validate_constraint
    | drop_constraint
    | (DISABLE | ENABLE) TRIGGER (trigger_name=schema_qualified_name | ALL | USER)?
    | ENABLE (REPLICA | ALWAYS) TRIGGER trigger_name=schema_qualified_name
    | (DISABLE | ENABLE) RULE rewrite_rule_name=schema_qualified_name
    | ENABLE (REPLICA | ALWAYS) RULE rewrite_rule_name=schema_qualified_name
    | (DISABLE | ENABLE) ROW LEVEL SECURITY
    | NO? FORCE ROW LEVEL SECURITY
    | CLUSTER ON index_name=schema_qualified_name
    | SET WITHOUT (CLUSTER | OIDS)
    | SET WITH OIDS
    | SET (LOGGED | UNLOGGED)
    | SET storage_parameter
    | RESET names_in_parens
    | define_foreign_options
    | INHERIT parent_table=schema_qualified_name
    | NO INHERIT parent_table=schema_qualified_name
    | OF type_name=schema_qualified_name
    | NOT OF
    | owner_to
    | set_tablespace
    | REPLICA IDENTITY (DEFAULT | FULL | NOTHING | USING INDEX identifier)
    | ALTER CONSTRAINT identifier table_deferrable? table_initialy_immed?
    ;

column_action
    : (SET DATA)? TYPE data_type collate_identifier? (USING vex)?
    | ADD identity_body
    | set_def_column
    | drop_def
    | (set=SET | DROP) NOT NULL
    | DROP IDENTITY if_exists?
    | DROP EXPRESSION if_exists?
    | SET storage_parameter
    | SET compression_identifier
    | set_statistics
    | SET STORAGE storage_option
    | RESET names_in_parens
    | define_foreign_options
    | alter_identity+
    ;

identity_body
    : GENERATED (ALWAYS | BY DEFAULT) AS IDENTITY (LEFT_PAREN sequence_body+ RIGHT_PAREN)?
    ;

alter_identity
    : SET GENERATED (ALWAYS | BY DEFAULT)
    | SET sequence_body
    | RESTART (WITH? NUMBER_LITERAL)?
    ;

storage_option
    : PLAIN
    | EXTERNAL
    | EXTENDED
    | MAIN
    ;

validate_constraint
    : VALIDATE CONSTRAINT constraint_name=schema_qualified_name
    ;

drop_constraint
    : DROP CONSTRAINT if_exists? constraint_name=identifier cascade_restrict?
    ;

table_deferrable
    : NOT? DEFERRABLE
    ;

table_initialy_immed
    : INITIALLY (DEFERRED | IMMEDIATE)
    ;

function_actions_common
    : (CALLED | RETURNS NULL) ON NULL INPUT
    | TRANSFORM transform_for_type (COMMA transform_for_type)*
    | STRICT
    | IMMUTABLE
    | VOLATILE
    | STABLE
    | NOT? LEAKPROOF
    | EXTERNAL? SECURITY (INVOKER | DEFINER)
    | PARALLEL (SAFE | UNSAFE | RESTRICTED)
    | COST execution_cost=unsigned_numeric_literal
    | ROWS result_rows=unsigned_numeric_literal
    | SUPPORT schema_qualified_name
    | SET (config_scope=identifier DOT)? config_param=identifier ((TO | EQUAL) set_statement_value | FROM CURRENT)
    | LANGUAGE lang_name=function_language
    | WINDOW
    | AS function_def
    ;

function_language
    : identifier
    | Character_String_Literal
    ;

function_def
    : definition=character_string (COMMA symbol=character_string)?
    ;

alter_index_statement
    : INDEX if_exists? schema_qualified_name index_def_action
    | INDEX ALL IN TABLESPACE identifier (OWNED BY identifier_list)? set_tablespace
    ;

index_def_action
    : rename_to
    | ATTACH PARTITION index=schema_qualified_name
    | NO? DEPENDS ON EXTENSION schema_qualified_name
    | ALTER COLUMN? (NUMBER_LITERAL | identifier) (set_statistics | SET storage_parameter)
    | RESET LEFT_PAREN identifier_list RIGHT_PAREN
    | set_tablespace
    | SET storage_parameter
    ;

alter_default_privileges_statement
    : DEFAULT PRIVILEGES
    (FOR (ROLE | USER) identifier_list)?
    (IN SCHEMA identifier_list)?
    abbreviated_grant_or_revoke
    ;

abbreviated_grant_or_revoke
    : (GRANT | REVOKE grant_option_for?) (
        table_column_privilege (COMMA table_column_privilege)* ON TABLES
        | (usage_select_update (COMMA usage_select_update)* | ALL PRIVILEGES?) ON SEQUENCES
        | (EXECUTE | ALL PRIVILEGES?) ON (FUNCTIONS | ROUTINES)
        | (USAGE | CREATE | ALL PRIVILEGES?) ON SCHEMAS
        | (USAGE | ALL PRIVILEGES?) ON TYPES)
    (grant_to_rule | revoke_from_cascade_restrict)
    ;

grant_option_for
    : GRANT OPTION FOR
    ;

alter_sequence_statement
    : SEQUENCE if_exists? name=schema_qualified_name
     ( sequence_body*
        | SET (LOGGED | UNLOGGED)
        | owner_to
        | set_schema
        | rename_to)
    ;

alter_view_statement
    : VIEW if_exists? name=schema_qualified_name alter_view_action
    ;

alter_view_action
    : ALTER COLUMN? column_name=identifier set_def_column
    | ALTER COLUMN? column_name=identifier drop_def
    | RENAME COLUMN? identifier TO identifier
    | rename_to
    | set_schema
    | SET storage_parameter
    | RESET names_in_parens
    ;

alter_materialized_view_statement
    : MATERIALIZED VIEW if_exists? schema_qualified_name alter_materialized_view_action
    | MATERIALIZED VIEW ALL IN TABLESPACE identifier (OWNED BY identifier_list)? set_tablespace
    ;

alter_materialized_view_action
    : rename_to
    | set_schema
    | RENAME COLUMN? identifier TO identifier
    | NO? DEPENDS ON EXTENSION identifier
    | materialized_view_action (COMMA materialized_view_action)*
    ;

materialized_view_action
    : ALTER COLUMN? identifier set_statistics
    | ALTER COLUMN? identifier SET storage_parameter
    | ALTER COLUMN? identifier RESET names_in_parens
    | ALTER COLUMN? identifier SET STORAGE storage_option
    | ALTER COLUMN? identifier SET compression_identifier
    | CLUSTER ON index_name=schema_qualified_name
    | SET WITHOUT CLUSTER
    | SET access_method_definition
    | set_tablespace
    | SET storage_parameter
    | RESET names_in_parens
    | owner_to
    ;

alter_event_trigger_statement
    : EVENT TRIGGER name=identifier alter_event_trigger_action
    ;

alter_event_trigger_action
    : DISABLE
    | ENABLE (REPLICA | ALWAYS)?
    | owner_to
    | rename_to
    ;

alter_type_statement
    : TYPE name=schema_qualified_name
      (set_schema
      | rename_to
      | ADD VALUE if_not_exists? new_enum_value=character_string ((BEFORE | AFTER) existing_enum_value=character_string)?
      | RENAME ATTRIBUTE attribute_name=identifier TO new_attribute_name=identifier cascade_restrict?
      | RENAME VALUE existing_enum_name=character_string TO new_enum_name=character_string
      | type_action (COMMA type_action)*
      | SET LEFT_PAREN type_property (COMMA type_property)* RIGHT_PAREN)
    ;

alter_domain_statement
    : DOMAIN name=schema_qualified_name
    (set_def_column
    | drop_def
    | (SET | DROP) NOT NULL
    | ADD dom_constraint=domain_constraint (NOT not_valid=VALID)?
    | drop_constraint
    | RENAME CONSTRAINT schema_qualified_name TO schema_qualified_name
    | validate_constraint
    | rename_to
    | set_schema)
    ;

alter_server_statement
    : SERVER identifier alter_server_action
    ;

alter_server_action
    : (VERSION character_string)? define_foreign_options
    | VERSION character_string
    | rename_to
    ;

alter_fts_statement
    : TEXT SEARCH
      ((TEMPLATE | DICTIONARY | CONFIGURATION | PARSER) name=schema_qualified_name (rename_to | set_schema)
      | DICTIONARY name=schema_qualified_name storage_parameter
      | CONFIGURATION name=schema_qualified_name alter_fts_configuration)
    ;

alter_fts_configuration
    : (ADD | ALTER) MAPPING FOR identifier_list WITH schema_qualified_name (COMMA schema_qualified_name)*
    | ALTER MAPPING (FOR identifier_list)? REPLACE schema_qualified_name WITH schema_qualified_name
    | DROP MAPPING (IF EXISTS)? FOR identifier_list
    ;

type_action
    : ADD ATTRIBUTE identifier data_type collate_identifier? cascade_restrict?
    | DROP ATTRIBUTE if_exists? identifier cascade_restrict?
    | ALTER ATTRIBUTE identifier (SET DATA)? TYPE data_type collate_identifier? cascade_restrict?
    ;

type_property
    : (RECEIVE | SEND | TYPMOD_IN | TYPMOD_OUT | ANALYZE) EQUAL schema_qualified_name
    | STORAGE EQUAL storage=storage_option
    ;

set_def_column
    : SET DEFAULT vex
    ;

drop_def
    : DROP DEFAULT
    ;

create_index_statement
    : dndbt_id=DNDBT_ID_DECLARATION_COMMENT?
      CREATE UNIQUE? INDEX CONCURRENTLY? if_not_exists? name=identifier? ON ONLY? table_name=schema_qualified_name
      (USING method=identifier)? index_columns including_index? with_storage_parameter? table_space? index_where?
    ;

index_columns
    : LEFT_PAREN index_column (COMMA index_column)* RIGHT_PAREN
    ;

index_column
    : column=vex operator_class=schema_qualified_name?
    (LEFT_PAREN option_with_value (COMMA option_with_value)* RIGHT_PAREN)?
    order_specification? null_ordering?
    ;

including_index
    : INCLUDE LEFT_PAREN identifier (COMMA identifier)* RIGHT_PAREN
    ;

index_where
    : WHERE vex
    ;

 create_extension_statement
    : CREATE EXTENSION if_not_exists? name=identifier
    WITH?
    (SCHEMA schema=identifier)?
    (VERSION (identifier | character_string))?
    (FROM (identifier | character_string))?
    CASCADE?
    ;

create_language_statement
    : CREATE (OR REPLACE)? TRUSTED? PROCEDURAL? LANGUAGE name=identifier
    (HANDLER schema_qualified_name (INLINE schema_qualified_name)? (VALIDATOR schema_qualified_name)?)?
    ;

create_event_trigger_statement
    : CREATE EVENT TRIGGER name=identifier ON identifier
    (WHEN (schema_qualified_name IN LEFT_PAREN character_string (COMMA character_string)* RIGHT_PAREN AND?)+ )?
    EXECUTE (PROCEDURE | FUNCTION) vex
    ;

create_type_statement
    : dndbt_id=DNDBT_ID_DECLARATION_COMMENT?
      CREATE TYPE name=schema_qualified_name (AS(
        LEFT_PAREN (attrs+=table_column_definition (COMMA attrs+=table_column_definition)*)? RIGHT_PAREN
        | ENUM LEFT_PAREN ( enums+=character_string (COMMA enums+=character_string)* )? RIGHT_PAREN
        | RANGE LEFT_PAREN
                (SUBTYPE EQUAL subtype_name=data_type
                | SUBTYPE_OPCLASS EQUAL subtype_operator_class=identifier
                | COLLATION EQUAL collation=schema_qualified_name
                | CANONICAL EQUAL canonical_function=schema_qualified_name
                | SUBTYPE_DIFF EQUAL subtype_diff_function=schema_qualified_name
                | MULTIRANGE_TYPE_NAME EQUAL multirange_name=data_type)?
                (COMMA (SUBTYPE EQUAL subtype_name=data_type
                | SUBTYPE_OPCLASS EQUAL subtype_operator_class=identifier
                | COLLATION EQUAL collation=schema_qualified_name
                | CANONICAL EQUAL canonical_function=schema_qualified_name
                | SUBTYPE_DIFF EQUAL subtype_diff_function=schema_qualified_name
                | MULTIRANGE_TYPE_NAME EQUAL multirange_name=data_type))*
            RIGHT_PAREN)
        | LEFT_PAREN
            // pg_dump prints internallength first
            (INTERNALLENGTH EQUAL (internallength=signed_numerical_literal | VARIABLE) COMMA)?
            INPUT EQUAL input_function=schema_qualified_name COMMA
            OUTPUT EQUAL output_function=schema_qualified_name
            (COMMA (RECEIVE EQUAL receive_function=schema_qualified_name
            | SEND EQUAL send_function=schema_qualified_name
            | TYPMOD_IN EQUAL type_modifier_input_function=schema_qualified_name
            | TYPMOD_OUT EQUAL type_modifier_output_function=schema_qualified_name
            | ANALYZE EQUAL analyze_function=schema_qualified_name
            | SUBSCRIPT EQUAL subscript_function=schema_qualified_name
            | INTERNALLENGTH EQUAL (internallength=signed_numerical_literal | VARIABLE )
            | PASSEDBYVALUE
            | ALIGNMENT EQUAL alignment=data_type
            | STORAGE EQUAL storage=storage_option
            | LIKE EQUAL like_type=data_type
            | CATEGORY EQUAL category=character_string
            | PREFERRED EQUAL preferred=truth_value
            | DEFAULT EQUAL default_value=vex
            | ELEMENT EQUAL element=data_type
            | DELIMITER EQUAL delimiter=character_string
            | COLLATABLE EQUAL collatable=truth_value))*
        RIGHT_PAREN)?
    ;

create_domain_statement
    : dndbt_id=DNDBT_ID_DECLARATION_COMMENT?
    CREATE DOMAIN name=schema_qualified_name AS? dat_type=data_type
    (collate_identifier | DEFAULT def_value=vex | dom_constraint+=domain_constraint)*
    ;

create_server_statement
    : CREATE SERVER if_not_exists? identifier (TYPE type=character_string)? (VERSION version=character_string)?
    FOREIGN DATA WRAPPER identifier
    define_foreign_options?
    ;

create_fts_dictionary_statement
    : CREATE TEXT SEARCH DICTIONARY name=schema_qualified_name
    LEFT_PAREN
        TEMPLATE EQUAL template=schema_qualified_name (COMMA option_with_value)*
    RIGHT_PAREN
    ;

option_with_value
    : identifier EQUAL vex
    ;

create_fts_configuration_statement
    : CREATE TEXT SEARCH CONFIGURATION name=schema_qualified_name
    LEFT_PAREN
        (PARSER EQUAL parser_name=schema_qualified_name
        | COPY EQUAL config_name=schema_qualified_name)
    RIGHT_PAREN
    ;

create_fts_template_statement
    : CREATE TEXT SEARCH TEMPLATE name=schema_qualified_name
    LEFT_PAREN
        (INIT EQUAL init_name=schema_qualified_name COMMA)?
        LEXIZE EQUAL lexize_name=schema_qualified_name
        (COMMA INIT EQUAL init_name=schema_qualified_name)?
    RIGHT_PAREN
    ;

create_fts_parser_statement
    : CREATE TEXT SEARCH PARSER name=schema_qualified_name
    LEFT_PAREN
        START EQUAL start_func=schema_qualified_name COMMA
        GETTOKEN EQUAL gettoken_func=schema_qualified_name COMMA
        END EQUAL end_func=schema_qualified_name COMMA
        (HEADLINE EQUAL headline_func=schema_qualified_name COMMA)?
        LEXTYPES EQUAL lextypes_func=schema_qualified_name
        (COMMA HEADLINE EQUAL headline_func=schema_qualified_name)?
    RIGHT_PAREN
    ;

create_collation_statement
    : CREATE COLLATION if_not_exists? name=schema_qualified_name
    (FROM schema_qualified_name | LEFT_PAREN (collation_option (COMMA collation_option)*)? RIGHT_PAREN)
    ;

alter_collation_statement
    : COLLATION name=schema_qualified_name (REFRESH VERSION | rename_to | owner_to | set_schema)
    ;

collation_option
    : (LOCALE | LC_COLLATE | LC_CTYPE | PROVIDER | VERSION) EQUAL (character_string | identifier)
    | DETERMINISTIC EQUAL boolean_value
    ;

create_user_mapping_statement
    : CREATE USER MAPPING if_not_exists? FOR (user_name | USER) SERVER identifier define_foreign_options?
    ;

alter_user_mapping_statement
    : USER MAPPING FOR (user_name | USER) SERVER identifier define_foreign_options?
    ;

alter_user_or_role_statement
    : (USER | ROLE) (alter_user_or_role_set_reset | identifier rename_to | user_name WITH? user_or_role_option_for_alter+)
    ;

alter_user_or_role_set_reset
    : (user_name | ALL) (IN DATABASE identifier)? set_reset_parameter
    ;

set_reset_parameter
    : SET (identifier DOT)? identifier (TO | EQUAL) set_statement_value
    | SET (identifier DOT)? identifier FROM CURRENT
    | RESET (identifier DOT)? identifier
    | RESET ALL
    ;

alter_group_statement
    : GROUP alter_group_action
    ;

alter_group_action
    : name=identifier rename_to
    | user_name (ADD | DROP) USER identifier_list
    ;

alter_tablespace_statement
    : TABLESPACE name=identifier alter_tablespace_action
    ;

alter_owner_statement
    : (OPERATOR target_operator
        | LARGE OBJECT NUMBER_LITERAL
        | (FUNCTION | PROCEDURE | AGGREGATE) name=schema_qualified_name function_args
        | (TEXT SEARCH DICTIONARY | TEXT SEARCH CONFIGURATION | FOREIGN DATA WRAPPER | SERVER | DOMAIN | SCHEMA | SEQUENCE | TYPE | MATERIALIZED? VIEW)
        if_exists? name=schema_qualified_name) owner_to
    ;

alter_tablespace_action
    : rename_to
    | owner_to
    | SET LEFT_PAREN option_with_value (COMMA option_with_value)* RIGHT_PAREN
    | RESET LEFT_PAREN identifier_list RIGHT_PAREN
    ;

alter_statistics_statement
    : STATISTICS if_exists? name=schema_qualified_name (rename_to | set_schema | owner_to | set_statistics)
    ;

set_statistics
    : SET STATISTICS signed_number_literal
    ;

alter_foreign_data_wrapper
    : FOREIGN DATA WRAPPER name=identifier alter_foreign_data_wrapper_action
    ;

alter_foreign_data_wrapper_action
    : (HANDLER schema_qualified_name | NO HANDLER )? (VALIDATOR schema_qualified_name | NO VALIDATOR)? define_foreign_options?
    | rename_to
    ;

alter_operator_statement
    : OPERATOR target_operator alter_operator_action
    ;

alter_operator_action
    : set_schema
    | SET LEFT_PAREN operator_set_restrict_join (COMMA operator_set_restrict_join)* RIGHT_PAREN
    ;

operator_set_restrict_join
    : (RESTRICT | JOIN) EQUAL schema_qualified_name
    ;

drop_user_mapping_statement
    : USER MAPPING if_exists? FOR (user_name | USER) SERVER identifier
    ;

drop_owned_statement
    : OWNED BY user_name (COMMA user_name)* cascade_restrict?
    ;

drop_operator_statement
    : OPERATOR if_exists? target_operator (COMMA target_operator)* cascade_restrict?
    ;

target_operator
    : name=operator_name LEFT_PAREN (left_type=data_type | NONE) COMMA (right_type=data_type | NONE) RIGHT_PAREN
    ;

domain_constraint
    : dndbt_id=DNDBT_ID_DECLARATION_COMMENT?
        (CONSTRAINT name=identifier)? (CHECK LEFT_PAREN vex RIGHT_PAREN | NOT? NULL)
    ;

create_transform_statement
    : CREATE (OR REPLACE)? TRANSFORM FOR data_type LANGUAGE identifier
    LEFT_PAREN
        FROM SQL WITH FUNCTION function_parameters COMMA
        TO SQL WITH FUNCTION function_parameters
    RIGHT_PAREN
    ;

create_access_method_statement
    : CREATE access_method_definition
    ;

access_method_definition
    : ACCESS METHOD identifier TYPE (TABLE | INDEX) HANDLER schema_qualified_name
    ;

create_user_or_role_statement
    : CREATE (USER | ROLE) name=identifier (WITH? user_or_role_option user_or_role_option*)?
    ;

user_or_role_option
    : user_or_role_or_group_common_option
    | user_or_role_common_option
    | user_or_role_or_group_option_for_create
    ;

user_or_role_option_for_alter
    : user_or_role_or_group_common_option
    | user_or_role_common_option
    ;

user_or_role_or_group_common_option
    : SUPERUSER | NOSUPERUSER
    | CREATEDB | NOCREATEDB
    | CREATEROLE | NOCREATEROLE
    | INHERIT | NOINHERIT
    | LOGIN | NOLOGIN
    | ENCRYPTED? PASSWORD (password=Character_String_Literal | NULL)
    | VALID UNTIL date_time=Character_String_Literal
    ;

user_or_role_common_option
    : REPLICATION | NOREPLICATION
    | BYPASSRLS | NOBYPASSRLS
    | CONNECTION LIMIT signed_number_literal
    ;

user_or_role_or_group_option_for_create
    : SYSID vex
    | (IN ROLE | IN GROUP | ROLE | ADMIN | USER) identifier_list
    ;

create_group_statement
    : CREATE GROUP name=identifier (WITH? group_option+)?
    ;

group_option
    : user_or_role_or_group_common_option
    | user_or_role_or_group_option_for_create
    ;

create_tablespace_statement
    : CREATE TABLESPACE name=identifier (OWNER user_name)?
    LOCATION directory=Character_String_Literal
    (WITH LEFT_PAREN option_with_value (COMMA option_with_value)* RIGHT_PAREN)?
    ;

create_statistics_statement
    : CREATE STATISTICS if_not_exists? name=schema_qualified_name
    (LEFT_PAREN identifier_list RIGHT_PAREN)?
    ON identifier COMMA identifier_list
    FROM schema_qualified_name
    ;

create_foreign_data_wrapper_statement
    : CREATE FOREIGN DATA WRAPPER name=identifier (HANDLER handler_func=schema_qualified_name | NO HANDLER )?
    (VALIDATOR validator_func=schema_qualified_name | NO VALIDATOR)?
     define_foreign_options?
    ;

create_operator_statement
    : CREATE OPERATOR name=operator_name LEFT_PAREN operator_option (COMMA operator_option)* RIGHT_PAREN
    ;

operator_name
    : (schema_name=identifier DOT)? operator=all_simple_op
    ;

operator_option
    : (FUNCTION | PROCEDURE) EQUAL func_name=schema_qualified_name
    | RESTRICT EQUAL restr_name=schema_qualified_name
    | JOIN EQUAL join_name=schema_qualified_name
    | (LEFTARG | RIGHTARG) EQUAL type=data_type
    | (COMMUTATOR | NEGATOR) EQUAL addition_oper_name=all_op_ref
    | SORT1
    | SORT2
    | LTCMP
    | GTCMP
    | HASHES
    | MERGES
    ;

create_aggregate_statement
    : CREATE (OR REPLACE)? AGGREGATE name=schema_qualified_name function_args? LEFT_PAREN
    (BASETYPE EQUAL base_type=data_type COMMA)?
    aggregate_required_param
    COMMA aggregate_required_param
    (COMMA aggregate_required_param)?
    (COMMA aggregate_param)*
    RIGHT_PAREN
    ;

aggregate_required_param
    : (SFUNC | SFUNC1 | SFUNC2) EQUAL sfunc_name=schema_qualified_name function_args?
    | (STYPE | STYPE1 | STYPE2) EQUAL type=data_type
    | BASETYPE EQUAL (data_type | Character_String_Literal)
    ;

aggregate_param
    : SSPACE EQUAL s_space=NUMBER_LITERAL
    | FINALFUNC EQUAL final_func=schema_qualified_name function_args?
    | FINALFUNC_EXTRA (EQUAL (TRUE | FALSE))?
    | FINALFUNC_MODIFY EQUAL (READ_ONLY | SHAREABLE | READ_WRITE)
    | COMBINEFUNC EQUAL combine_func=schema_qualified_name function_args?
    | SERIALFUNC EQUAL serial_func=schema_qualified_name function_args?
    | DESERIALFUNC EQUAL deserial_func=schema_qualified_name function_args?
    | (INITCOND | INITCOND1 | INITCOND2) EQUAL init_cond=vex
    | MSFUNC EQUAL ms_func=schema_qualified_name function_args?
    | MINVFUNC EQUAL minv_func=schema_qualified_name function_args?
    | MSTYPE EQUAL ms_type=data_type
    | MSSPACE EQUAL ms_space=NUMBER_LITERAL
    | MFINALFUNC EQUAL mfinal_func=schema_qualified_name function_args?
    | MFINALFUNC_EXTRA (EQUAL (TRUE | FALSE))?
    | MFINALFUNC_MODIFY EQUAL (READ_ONLY | SHAREABLE | READ_WRITE)
    | MINITCOND EQUAL minit_cond=vex
    | SORTOP EQUAL all_op_ref
    | PARALLEL EQUAL (SAFE | RESTRICTED | UNSAFE)
    | HYPOTHETICAL (EQUAL (TRUE | FALSE))?
    ;

set_statement
    : SET set_action
    ;

set_action
    : CONSTRAINTS (ALL | names_references) (DEFERRED | IMMEDIATE)
    | TRANSACTION transaction_mode (COMMA transaction_mode)*
    | TRANSACTION SNAPSHOT Character_String_Literal
    | SESSION CHARACTERISTICS AS TRANSACTION transaction_mode (COMMA transaction_mode)*
    | (SESSION | LOCAL)? session_local_option
    | XML OPTION (DOCUMENT | CONTENT)
    ;

session_local_option
    : SESSION AUTHORIZATION (Character_String_Literal | identifier | DEFAULT)
    | TIME ZONE (Character_String_Literal | signed_numerical_literal | LOCAL | DEFAULT)
    | (identifier DOT)? config_param=identifier (TO | EQUAL) set_statement_value
    | ROLE (identifier | NONE)
    ;

set_statement_value
    : vex (COMMA vex)*
    | DEFAULT
    ;

create_rewrite_statement
    : CREATE (OR REPLACE)? RULE name=identifier AS ON event=(SELECT | INSERT | DELETE | UPDATE)
     TO table_name=schema_qualified_name (WHERE vex)? DO (ALSO | INSTEAD)?
     (NOTHING
        | rewrite_command
        | (LEFT_PAREN (rewrite_command SEMI_COLON)* rewrite_command SEMI_COLON? RIGHT_PAREN)
     )
    ;

rewrite_command
    : select_stmt
    | insert_stmt
    | update_stmt
    | delete_stmt
    | notify_stmt
    ;

create_trigger_statement
    : dndbt_id=DNDBT_ID_DECLARATION_COMMENT?
    CREATE (OR REPLACE)? CONSTRAINT? TRIGGER name=identifier (before_true=BEFORE | (INSTEAD OF) | AFTER)
    (((insert_true=INSERT | delete_true=DELETE | truncate_true=TRUNCATE)
      | update_true=UPDATE (OF identifier_list)?)OR?)+
    ON table_name=schema_qualified_name
    (FROM referenced_table_name=schema_qualified_name)?
    table_deferrable? table_initialy_immed?
    (REFERENCING trigger_referencing trigger_referencing?)?
    (for_each_true=FOR EACH? (ROW | STATEMENT))?
    when_trigger?
    EXECUTE (FUNCTION | PROCEDURE) func_name=function_call
    ;

trigger_referencing
    : (OLD | NEW) TABLE AS? identifier
    ;

when_trigger
    : WHEN LEFT_PAREN vex RIGHT_PAREN
    ;

rule_common
    : GRANT (permissions | columns_permissions) ON rule_member_object TO roles_names (WITH GRANT OPTION)? granted_by?
    | REVOKE grant_option_for? (permissions | columns_permissions) ON rule_member_object FROM roles_names granted_by? cascade_restrict?
    | other_rules
    ;

rule_member_object
    : TABLE? table_names=names_references
    | SEQUENCE names_references
    | DATABASE names_references
    | DOMAIN names_references
    | FOREIGN DATA WRAPPER names_references
    | FOREIGN SERVER names_references
    | (FUNCTION | PROCEDURE | ROUTINE) func_name+=function_parameters (COMMA func_name+=function_parameters)*
    | LARGE OBJECT NUMBER_LITERAL (COMMA NUMBER_LITERAL)*
    | LANGUAGE names_references
    | SCHEMA schema_names=names_references
    | TABLESPACE names_references
    | TYPE names_references
    | ALL (TABLES | SEQUENCES | FUNCTIONS | PROCEDURES | ROUTINES) IN SCHEMA names_references
    ;

columns_permissions
    : table_column_privileges (COMMA table_column_privileges)*
    ;

table_column_privileges
    : table_column_privilege LEFT_PAREN identifier_list RIGHT_PAREN
    ;

permissions
    : permission (COMMA permission)*
    ;

permission
    : ALL PRIVILEGES?
    | CONNECT
    | CREATE
    | DELETE
    | EXECUTE
    | INSERT
    | UPDATE
    | REFERENCES
    | SELECT
    | TEMP
    | TRIGGER
    | TRUNCATE
    | USAGE
    ;

other_rules
    : GRANT names_references TO roles_names (WITH ADMIN OPTION)? granted_by?
    | REVOKE (ADMIN OPTION FOR)? names_references FROM roles_names granted_by? cascade_restrict?
    ;

grant_to_rule
    : TO roles_names (WITH GRANT OPTION)?
    ;

revoke_from_cascade_restrict
    : FROM roles_names cascade_restrict?
    ;

roles_names
    : role_name_with_group (COMMA role_name_with_group)*
    ;

role_name_with_group
    : GROUP? user_name
    ;

granted_by
    : GRANTED BY role_name_with_group
    ;

comment_on_statement
    : COMMENT ON comment_member_object IS (character_string | NULL)
    ;

security_label
    : SECURITY LABEL (FOR (identifier | character_string))? ON label_member_object IS (character_string | NULL)
    ;

comment_member_object
    : ACCESS METHOD identifier
    | (AGGREGATE | PROCEDURE | FUNCTION | ROUTINE) name=schema_qualified_name function_args
    | CAST LEFT_PAREN cast_name RIGHT_PAREN
    | COLLATION identifier
    | COLUMN name=schema_qualified_name
    | CONSTRAINT identifier ON DOMAIN? table_name=schema_qualified_name
    | CONVERSION name=schema_qualified_name
    | DATABASE identifier
    | DOMAIN name=schema_qualified_name
    | EXTENSION identifier
    | EVENT TRIGGER identifier
    | FOREIGN DATA WRAPPER identifier
    | FOREIGN? TABLE name=schema_qualified_name
    | INDEX name=schema_qualified_name
    | LARGE OBJECT NUMBER_LITERAL
    | MATERIALIZED? VIEW name=schema_qualified_name
    | OPERATOR target_operator
    | OPERATOR (FAMILY| CLASS) name=schema_qualified_name USING index_method=identifier
    | POLICY identifier ON table_name=schema_qualified_name
    | PROCEDURAL? LANGUAGE name=schema_qualified_name
    | PUBLICATION identifier
    | ROLE identifier
    | RULE identifier ON table_name=schema_qualified_name
    | SCHEMA identifier
    | SEQUENCE name=schema_qualified_name
    | SERVER identifier
    | STATISTICS name=schema_qualified_name
    | SUBSCRIPTION identifier
    | TABLESPACE identifier
    | TEXT SEARCH CONFIGURATION name=schema_qualified_name
    | TEXT SEARCH DICTIONARY name=schema_qualified_name
    | TEXT SEARCH PARSER name=schema_qualified_name
    | TEXT SEARCH TEMPLATE name=schema_qualified_name
    | TRANSFORM FOR name=schema_qualified_name LANGUAGE identifier
    | TRIGGER identifier ON table_name=schema_qualified_name
    | TYPE name=schema_qualified_name
    ;

label_member_object
    : (AGGREGATE | PROCEDURE | FUNCTION | ROUTINE) schema_qualified_name function_args
    | COLUMN schema_qualified_name
    | DATABASE identifier
    | DOMAIN schema_qualified_name
    | EVENT TRIGGER identifier
    | FOREIGN? TABLE schema_qualified_name
    | LARGE OBJECT NUMBER_LITERAL
    | MATERIALIZED? VIEW schema_qualified_name
    | PROCEDURAL? LANGUAGE schema_qualified_name
    | PUBLICATION identifier
    | ROLE identifier
    | SCHEMA identifier
    | SEQUENCE schema_qualified_name
    | SUBSCRIPTION identifier
    | TABLESPACE identifier
    | TYPE schema_qualified_name
    ;

/*
===============================================================================
  Function and Procedure Definition
===============================================================================
*/
create_function_statement
    : dndbt_id=DNDBT_ID_DECLARATION_COMMENT?
    CREATE (OR REPLACE)? (FUNCTION | PROCEDURE) function_parameters
    (RETURNS (rettype_data=data_type_for_func_args_or_retval | ret_table=function_ret_table))?
    (function_actions_common+ with_storage_parameter? | function_actions_common* function_body)
    ;

transform_for_type
    : FOR TYPE data_type
    ;

function_ret_table
    : TABLE LEFT_PAREN function_column_name_type (COMMA function_column_name_type)* RIGHT_PAREN
    ;

function_column_name_type
    : identifier data_type
    ;

function_parameters
    : schema_qualified_name function_args
    ;

function_args
    : LEFT_PAREN ((function_argument (COMMA function_argument)*)? agg_order? | MULTIPLY) RIGHT_PAREN
    ;

agg_order
    : ORDER BY function_argument (COMMA function_argument)*
    ;

function_body
    : function_return
    | BEGIN ATOMIC SEMI_COLON* ((statement | function_return) SEMI_COLON+)* END
    ;

function_return
    : RETURN vex
    ;

character_string
    : BeginDollarStringConstant Text_between_Dollar* EndDollarStringConstant
    | Character_String_Literal
    ;

function_argument
    : (argmode? identifier_nontype? | identifier_nontype? argmode?) data_type_for_func_args_or_retval ((DEFAULT | EQUAL) vex)?
    ;

argmode
    : IN | OUT | INOUT | VARIADIC
    ;

create_sequence_statement
    : dndbt_id=DNDBT_ID_DECLARATION_COMMENT?
        CREATE ((TEMPORARY | TEMP) | UNLOGGED)? SEQUENCE if_not_exists? name=schema_qualified_name (sequence_body)*
    ;

sequence_body
    : AS type=(SMALLINT | INTEGER | BIGINT | INT)
    | SEQUENCE NAME name=schema_qualified_name
    | INCREMENT BY? incr=signed_numerical_literal
    | (MINVALUE minval=signed_numerical_literal | NO MINVALUE)
    | (MAXVALUE maxval=signed_numerical_literal | NO MAXVALUE)
    | START WITH? start_val=signed_numerical_literal
    | RESTART (WITH? signed_number_literal)?
    | CACHE cache_val=signed_numerical_literal
    | cycle_true=NO? cycle_val=CYCLE
    | OWNED BY owned_by=schema_qualified_name
    ;

signed_number_literal
    : sign? NUMBER_LITERAL
    ;

signed_numerical_literal
    : sign? unsigned_numeric_literal
    ;

sign
    : PLUS | MINUS
    ;

create_schema_statement
    : CREATE SCHEMA if_not_exists? name=identifier? (AUTHORIZATION user_name)? create_schema_element*
    ;

create_schema_element
    : create_index_statement
    | create_sequence_statement
    | create_table_as_statement
    | create_table_statement
    | create_trigger_statement
    | create_view_statement
    ;

create_policy_statement
    : CREATE POLICY identifier ON schema_qualified_name
    (AS (PERMISSIVE | RESTRICTIVE))?
    (FOR event=(ALL | SELECT | INSERT | UPDATE | DELETE))?
    (TO user_name (COMMA user_name)*)?
    (USING using=vex)? (WITH CHECK check=vex)?
    ;

alter_policy_statement
    : POLICY identifier ON schema_qualified_name rename_to
    | POLICY identifier ON schema_qualified_name (TO user_name (COMMA user_name)*)? (USING vex)? (WITH CHECK vex)?
    ;

drop_policy_statement
    : POLICY if_exists? identifier ON schema_qualified_name cascade_restrict?
    ;

create_subscription_statement
    : CREATE SUBSCRIPTION identifier
    CONNECTION Character_String_Literal
    PUBLICATION identifier_list
    with_storage_parameter?
    ;

alter_subscription_statement
    : SUBSCRIPTION identifier alter_subscription_action
    ;

alter_subscription_action
    : CONNECTION character_string
    | (ADD | DROP | SET) PUBLICATION identifier_list with_storage_parameter?
    | REFRESH PUBLICATION with_storage_parameter?
    | ENABLE
    | DISABLE
    | SET storage_parameter
    | owner_to
    | rename_to
    ;

create_cast_statement
    : CREATE CAST LEFT_PAREN cast_name RIGHT_PAREN
    (WITH FUNCTION func_name=schema_qualified_name function_args | WITHOUT FUNCTION | WITH INOUT)
    (AS ASSIGNMENT | AS IMPLICIT)?
    ;

cast_name
    : source=data_type AS target=data_type
    ;

drop_cast_statement
    : CAST if_exists? LEFT_PAREN cast_name RIGHT_PAREN cascade_restrict?
    ;

create_operator_family_statement
    : CREATE OPERATOR FAMILY schema_qualified_name USING identifier
    ;

alter_operator_family_statement
    : OPERATOR FAMILY schema_qualified_name USING identifier operator_family_action
    ;

operator_family_action
    : rename_to
    | owner_to
    | set_schema
    | ADD add_operator_to_family (COMMA add_operator_to_family)*
    | DROP drop_operator_from_family (COMMA drop_operator_from_family)*
    ;

add_operator_to_family
    : OPERATOR unsigned_numeric_literal target_operator (FOR SEARCH | FOR ORDER BY schema_qualified_name)?
    | FUNCTION unsigned_numeric_literal (LEFT_PAREN (data_type | NONE) (COMMA (data_type | NONE))? RIGHT_PAREN)? function_call
    ;

drop_operator_from_family
    : (OPERATOR | FUNCTION) unsigned_numeric_literal LEFT_PAREN (data_type | NONE) (COMMA (data_type | NONE))? RIGHT_PAREN
    ;

drop_operator_family_statement
    : OPERATOR FAMILY if_exists? schema_qualified_name USING identifier cascade_restrict?
    ;

create_operator_class_statement
    : CREATE OPERATOR CLASS schema_qualified_name DEFAULT? FOR TYPE data_type
    USING identifier (FAMILY schema_qualified_name)? AS
    create_operator_class_option (COMMA create_operator_class_option)*
    ;

create_operator_class_option
    : OPERATOR unsigned_numeric_literal name=operator_name
        (LEFT_PAREN (data_type | NONE) COMMA (data_type | NONE) RIGHT_PAREN)?
        (FOR SEARCH | FOR ORDER BY schema_qualified_name)?
    | FUNCTION unsigned_numeric_literal
        (LEFT_PAREN (data_type | NONE) (COMMA (data_type | NONE))? RIGHT_PAREN)?
        function_call
    | STORAGE data_type
    ;

alter_operator_class_statement
    : OPERATOR CLASS schema_qualified_name USING identifier (rename_to | owner_to | set_schema)
    ;

drop_operator_class_statement
    : OPERATOR CLASS if_exists? schema_qualified_name USING identifier cascade_restrict?
    ;

create_conversion_statement
    : CREATE DEFAULT? CONVERSION schema_qualified_name FOR Character_String_Literal TO Character_String_Literal FROM schema_qualified_name
    ;

alter_conversion_statement
    : CONVERSION schema_qualified_name (rename_to | owner_to | set_schema)
    ;

create_publication_statement
    : CREATE PUBLICATION identifier
    (FOR publication_object (COMMA publication_object)* | FOR ALL TABLES)?
    with_storage_parameter?
    ;

publication_object
    : TABLE only_table_multiply (COMMA only_table_multiply)*
    | TABLES IN SCHEMA (schema_qualified_name | CURRENT_SCHEMA) (COMMA schema_qualified_name | CURRENT_SCHEMA)*
    ;

alter_publication_statement
    : PUBLICATION identifier alter_publication_action
    ;

alter_publication_action
    : rename_to
    | owner_to
    | SET storage_parameter
    | (ADD | DROP | SET) TABLE only_table_multiply (COMMA only_table_multiply)*
    ;

only_table_multiply
    : ONLY? schema_qualified_name MULTIPLY?
    ;

alter_trigger_statement
    : TRIGGER identifier ON schema_qualified_name (rename_to | NO? DEPENDS ON EXTENSION identifier)
    ;

alter_rule_statement
    : RULE identifier ON schema_qualified_name rename_to
    ;

copy_statement
    : copy_to_statement
    | copy_from_statement
    ;

copy_from_statement
    : COPY table_cols
    FROM (PROGRAM? Character_String_Literal | STDIN)
    (WITH? (LEFT_PAREN copy_option_list RIGHT_PAREN | copy_option_list))?
    (WHERE vex)?
    ;

copy_to_statement
    : COPY (table_cols | LEFT_PAREN data_statement RIGHT_PAREN)
    TO (PROGRAM? Character_String_Literal | STDOUT)
    (WITH? (LEFT_PAREN copy_option_list RIGHT_PAREN | copy_option_list))?
    ;

copy_option_list
    : copy_option (COMMA? copy_option)*
    ;

copy_option
    : FORMAT? (TEXT | CSV | BINARY)
    | OIDS truth_value?
    | FREEZE truth_value?
    | DELIMITER AS? Character_String_Literal
    | NULL AS? Character_String_Literal
    | HEADER truth_value?
    | QUOTE Character_String_Literal
    | ESCAPE Character_String_Literal
    | FORCE QUOTE (MULTIPLY | identifier_list)
    | FORCE_QUOTE (MULTIPLY | LEFT_PAREN identifier_list RIGHT_PAREN)
    | FORCE NOT NULL identifier_list
    | FORCE_NOT_NULL LEFT_PAREN identifier_list RIGHT_PAREN
    | FORCE_NULL LEFT_PAREN identifier_list RIGHT_PAREN
    | ENCODING Character_String_Literal
    ;

create_view_statement
    : dndbt_id=DNDBT_ID_DECLARATION_COMMENT?
    CREATE (OR REPLACE)? (TEMP | TEMPORARY)? RECURSIVE? MATERIALIZED? VIEW
    if_not_exists? name=schema_qualified_name column_names=view_columns?
    (USING identifier)?
    (WITH storage_parameter)?
    table_space?
    AS v_query=select_stmt
    with_check_option?
    (WITH NO? DATA)?
    ;

if_exists
    : IF EXISTS
    ;

if_not_exists
    : IF NOT EXISTS
    ;

view_columns
    : LEFT_PAREN identifier (COMMA identifier)* RIGHT_PAREN
    ;

merge_columns
    : LEFT_PAREN identifier (COMMA identifier)* RIGHT_PAREN
    ;

with_check_option
    : WITH (CASCADED|LOCAL)? CHECK OPTION
    ;

create_database_statement
    : CREATE DATABASE identifier (WITH? create_database_option+)?
    ;

create_database_option
    : (OWNER | TEMPLATE | ENCODING | LOCALE | LC_COLLATE | LC_CTYPE | TABLESPACE) EQUAL? (character_string | identifier | DEFAULT)
    | alter_database_option
    ;

alter_database_statement
    : DATABASE identifier alter_database_action?
    ;

alter_database_action
    : WITH? alter_database_option+
    | WITH? TABLESPACE EQUAL? (character_string | identifier | DEFAULT)
    | rename_to
    | owner_to
    | set_tablespace
    | REFRESH COLLATION VERSION
    | set_reset_parameter
    ;

alter_database_option
    : (ALLOW_CONNECTIONS | IS_TEMPLATE) EQUAL? (boolean_value | DEFAULT)
    | CONNECTION LIMIT EQUAL? (signed_number_literal | DEFAULT)
    ;

create_table_statement
    : dndbt_id=DNDBT_ID_DECLARATION_COMMENT?
    CREATE ((GLOBAL | LOCAL)? (TEMPORARY | TEMP) | UNLOGGED)? TABLE if_not_exists? name=schema_qualified_name
    define_table
    partition_by?
    (USING identifier)?
    storage_parameter_oid?
    on_commit?
    table_space?
    ;

create_table_as_statement
    : CREATE ((GLOBAL | LOCAL)? (TEMPORARY | TEMP) | UNLOGGED)? TABLE if_not_exists? name=schema_qualified_name
    names_in_parens?
    (USING identifier)?
    storage_parameter_oid?
    on_commit?
    table_space?
    AS (select_stmt | execute_statement)
    (WITH NO? DATA)?
    ;

create_foreign_table_statement
    : CREATE FOREIGN TABLE if_not_exists? name=schema_qualified_name
    (define_table_items | define_partition)
    define_server
    ;

define_table
    : define_table_items
    | define_type
    | define_partition
    ;

define_partition
    : PARTITION OF parent_table=schema_qualified_name
    list_of_type_column_def?
    for_values_bound
    ;

for_values_bound
    : FOR VALUES partition_bound_spec
    | DEFAULT
    ;

partition_bound_spec
    : IN LEFT_PAREN vex (COMMA vex)* RIGHT_PAREN
    | FROM partition_bound_part TO partition_bound_part
    | WITH LEFT_PAREN MODULUS NUMBER_LITERAL COMMA REMAINDER NUMBER_LITERAL RIGHT_PAREN
    ;

partition_bound_part
    : LEFT_PAREN vex (COMMA vex)* RIGHT_PAREN
    ;

define_table_items
    : LEFT_PAREN (table_item_definition (COMMA table_item_definition)*)? RIGHT_PAREN (INHERITS names_in_parens)?
    ;

define_type
    : OF type_name=data_type list_of_type_column_def?
    ;

partition_by
    : PARTITION BY partition_method
    ;

partition_method
    : (RANGE | LIST | HASH) LEFT_PAREN partition_column (COMMA partition_column)* RIGHT_PAREN
    ;

partition_column
    : vex identifier?
    ;

define_server
    : SERVER identifier define_foreign_options?
    ;

define_foreign_options
    : OPTIONS LEFT_PAREN (foreign_option (COMMA foreign_option)*) RIGHT_PAREN
    ;

foreign_option
    : (ADD | SET | DROP)? foreign_option_name character_string?
    ;

foreign_option_name
    : identifier
    | USER
    ;

list_of_type_column_def
    : LEFT_PAREN (table_of_type_column_def (COMMA table_of_type_column_def)*) RIGHT_PAREN
    ;

table_item_definition
    : dndbt_id=DNDBT_ID_DECLARATION_COMMENT?
        table_column_definition
    | dndbt_id=DNDBT_ID_DECLARATION_COMMENT?
        tabl_constraint=constraint_common
    | LIKE schema_qualified_name like_option*
    ;

table_of_type_column_def
    : identifier (WITH OPTIONS)? collate_identifier? constraint_common*
    | tabl_constraint=constraint_common
    ;

table_column_definition
    : identifier data_type define_foreign_options? compression_identifier? collate_identifier? constraint_common*
    ;

like_option
    : (INCLUDING | EXCLUDING) (COMMENTS | COMPRESSION | CONSTRAINTS | DEFAULTS | GENERATED | IDENTITY | INDEXES | STORAGE | ALL)
    ;
/** NULL, DEFAULT - column constraint
* EXCLUDE, FOREIGN KEY - table_constraint
*/
constraint_common
    : (CONSTRAINT identifier)? constr_body table_deferrable? table_initialy_immed?
    ;

constr_body
    : EXCLUDE (USING index_method=identifier)?
            LEFT_PAREN index_column WITH all_op (COMMA index_column WITH all_op)* RIGHT_PAREN
            index_parameters (where=WHERE exp=vex)?
    | (FOREIGN KEY cols=names_in_parens)? REFERENCES schema_qualified_name refcols=names_in_parens?
        fk_action_clause* refcols=names_in_parens?
    | CHECK LEFT_PAREN expression=vex RIGHT_PAREN (NO INHERIT)?
    | NOT? NULL
    | (UNIQUE | PRIMARY KEY) cols=names_in_parens? index_parameters
    | DEFAULT default_expr=vex
    | identity_body
    | GENERATED ALWAYS AS LEFT_PAREN vex RIGHT_PAREN STORED
    ;

all_op
    : op
    | EQUAL | NOT_EQUAL | LTH | LEQ | GTH | GEQ
    | PLUS | MINUS | MULTIPLY | DIVIDE | MODULAR | EXP
    ;

all_simple_op
    : op_chars
    | EQUAL | NOT_EQUAL | LTH | LEQ | GTH | GEQ
    | PLUS | MINUS | MULTIPLY | DIVIDE | MODULAR | EXP
    ;

op_chars
    : OP_CHARS
    | LESS_LESS
    | GREATER_GREATER
    | HASH_SIGN
    ;

index_parameters
    : including_index? with_storage_parameter? (USING INDEX (table_space | schema_qualified_name))?
    ;

names_in_parens
    : LEFT_PAREN names_references RIGHT_PAREN
    ;

names_references
    : schema_qualified_name (COMMA schema_qualified_name)*
    ;

storage_parameter
    : LEFT_PAREN storage_parameter_option (COMMA storage_parameter_option)* RIGHT_PAREN
    ;

storage_parameter_option
    : storage_parameter_name (EQUAL vex)?
    ;

storage_parameter_name
    : col_label (DOT col_label)?
    ;

with_storage_parameter
    : WITH storage_parameter
    ;

storage_parameter_oid
    : with_storage_parameter | (WITH OIDS) | (WITHOUT OIDS)
    ;

on_commit
    : ON COMMIT (PRESERVE ROWS | DELETE ROWS | DROP)
    ;

table_space
    : TABLESPACE identifier
    ;

set_tablespace
    : SET TABLESPACE identifier NOWAIT?
    ;

fk_action_clause
    : (MATCH (FULL | PARTIAL | SIMPLE) | ON (DELETE | UPDATE) fk_action)
    ;

fk_action
    : cascade_restrict
    | SET (NULL | DEFAULT)
    | NO ACTION
    ;

owner_to
    : OWNER TO user_name
    ;

rename_to
    : RENAME TO name=identifier
    ;

set_schema
    : SET SCHEMA identifier
    ;

table_column_privilege
    : SELECT | INSERT | UPDATE | DELETE | TRUNCATE | REFERENCES | TRIGGER | ALL PRIVILEGES?
    ;

usage_select_update
    : USAGE | SELECT | UPDATE
    ;

partition_by_columns
    : PARTITION BY vex (COMMA vex)*
    ;

cascade_restrict
    : CASCADE | RESTRICT
    ;

compression_identifier
    : COMPRESSION (compression_method=identifier | DEFAULT)
    ;

collate_identifier
    : COLLATE collation=schema_qualified_name
    ;

indirection_var
    : (identifier | dollar_number) indirection_list?
    ;

dollar_number
    : DOLLAR_NUMBER
    ;

indirection_list
    : indirection+
    | indirection* DOT MULTIPLY
    ;

indirection
    : DOT col_label
    | LEFT_BRACKET vex RIGHT_BRACKET
    | LEFT_BRACKET vex? COLON vex? RIGHT_BRACKET
    ;

/*
===============================================================================
  11.21 <data types>
===============================================================================
*/

drop_database_statement
    : DATABASE if_exists? identifier (WITH? LEFT_PAREN FORCE RIGHT_PAREN)?
    ;

drop_function_statement
    : (FUNCTION | PROCEDURE | ROUTINE | AGGREGATE) if_exists?
        name+=schema_qualified_name function_args?
        (COMMA name+=schema_qualified_name function_args?)*
        cascade_restrict?
    ;

drop_trigger_statement
    : TRIGGER if_exists? name=identifier ON table_name=schema_qualified_name cascade_restrict?
    ;

drop_rule_statement
    : RULE if_exists? name=identifier ON schema_qualified_name cascade_restrict?
    ;

drop_statements
    : (ACCESS METHOD
    | COLLATION
    | CONVERSION
    | DOMAIN
    | EVENT TRIGGER
    | EXTENSION
    | GROUP
    | FOREIGN? TABLE
    | FOREIGN DATA WRAPPER
    | INDEX CONCURRENTLY?
    | MATERIALIZED? VIEW
    | PROCEDURAL? LANGUAGE
    | PUBLICATION
    | ROLE
    | SCHEMA
    | SEQUENCE
    | SERVER
    | STATISTICS
    | SUBSCRIPTION
    | TABLESPACE
    | TYPE
    | TEXT SEARCH (CONFIGURATION | DICTIONARY | PARSER | TEMPLATE)
    | USER) if_exist_names_restrict_cascade
    ;

if_exist_names_restrict_cascade
    : if_exists? names_references cascade_restrict?
    ;
/*
===============================================================================
  5.2 <token and separator>

  Specifying lexical units (tokens and separators) that participate in SQL language
===============================================================================
*/

id_token
  : Identifier | QuotedIdentifier | tokens_nonkeyword;

/*
  old rule for default old identifier behavior
  includes types
*/
identifier
    : id_token
    | tokens_nonreserved
    | tokens_nonreserved_except_function_type
    ;

identifier_for_func_name
    : id_token
    | tokens_nonreserved
    | tokens_reserved_except_function_type
    | tokens_nonreserved_for_func_name
    ;

identifier_nontype
    : id_token
    | tokens_nonreserved
    | tokens_reserved_except_function_type
    ;

col_label
    : id_token
    | tokens_reserved
    | tokens_nonreserved
    | tokens_reserved_except_function_type
    | tokens_nonreserved_except_function_type
    ;

bare_col_label
    : bare_label_keyword
    | id_token
    ;

/*
 * These rules should be generated using code in the Keyword class.
 * Word tokens that are not keywords should be added to nonkeyword list (tokens_nonkeyword).
 */
bare_label_keyword
    : ABORT
    | ABSOLUTE
    | ACCESS
    | ACTION
    | ADD
    | ADMIN
    | AFTER
    | AGGREGATE
    | ALL
    | ALSO
    | ALTER
    | ALWAYS
    | ANALYZE
    | AND
    | ANY
    | ASC
    | ASENSITIVE
    | ASSERTION
    | ASSIGNMENT
    | ASYMMETRIC
    | AT
    | ATOMIC
    | ATTACH
    | ATTRIBUTE
    | AUTHORIZATION
    | BACKWARD
    | BEFORE
    | BEGIN
    | BETWEEN
    | BIGINT
    | BINARY
    | BIT
    | BOOLEAN
    | BOTH
    | BREADTH
    | BY
    | CACHE
    | CALL
    | CALLED
    | CASCADE
    | CASCADED
    | CASE
    | CAST
    | CATALOG
    | CHAIN
    | CHARACTERISTICS
    | CHECK
    | CHECKPOINT
    | CLASS
    | CLOSE
    | CLUSTER
    | COALESCE
    | COLLATE
    | COLLATION
    | COLUMN
    | COLUMNS
    | COMMENT
    | COMMENTS
    | COMMIT
    | COMMITTED
    | COMPRESSION
    | CONCURRENTLY
    | CONFIGURATION
    | CONFLICT
    | CONNECTION
    | CONSTRAINT
    | CONSTRAINTS
    | CONTENT
    | CONTINUE
    | CONVERSION
    | COPY
    | COST
    | CROSS
    | CSV
    | CUBE
    | CURRENT
    | CURRENT_CATALOG
    | CURRENT_DATE
    | CURRENT_ROLE
    | CURRENT_SCHEMA
    | CURRENT_TIME
    | CURRENT_TIMESTAMP
    | CURRENT_USER
    | CURSOR
    | CYCLE
    | DATA
    | DATABASE
    | DEALLOCATE
    | DEC
    | DECIMAL
    | DECLARE
    | DEFAULT
    | DEFAULTS
    | DEFERRABLE
    | DEFERRED
    | DEFINER
    | DELETE
    | DELIMITER
    | DELIMITERS
    | DEPENDS
    | DEPTH
    | DESC
    | DETACH
    | DICTIONARY
    | DISABLE
    | DISCARD
    | DISTINCT
    | DO
    | DOCUMENT
    | DOMAIN
    | DOUBLE
    | DROP
    | EACH
    | ELSE
    | ENABLE
    | ENCODING
    | ENCRYPTED
    | END
    | ENUM
    | ESCAPE
    | EVENT
    | EXCLUDE
    | EXCLUDING
    | EXCLUSIVE
    | EXECUTE
    | EXISTS
    | EXPLAIN
    | EXPRESSION
    | EXTENSION
    | EXTERNAL
    | EXTRACT
    | FALSE
    | FAMILY
    | FINALIZE
    | FIRST
    | FLOAT
    | FOLLOWING
    | FORCE
    | FOREIGN
    | FORWARD
    | FREEZE
    | FULL
    | FUNCTION
    | FUNCTIONS
    | GENERATED
    | GLOBAL
    | GRANTED
    | GREATEST
    | GROUPING
    | GROUPS
    | HANDLER
    | HEADER
    | HOLD
    | IDENTITY
    | IF
    | ILIKE
    | IMMEDIATE
    | IMMUTABLE
    | IMPLICIT
    | IMPORT
    | IN
    | INCLUDE
    | INCLUDING
    | INCREMENT
    | INDEX
    | INDEXES
    | INHERIT
    | INHERITS
    | INITIALLY
    | INLINE
    | INNER
    | INOUT
    | INPUT
    | INSENSITIVE
    | INSERT
    | INSTEAD
    | INT
    | INTEGER
    | INTERVAL
    | INVOKER
    | IS
    | ISOLATION
    | JOIN
    | KEY
    | LABEL
    | LANGUAGE
    | LARGE
    | LAST
    | LATERAL
    | LEADING
    | LEAKPROOF
    | LEAST
    | LEFT
    | LEVEL
    | LIKE
    | LISTEN
    | LOAD
    | LOCAL
    | LOCALTIME
    | LOCALTIMESTAMP
    | LOCATION
    | LOCK
    | LOCKED
    | LOGGED
    | MAPPING
    | MATCH
    | MATCHED
    | MATERIALIZED
    | MAXVALUE
    | MERGE
    | METHOD
    | MINVALUE
    | MODE
    | MOVE
    | NAME
    | NAMES
    | NATIONAL
    | NATURAL
    | NCHAR
    | NEW
    | NEXT
    | NFC
    | NFD
    | NFKC
    | NFKD
    | NO
    | NONE
    | NORMALIZE
    | NORMALIZED
    | NOT
    | NOTHING
    | NOTIFY
    | NOWAIT
    | NULL
    | NULLIF
    | NULLS
    | NUMERIC
    | OBJECT
    | OF
    | OFF
    | OIDS
    | OLD
    | ONLY
    | OPERATOR
    | OPTION
    | OPTIONS
    | OR
    | ORDINALITY
    | OTHERS
    | OUT
    | OUTER
    | OVERLAY
    | OVERRIDING
    | OWNED
    | OWNER
    | PARALLEL
    | PARSER
    | PARTIAL
    | PARTITION
    | PASSING
    | PASSWORD
    | PLACING
    | PLANS
    | POLICY
    | POSITION
    | PRECEDING
    | PREPARE
    | PREPARED
    | PRESERVE
    | PRIMARY
    | PRIOR
    | PRIVILEGES
    | PROCEDURAL
    | PROCEDURE
    | PROCEDURES
    | PROGRAM
    | PUBLICATION
    | QUOTE
    | RANGE
    | READ
    | REAL
    | REASSIGN
    | RECHECK
    | RECURSIVE
    | REF
    | REFERENCES
    | REFERENCING
    | REFRESH
    | REINDEX
    | RELATIVE
    | RELEASE
    | RENAME
    | REPEATABLE
    | REPLACE
    | REPLICA
    | RESET
    | RESTART
    | RESTRICT
    | RETURN
    | RETURNS
    | REVOKE
    | RIGHT
    | ROLE
    | ROLLBACK
    | ROLLUP
    | ROUTINE
    | ROUTINES
    | ROW
    | ROWS
    | RULE
    | SAVEPOINT
    | SCHEMA
    | SCHEMAS
    | SCROLL
    | SEARCH
    | SECURITY
    | SELECT
    | SEQUENCE
    | SEQUENCES
    | SERIALIZABLE
    | SERVER
    | SESSION
    | SESSION_USER
    | SET
    | SETOF
    | SETS
    | SHARE
    | SHOW
    | SIMILAR
    | SIMPLE
    | SKIP_
    | SMALLINT
    | SNAPSHOT
    | SOME
    | SQL
    | STABLE
    | STANDALONE
    | START
    | STATEMENT
    | STATISTICS
    | STDIN
    | STDOUT
    | STORAGE
    | STORED
    | STRICT
    | STRIP
    | SUBSCRIPTION
    | SUBSTRING
    | SUPPORT
    | SYMMETRIC
    | SYSID
    | SYSTEM
    | TABLE
    | TABLES
    | TABLESAMPLE
    | TABLESPACE
    | TEMP
    | TEMPLATE
    | TEMPORARY
    | TEXT
    | THEN
    | TIES
    | TIME
    | TIMESTAMP
    | TRAILING
    | TRANSACTION
    | TRANSFORM
    | TREAT
    | TRIGGER
    | TRIM
    | TRUE
    | TRUNCATE
    | TRUSTED
    | TYPE
    | TYPES
    | UESCAPE
    | UNBOUNDED
    | UNCOMMITTED
    | UNENCRYPTED
    | UNIQUE
    | UNKNOWN
    | UNLISTEN
    | UNLOGGED
    | UNTIL
    | UPDATE
    | USER
    | USING
    | VACUUM
    | VALID
    | VALIDATE
    | VALIDATOR
    | VALUE
    | VALUES
    | VARCHAR
    | VARIADIC
    | VERBOSE
    | VERSION
    | VIEW
    | VIEWS
    | VOLATILE
    | WHEN
    | WHITESPACE
    | WORK
    | WRAPPER
    | WRITE
    | XML
    | XMLATTRIBUTES
    | XMLCONCAT
    | XMLELEMENT
    | XMLEXISTS
    | XMLFOREST
    | XMLNAMESPACES
    | XMLPARSE
    | XMLPI
    | XMLROOT
    | XMLSERIALIZE
    | XMLTABLE
    | YES
    | ZONE
    ;

tokens_nonreserved
    : ABORT
    | ABSOLUTE
    | ACCESS
    | ACTION
    | ADD
    | ADMIN
    | AFTER
    | AGGREGATE
    | ALSO
    | ALTER
    | ALWAYS
    | ASENSITIVE
    | ASSERTION
    | ASSIGNMENT
    | AT
    | ATOMIC
    | ATTACH
    | ATTRIBUTE
    | BACKWARD
    | BEFORE
    | BEGIN
    | BREADTH
    | BY
    | CACHE
    | CALL
    | CALLED
    | CASCADE
    | CASCADED
    | CATALOG
    | CHAIN
    | CHARACTERISTICS
    | CHECKPOINT
    | CLASS
    | CLOSE
    | CLUSTER
    | COLUMNS
    | COMMENT
    | COMMENTS
    | COMMIT
    | COMMITTED
    | COMPRESSION
    | CONFIGURATION
    | CONFLICT
    | CONNECTION
    | CONSTRAINTS
    | CONTENT
    | CONTINUE
    | CONVERSION
    | COPY
    | COST
    | CSV
    | CUBE
    | CURRENT
    | CURSOR
    | CYCLE
    | DATA
    | DATABASE
    | DAY
    | DEALLOCATE
    | DECLARE
    | DEFAULTS
    | DEFERRED
    | DEFINER
    | DELETE
    | DELIMITER
    | DELIMITERS
    | DEPENDS
    | DEPTH
    | DETACH
    | DICTIONARY
    | DISABLE
    | DISCARD
    | DOCUMENT
    | DOMAIN
    | DOUBLE
    | DROP
    | EACH
    | ENABLE
    | ENCODING
    | ENCRYPTED
    | ENUM
    | ESCAPE
    | EVENT
    | EXCLUDE
    | EXCLUDING
    | EXCLUSIVE
    | EXECUTE
    | EXPLAIN
    | EXPRESSION
    | EXTENSION
    | EXTERNAL
    | FAMILY
    | FILTER
    | FINALIZE
    | FIRST
    | FOLLOWING
    | FORCE
    | FORWARD
    | FUNCTION
    | FUNCTIONS
    | GENERATED
    | GLOBAL
    | GRANTED
    | GROUPS
    | HANDLER
    | HEADER
    | HOLD
    | HOUR
    | IDENTITY
    | IF
    | IMMEDIATE
    | IMMUTABLE
    | IMPLICIT
    | IMPORT
    | INCLUDE
    | INCLUDING
    | INCREMENT
    | INDEX
    | INDEXES
    | INHERIT
    | INHERITS
    | INLINE
    | INPUT
    | INSENSITIVE
    | INSERT
    | INSTEAD
    | INVOKER
    | ISOLATION
    | KEY
    | LABEL
    | LANGUAGE
    | LARGE
    | LAST
    | LEAKPROOF
    | LEVEL
    | LISTEN
    | LOAD
    | LOCAL
    | LOCATION
    | LOCK
    | LOCKED
    | LOGGED
    | MAPPING
    | MATCH
    | MATCHED
    | MATERIALIZED
    | MAXVALUE
    | MERGE
    | METHOD
    | MINUTE
    | MINVALUE
    | MODE
    | MONTH
    | MOVE
    | NAME
    | NAMES
    | NEW
    | NEXT
    | NFC
    | NFD
    | NFKC
    | NFKD
    | NO
    | NORMALIZED
    | NOTHING
    | NOTIFY
    | NOWAIT
    | NULLS
    | OBJECT
    | OF
    | OFF
    | OIDS
    | OLD
    | OPERATOR
    | OPTION
    | OPTIONS
    | ORDINALITY
    | OTHERS
    | OVER
    | OVERRIDING
    | OWNED
    | OWNER
    | PARALLEL
    | PARSER
    | PARTIAL
    | PARTITION
    | PASSING
    | PASSWORD
    | PLANS
    | POLICY
    | PRECEDING
    | PREPARE
    | PREPARED
    | PRESERVE
    | PRIOR
    | PRIVILEGES
    | PROCEDURAL
    | PROCEDURE
    | PROCEDURES
    | PROGRAM
    | PUBLICATION
    | QUOTE
    | RANGE
    | READ
    | REASSIGN
    | RECHECK
    | RECURSIVE
    | REF
    | REFERENCING
    | REFRESH
    | REINDEX
    | RELATIVE
    | RELEASE
    | RENAME
    | REPEATABLE
    | REPLACE
    | REPLICA
    | RESET
    | RESTART
    | RESTRICT
    | RETURN
    | RETURNS
    | REVOKE
    | ROLE
    | ROLLBACK
    | ROLLUP
    | ROUTINE
    | ROUTINES
    | ROWS
    | RULE
    | SAVEPOINT
    | SCHEMA
    | SCHEMAS
    | SCROLL
    | SEARCH
    | SECOND
    | SECURITY
    | SEQUENCE
    | SEQUENCES
    | SERIALIZABLE
    | SERVER
    | SESSION
    | SET
    | SETS
    | SHARE
    | SHOW
    | SIMPLE
    | SKIP_
    | SNAPSHOT
    | SQL
    | STABLE
    | STANDALONE
    | START
    | STATEMENT
    | STATISTICS
    | STDIN
    | STDOUT
    | STORAGE
    | STORED
    | STRICT
    | STRIP
    | SUBSCRIPTION
    | SUPPORT
    | SYSID
    | SYSTEM
    | TABLES
    | TABLESPACE
    | TEMP
    | TEMPLATE
    | TEMPORARY
    | TEXT
    | TIES
    | TRANSACTION
    | TRANSFORM
    | TRIGGER
    | TRUNCATE
    | TRUSTED
    | TYPE
    | TYPES
    | UESCAPE
    | UNBOUNDED
    | UNCOMMITTED
    | UNENCRYPTED
    | UNKNOWN
    | UNLISTEN
    | UNLOGGED
    | UNTIL
    | UPDATE
    | VACUUM
    | VALID
    | VALIDATE
    | VALIDATOR
    | VALUE
    | VARYING
    | VERSION
    | VIEW
    | VIEWS
    | VOLATILE
    | WHITESPACE
    | WITHIN
    | WITHOUT
    | WORK
    | WRAPPER
    | WRITE
    | XML
    | YEAR
    | YES
    | ZONE
    ;

tokens_nonreserved_for_func_name
    : NORMALIZE
    ;

tokens_nonreserved_except_function_type
    : BETWEEN
    | BIGINT
    | BIT
    | BOOLEAN
    | CHAR
    | CHARACTER
    | COALESCE
    | DEC
    | DECIMAL
    | EXISTS
    | EXTRACT
    | FLOAT
    | GREATEST
    | GROUPING
    | INOUT
    | INT
    | INTEGER
    | INTERVAL
    | LEAST
    | NATIONAL
    | NCHAR
    | NONE
    | NORMALIZE
    | NULLIF
    | NUMERIC
    | OUT
    | OVERLAY
    | POSITION
    | PRECISION
    | REAL
    | ROW
    | SETOF
    | SMALLINT
    | SUBSTRING
    | TIME
    | TIMESTAMP
    | TREAT
    | TRIM
    | VALUES
    | VARCHAR
    | XMLATTRIBUTES
    | XMLCONCAT
    | XMLELEMENT
    | XMLEXISTS
    | XMLFOREST
    | XMLNAMESPACES
    | XMLPARSE
    | XMLPI
    | XMLROOT
    | XMLSERIALIZE
    | XMLTABLE
    ;

tokens_reserved_except_function_type
    : AUTHORIZATION
    | BINARY
    | COLLATION
    | CONCURRENTLY
    | CROSS
    | CURRENT_SCHEMA
    | FREEZE
    | FULL
    | ILIKE
    | INNER
    | IS
    | ISNULL
    | JOIN
    | LEFT
    | LIKE
    | NATURAL
    | NOTNULL
    | OUTER
    | OVERLAPS
    | RIGHT
    | SIMILAR
    | TABLESAMPLE
    | VERBOSE
    ;

tokens_reserved
    : ALL
    | ANALYZE
    | AND
    | ANY
    | ARRAY
    | AS
    | ASC
    | ASYMMETRIC
    | BOTH
    | CASE
    | CAST
    | CHECK
    | COLLATE
    | COLUMN
    | CONSTRAINT
    | CREATE
    | CURRENT_CATALOG
    | CURRENT_DATE
    | CURRENT_ROLE
    | CURRENT_TIME
    | CURRENT_TIMESTAMP
    | CURRENT_USER
    | DEFAULT
    | DEFERRABLE
    | DESC
    | DISTINCT
    | DO
    | ELSE
    | END
    | EXCEPT
    | FALSE
    | FETCH
    | FOR
    | FOREIGN
    | FROM
    | GRANT
    | GROUP
    | HAVING
    | IN
    | INITIALLY
    | INTERSECT
    | INTO
    | LATERAL
    | LEADING
    | LIMIT
    | LOCALTIME
    | LOCALTIMESTAMP
    | NOT
    | NULL
    | OFFSET
    | ON
    | ONLY
    | OR
    | ORDER
    | PLACING
    | PRIMARY
    | REFERENCES
    | RETURNING
    | SELECT
    | SESSION_USER
    | SOME
    | SYMMETRIC
    | TABLE
    | THEN
    | TO
    | TRAILING
    | TRUE
    | UNION
    | UNIQUE
    | USER
    | USING
    | VARIADIC
    | WHEN
    | WHERE
    | WINDOW
    | WITH
    ;

tokens_nonkeyword
    : ALIGNMENT
    | ALLOW_CONNECTIONS
    | BASETYPE
    | BUFFERS
    | BYPASSRLS
    | CANONICAL
    | CATEGORY
    | COLLATABLE
    | COMBINEFUNC
    | COMMUTATOR
    | CONNECT
    | COSTS
    | CREATEDB
    | CREATEROLE
    | DESERIALFUNC
    | DETERMINISTIC
    | DISABLE_PAGE_SKIPPING
    | ELEMENT
    | EXTENDED
    | FINALFUNC
    | FINALFUNC_EXTRA
    | FINALFUNC_MODIFY
    | FORCE_NOT_NULL
    | FORCE_NULL
    | FORCE_QUOTE
    | FORMAT
    | GETTOKEN
    | GTCMP
    | HASH
    | HASHES
    | HEADLINE
    | HYPOTHETICAL
    | INDEX_CLEANUP
    | INIT
    | INITCOND
    | INITCOND1
    | INITCOND2
    | INTERNALLENGTH
    | IS_TEMPLATE
    | JSON
    | LC_COLLATE
    | LC_CTYPE
    | LEFTARG
    | LEXIZE
    | LEXTYPES
    | LIST
    | LOCALE
    | LOGIN
    | LTCMP
    | MAIN
    | MERGES
    | MFINALFUNC
    | MFINALFUNC_EXTRA
    | MFINALFUNC_MODIFY
    | MINITCOND
    | MINVFUNC
    | MODULUS
    | MSFUNC
    | MSSPACE
    | MSTYPE
    | MULTIRANGE_TYPE_NAME
    | NEGATOR
    | NOBYPASSRLS
    | NOCREATEDB
    | NOCREATEROLE
    | NOINHERIT
    | NOLOGIN
    | NOREPLICATION
    | NOSUPERUSER
    | OUTPUT
    | PASSEDBYVALUE
    | PATH
    | PERMISSIVE
    | PLAIN
    | PREFERRED
    | PROVIDER
    | READ_ONLY
    | READ_WRITE
    | RECEIVE
    | REMAINDER
    | REPLICATION
    | RESTRICTED
    | RESTRICTIVE
    | RIGHTARG
    | SAFE
    | SEND
    | SERIALFUNC
    | SETTINGS
    | SFUNC
    | SFUNC1
    | SFUNC2
    | SHAREABLE
    | SKIP_LOCKED
    | SORT1
    | SORT2
    | SORTOP
    | SSPACE
    | STYPE
    | STYPE1
    | STYPE2
    | SUBTYPE_DIFF
    | SUBTYPE_OPCLASS
    | SUBTYPE
    | SUBSCRIPT
    | SUMMARY
    | SUPERUSER
    | TIMING
    | TYPMOD_IN
    | TYPMOD_OUT
    | UNSAFE
    | USAGE
    | VARIABLE
    | WAL
    | YAML

    // plpgsql tokens
    | ALIAS
    | ASSERT
    | CONSTANT
    | DATATYPE
    | DEBUG
    | DETAIL
    | DIAGNOSTICS
    | ELSEIF
    | ELSIF
    | ERRCODE
    | EXIT
    | EXCEPTION
    | FOREACH
    | GET
    | HINT
    | INFO
    | LOG
    | LOOP
    | MESSAGE
    | NOTICE
    | OPEN
    | PERFORM
    | QUERY
    | RAISE
    | RECORD
    | REVERSE
    | ROWTYPE
    | SLICE
    | SQLSTATE
    | STACKED
    | WARNING
    | WHILE
    ;

/*
===============================================================================
  6.1 <data types>
===============================================================================
*/

schema_qualified_name_nontype
    : identifier_nontype
    | schema=identifier DOT identifier_nontype
    ;

type_list
    : data_type (COMMA data_type)*
    ;

data_type
    : SETOF? predefined_type (ARRAY array_type? | array_type+)?
    ;

array_type
    : LEFT_BRACKET NUMBER_LITERAL? RIGHT_BRACKET
    ;

predefined_type
    : BIGINT
    | BIT VARYING? type_length?
    | BOOLEAN
    | DEC precision_param?
    | DECIMAL precision_param?
    | DOUBLE PRECISION
    | FLOAT precision_param?
    | INT
    | INTEGER
    | INTERVAL interval_field? type_length?
    | NATIONAL? (CHARACTER | CHAR) VARYING? type_length?
    | NCHAR VARYING? type_length?
    | NUMERIC precision_param_with_signed_scale?
    | REAL
    | SMALLINT
    | TIME type_length? ((WITH | WITHOUT) TIME ZONE)?
    | TIMESTAMP type_length? ((WITH | WITHOUT) TIME ZONE)?
    | VARCHAR type_length?
    | schema_qualified_name_nontype (LEFT_PAREN vex (COMMA vex)* RIGHT_PAREN)?
    ;

interval_field
    : YEAR
    | MONTH
    | DAY
    | HOUR
    | MINUTE
    | SECOND
    | YEAR TO MONTH
    | DAY TO HOUR
    | DAY TO MINUTE
    | DAY TO SECOND
    | HOUR TO MINUTE
    | HOUR TO SECOND
    | MINUTE TO SECOND
    ;

type_length
    : LEFT_PAREN NUMBER_LITERAL RIGHT_PAREN
    ;

precision_param
    : LEFT_PAREN precision=NUMBER_LITERAL (COMMA scale=NUMBER_LITERAL)? RIGHT_PAREN
    ;

precision_param_with_signed_scale
    : LEFT_PAREN precision=NUMBER_LITERAL (COMMA scale=signed_number_literal)? RIGHT_PAREN
    ;

/*
===============================================================================
  6.25 <value expression>
===============================================================================
*/

vex
  : vex CAST_EXPRESSION data_type
  | LEFT_PAREN vex RIGHT_PAREN indirection_list?
  | LEFT_PAREN vex (COMMA vex)+ RIGHT_PAREN
  | vex collate_identifier
  | <assoc=right> (PLUS | MINUS) vex
  | vex AT TIME ZONE vex
  | vex EXP vex
  | vex (MULTIPLY | DIVIDE | MODULAR) vex
  | vex (PLUS | MINUS) vex
  | vex op vex
  | op vex
  | vex op
  | vex NOT? IN LEFT_PAREN (select_stmt_no_parens | vex (COMMA vex)*) RIGHT_PAREN
  | vex NOT? BETWEEN (ASYMMETRIC | SYMMETRIC)? vex_b AND vex
  | vex NOT? (LIKE | ILIKE | SIMILAR TO) vex
  | vex NOT? (LIKE | ILIKE | SIMILAR TO) vex ESCAPE vex
  | vex (LTH | GTH | LEQ | GEQ | EQUAL | NOT_EQUAL) vex
  | vex IS NOT? (truth_value | NULL)
  | vex IS NOT? DISTINCT FROM vex
  | vex IS NOT? DOCUMENT
  | vex IS NOT? UNKNOWN
  | vex IS NOT? (NFC | NFD | NFKC | NFKD)? NORMALIZED
  | vex IS NOT? OF LEFT_PAREN type_list RIGHT_PAREN
  | vex ISNULL
  | vex NOTNULL
  | <assoc=right> NOT vex
  | vex AND vex
  | vex OR vex
  | value_expression_primary
  ;

// partial copy of vex
// resolves (vex BETWEEN vex AND vex) vs. (vex AND vex) ambiguity
// vex references that are not at alternative edge are referencing the full rule
// see postgres' b_expr (src/backend/parser/gram.y)
vex_b
  : vex_b CAST_EXPRESSION data_type
  | LEFT_PAREN vex RIGHT_PAREN indirection_list?
  | LEFT_PAREN vex (COMMA vex)+ RIGHT_PAREN
  | <assoc=right> (PLUS | MINUS) vex_b
  | vex_b EXP vex_b
  | vex_b (MULTIPLY | DIVIDE | MODULAR) vex_b
  | vex_b (PLUS | MINUS) vex_b
  | vex_b op vex_b
  | op vex_b
  | vex_b (LTH | GTH | LEQ | GEQ | EQUAL | NOT_EQUAL) vex_b
  | vex_b IS NOT? DISTINCT FROM vex_b
  | vex_b IS NOT? DOCUMENT
  | vex_b IS NOT? UNKNOWN
  | vex_b IS NOT? OF LEFT_PAREN type_list RIGHT_PAREN
  | value_expression_primary
  ;

op
  : op_chars
  | OPERATOR LEFT_PAREN identifier DOT all_simple_op RIGHT_PAREN
  ;

all_op_ref
  : all_simple_op
  | OPERATOR LEFT_PAREN identifier DOT all_simple_op RIGHT_PAREN
  ;

datetime_overlaps
  : LEFT_PAREN vex COMMA vex RIGHT_PAREN OVERLAPS LEFT_PAREN vex COMMA vex RIGHT_PAREN
  ;

value_expression_primary
  : unsigned_value_specification
  | LEFT_PAREN select_stmt_no_parens RIGHT_PAREN indirection_list?
  | case_expression
  | NULL
  | MULTIPLY
  // technically incorrect since ANY cannot be value expression
  // but fixing this would require to write a vex rule duplicating all operators
  // like vex (op|op|op|...) comparison_mod
  | comparison_mod
  | EXISTS table_subquery
  | function_call
  | indirection_var
  | array_expression
  | type_coercion
  | datetime_overlaps
  ;

unsigned_value_specification
  : unsigned_numeric_literal
  | character_string
  | truth_value
  ;

unsigned_numeric_literal
  : NUMBER_LITERAL
  | REAL_NUMBER
  ;

truth_value
  : TRUE | FALSE | ON // on is reserved but is required by SET statements
  ;

case_expression
  : CASE vex? (WHEN vex THEN r+=vex)+ (ELSE r+=vex)? END
  ;

cast_specification
  : (CAST | TREAT) LEFT_PAREN vex AS data_type RIGHT_PAREN
  ;

// using data_type for function name because keyword-named functions
// use the same category of keywords as keyword-named types
function_call
    : schema_qualified_name_for_func_name LEFT_PAREN (set_qualifier? vex_or_named_notation (COMMA vex_or_named_notation)* orderby_clause?)? RIGHT_PAREN
        (WITHIN GROUP LEFT_PAREN orderby_clause RIGHT_PAREN)?
        filter_clause? (OVER (identifier | window_definition))?
    | function_construct
    | extract_function
    | system_function
    | date_time_function
    | string_value_function
    | xml_function
    ;

vex_or_named_notation
    : VARIADIC? (argname=identifier pointer)? vex
    ;

pointer
    : EQUAL_GTH | COLON_EQUAL
    ;

function_construct
    : (COALESCE | GREATEST | GROUPING | LEAST | NULLIF | XMLCONCAT) LEFT_PAREN vex (COMMA vex)* RIGHT_PAREN
    | ROW LEFT_PAREN (vex (COMMA vex)*)? RIGHT_PAREN
    ;

extract_function
    : EXTRACT LEFT_PAREN (identifier | character_string) FROM vex RIGHT_PAREN
    ;

system_function
    : CURRENT_CATALOG
    // parens are handled by generic function call
    // since current_schema is defined as reserved(can be function) keyword
    | CURRENT_SCHEMA /*(LEFT_PAREN RIGHT_PAREN)?*/
    | CURRENT_ROLE
    | CURRENT_USER
    | SESSION_USER
    | USER
    | cast_specification
    ;

date_time_function
    : CURRENT_DATE
    | CURRENT_TIME type_length?
    | CURRENT_TIMESTAMP type_length?
    | LOCALTIME type_length?
    | LOCALTIMESTAMP type_length?
    ;

string_value_function
    : TRIM LEFT_PAREN (LEADING | TRAILING | BOTH)? (chars=vex FROM str=vex | FROM? str=vex (COMMA chars=vex)?) RIGHT_PAREN
    | SUBSTRING LEFT_PAREN vex (COMMA vex)* ((FROM | SIMILAR) vex)? ((FOR | ESCAPE) vex)? RIGHT_PAREN
    | POSITION LEFT_PAREN vex_b IN vex RIGHT_PAREN
    | OVERLAY LEFT_PAREN vex PLACING vex FROM vex (FOR vex)? RIGHT_PAREN
    | COLLATION FOR LEFT_PAREN vex RIGHT_PAREN
    ;

xml_function
    : XMLELEMENT LEFT_PAREN NAME name=identifier
        (COMMA XMLATTRIBUTES LEFT_PAREN vex (AS attname=identifier)? (COMMA vex (AS attname=identifier)?)* RIGHT_PAREN)?
        (COMMA vex)* RIGHT_PAREN
    | XMLFOREST LEFT_PAREN vex (AS name=identifier)? (COMMA vex (AS name=identifier)?)* RIGHT_PAREN
    | XMLPI LEFT_PAREN NAME name=identifier (COMMA vex)? RIGHT_PAREN
    | XMLROOT LEFT_PAREN vex COMMA VERSION (vex | NO VALUE) (COMMA STANDALONE (YES | NO | NO VALUE))? RIGHT_PAREN
    | XMLEXISTS LEFT_PAREN vex PASSING (BY REF)? vex (BY REF)? RIGHT_PAREN
    | XMLPARSE LEFT_PAREN (DOCUMENT | CONTENT) vex RIGHT_PAREN
    | XMLSERIALIZE LEFT_PAREN (DOCUMENT | CONTENT) vex AS data_type RIGHT_PAREN
    | XMLTABLE LEFT_PAREN
        (XMLNAMESPACES LEFT_PAREN vex AS name=identifier (COMMA vex AS name=identifier)* RIGHT_PAREN COMMA)?
        vex PASSING (BY REF)? vex (BY REF)?
        COLUMNS xml_table_column (COMMA xml_table_column)*
        RIGHT_PAREN
    ;

xml_table_column
    : name=identifier (data_type (PATH vex)? (DEFAULT vex)? (NOT? NULL)? | FOR ORDINALITY)
    ;

comparison_mod
    : (ALL | ANY | SOME) LEFT_PAREN (vex | select_stmt_no_parens) RIGHT_PAREN
    ;

filter_clause
    : FILTER LEFT_PAREN WHERE vex RIGHT_PAREN
    ;

window_definition
    : LEFT_PAREN identifier? partition_by_columns? orderby_clause? frame_clause? RIGHT_PAREN
    ;

frame_clause
    : (RANGE | ROWS | GROUPS) (frame_bound | BETWEEN frame_bound AND frame_bound)
    (EXCLUDE (CURRENT ROW | GROUP | TIES | NO OTHERS))?
    ;

frame_bound
    : vex (PRECEDING | FOLLOWING)
    | CURRENT ROW
    ;

array_expression
    : ARRAY (array_elements | table_subquery)
    ;

array_elements
    : LEFT_BRACKET ((vex | array_elements) (COMMA (vex | array_elements))*)? RIGHT_BRACKET
    ;

type_coercion
    : data_type character_string
    | INTERVAL character_string interval_field type_length?
    ;

/*
===============================================================================
  7.13 <query expression>
===============================================================================
*/
schema_qualified_name
    : identifier ( DOT identifier ( DOT identifier )? )?
    ;

schema_qualified_name_for_func_name
    : identifier_for_func_name
    | schema=identifier DOT identifier_for_func_name
    ;

set_qualifier
    : DISTINCT | ALL
    ;

table_subquery
    : LEFT_PAREN select_stmt RIGHT_PAREN
    ;

subquery_short_syntax
    : select_sublist select_primary_body
    ;

select_stmt
    : with_clause? select_ops after_ops*
    ;

plpgsql_select_stmt
    : with_clause? plpgsql_select_ops plpgsql_after_ops*
    ;

after_ops
    : orderby_clause
    | LIMIT (vex | ALL)
    | OFFSET vex (ROW | ROWS)?
    | FETCH (FIRST | NEXT) vex? (ROW | ROWS) (ONLY | WITH TIES)?
    | FOR (UPDATE | NO KEY UPDATE | SHARE | KEY SHARE) (OF schema_qualified_name (COMMA schema_qualified_name)*)? (NOWAIT | SKIP_ LOCKED)?
    ;

plpgsql_after_ops
    : orderby_clause
    | LIMIT (vex | ALL)
    | OFFSET vex (ROW | ROWS)?
    | FETCH (FIRST | NEXT) vex? (ROW | ROWS) (ONLY | WITH TIES)?
    | FOR (UPDATE | NO KEY UPDATE | SHARE | KEY SHARE) (OF schema_qualified_name (COMMA schema_qualified_name)*)? (NOWAIT | SKIP_ LOCKED)?
    | into_var
    ;

// select_stmt copy that doesn't consume external parens
// for use in vex
// we let the vex rule to consume as many parens as it can
select_stmt_no_parens
    : with_clause? select_ops_no_parens after_ops*
    ;

with_clause
    : WITH RECURSIVE? with_query (COMMA with_query)*
    ;

with_query
    : query_name=identifier (LEFT_PAREN column_name+=identifier (COMMA column_name+=identifier)* RIGHT_PAREN)?
    AS (NOT? MATERIALIZED)? LEFT_PAREN data_statement RIGHT_PAREN
    (SEARCH (BREADTH | DEPTH) FIRST BY identifier (COMMA identifier)* SET identifier)?
    (CYCLE identifier (COMMA identifier)* SET identifier (TO vex DEFAULT vex)? USING identifier )?
    ;

select_ops
    : LEFT_PAREN select_stmt RIGHT_PAREN // parens can be used to apply "global" clauses (WITH etc) to a particular select in UNION expr
    | select_ops (INTERSECT | UNION | EXCEPT) set_qualifier? select_ops
    | select_primary
    ;

plpgsql_select_ops
    : LEFT_PAREN plpgsql_select_stmt RIGHT_PAREN // parens can be used to apply "global" clauses (WITH etc) to a particular select in UNION expr
    | plpgsql_select_ops (INTERSECT | UNION | EXCEPT) set_qualifier? plpgsql_select_ops
    | plpgsql_select_primary
    ;

// version of select_ops for use in select_stmt_no_parens
select_ops_no_parens
    : select_ops (INTERSECT | UNION | EXCEPT) set_qualifier? (select_primary | LEFT_PAREN select_stmt RIGHT_PAREN)
    | select_primary
    ;

// also change perform_stmt when making changes here
select_primary
    : SELECT (set_qualifier (ON LEFT_PAREN vex (COMMA vex)* RIGHT_PAREN)?)?
        select_list? into_table? select_primary_body
    | TABLE ONLY? schema_qualified_name MULTIPLY?
    | values_stmt
    ;

select_primary_body
    : (FROM from_item (COMMA from_item)*)?
        (WHERE vex)?
        groupby_clause?
        (HAVING vex)?
        (WINDOW identifier AS window_definition (COMMA identifier AS window_definition)*)?
    ;

plpgsql_select_primary
    : SELECT
        (set_qualifier (ON LEFT_PAREN vex (COMMA vex)* RIGHT_PAREN)?)?
        into_var? select_list? into_var?
        (FROM from_item (COMMA from_item)*)? into_var?
        (WHERE vex)? into_var?
        groupby_clause? into_var?
        (HAVING vex)? into_var?
        (WINDOW identifier AS window_definition (COMMA identifier AS window_definition)*)? into_var?
    | TABLE ONLY? schema_qualified_name MULTIPLY?
    | values_stmt
    ;

select_list
  : select_sublist (COMMA select_sublist)*
  ;

select_sublist
  : vex (AS col_label | bare_col_label)?
  ;

into_table
    : INTO (TEMPORARY | TEMP | UNLOGGED)? TABLE? schema_qualified_name
    ;

from_item
    : LEFT_PAREN from_item RIGHT_PAREN alias_clause?
    | from_item CROSS JOIN from_item
    | from_item (INNER | (LEFT | RIGHT | FULL) OUTER?)? JOIN from_item ON vex
    | from_item (INNER | (LEFT | RIGHT | FULL) OUTER?)? JOIN from_item USING names_in_parens alias_clause?
    | from_item NATURAL (INNER | (LEFT | RIGHT | FULL) OUTER?)? JOIN from_item
    | from_primary
    ;

from_primary
    : ONLY? schema_qualified_name MULTIPLY? alias_clause? (TABLESAMPLE method=identifier LEFT_PAREN vex (COMMA vex)* RIGHT_PAREN (REPEATABLE vex)?)?
    | LATERAL? table_subquery alias_clause
    | LATERAL? function_call (WITH ORDINALITY)?
        (AS from_function_column_def
        | AS? alias=identifier (LEFT_PAREN column_alias+=identifier (COMMA column_alias+=identifier)* RIGHT_PAREN | from_function_column_def)?
        )?
    | LATERAL? ROWS FROM LEFT_PAREN function_call (AS from_function_column_def)? (COMMA function_call (AS from_function_column_def)?)* RIGHT_PAREN
    (WITH ORDINALITY)? (AS? identifier (LEFT_PAREN identifier (COMMA identifier)* RIGHT_PAREN)?)?
    ;

alias_clause
    : AS? alias=identifier (LEFT_PAREN column_alias+=identifier (COMMA column_alias+=identifier)* RIGHT_PAREN)?
    ;

from_function_column_def
    : LEFT_PAREN column_alias+=identifier data_type (COMMA column_alias+=identifier data_type)* RIGHT_PAREN
    ;

groupby_clause
  : GROUP BY (ALL | DISTINCT)? grouping_element_list
  ;

grouping_element_list
  : grouping_element (COMMA grouping_element)*
  ;

grouping_element
  : vex
  | LEFT_PAREN RIGHT_PAREN
  | (ROLLUP | CUBE | GROUPING SETS) LEFT_PAREN grouping_element_list RIGHT_PAREN
  ;

values_stmt
    : VALUES values_values (COMMA values_values)*
    ;

values_values
    : LEFT_PAREN (vex | DEFAULT) (COMMA (vex | DEFAULT))* RIGHT_PAREN
    ;

orderby_clause
    : ORDER BY sort_specifier (COMMA sort_specifier)*
    ;

sort_specifier
    : vex order_specification? null_ordering?
    ;

order_specification
    : ASC | DESC | USING all_op_ref
    ;

null_ordering
    : NULLS (FIRST | LAST)
    ;

insert_stmt
    : with_clause? INSERT INTO insert_table_name=schema_qualified_name (AS alias=identifier)?
    insert_columns?
    (OVERRIDING (SYSTEM | USER) VALUE)?
    (select_stmt | DEFAULT VALUES)
    (ON CONFLICT conflict_object? conflict_action)?
    (RETURNING select_list into_var?)?
    ;

into_var
    : INTO STRICT? var (',' var)*
    ;

insert_columns
    : LEFT_PAREN indirection_identifier (COMMA indirection_identifier)* RIGHT_PAREN
    ;

indirection_identifier
    : identifier indirection_list?
    ;

conflict_object
    : index_columns index_where?
    | ON CONSTRAINT identifier
    ;

conflict_action
    : DO NOTHING
    | DO UPDATE SET update_set (COMMA update_set)* (WHERE vex)?
    ;

delete_stmt
    : with_clause? DELETE FROM ONLY? delete_table_name=schema_qualified_name MULTIPLY? (AS? alias=identifier)?
    (USING from_item (COMMA from_item)*)?
    (WHERE (vex | CURRENT OF cursor=identifier))?
    (RETURNING select_list into_var?)?
    ;

merge_stmt
    : with_clause?
        MERGE INTO ONLY? update_table_name=schema_qualified_name MULTIPLY? (AS? alias=identifier)?
        USING from_item ON vex
        merge_when_clause+
    ;

merge_when_clause
    : WHEN MATCHED (AND vex)? THEN (merge_update_action | DELETE | DO NOTHING)
    | WHEN NOT MATCHED (AND vex)? THEN (merge_insert_action | DO NOTHING)
    ;

merge_insert_action
    : INSERT merge_columns?
        (OVERRIDING (SYSTEM | USER) VALUE)?
        (VALUES values_values | DEFAULT VALUES)
    ;

merge_update_action
    : UPDATE SET
        (identifier EQUAL (vex | DEFAULT) (COMMA identifier EQUAL (vex | DEFAULT))*
        | merge_columns EQUAL values_values (COMMA values_values)*)
    ;

update_stmt
    : with_clause? UPDATE ONLY? update_table_name=schema_qualified_name MULTIPLY? (AS? alias=identifier)?
    SET update_set (COMMA update_set)*
    (FROM from_item (COMMA from_item)*)?
    (WHERE (vex | CURRENT OF cursor=identifier))?
    (RETURNING select_list into_var?)?
    ;

update_set
    : column+=indirection_identifier EQUAL (value+=vex | DEFAULT)
    | LEFT_PAREN column+=indirection_identifier (COMMA column+=indirection_identifier)* RIGHT_PAREN EQUAL ROW?
    (LEFT_PAREN (value+=vex | DEFAULT) (COMMA (value+=vex | DEFAULT))* RIGHT_PAREN | table_subquery)
    ;

notify_stmt
    : NOTIFY channel=identifier (COMMA payload=character_string)?
    ;

truncate_stmt
    : TRUNCATE TABLE? only_table_multiply (COMMA only_table_multiply)*
    ((RESTART | CONTINUE) IDENTITY)? cascade_restrict?
    ;

identifier_list
    : identifier (COMMA identifier)*
    ;

anonymous_block
    : DO (LANGUAGE (identifier | character_string))? character_string
    | DO character_string LANGUAGE (identifier | character_string)
    ;

// plpgsql rules

comp_options
    : HASH_SIGN identifier (identifier | truth_value)
    ;

function_block
    : start_label? declarations?
    BEGIN plpgsql_function_statements exception_statement?
    END end_label=identifier?
    ;

start_label
    : LESS_LESS col_label GREATER_GREATER
    ;

declarations
    : DECLARE declaration*
    ;

declaration
    : DECLARE* identifier type_declaration SEMI_COLON
    ;

type_declaration
    : CONSTANT? data_type_dec collate_identifier? (NOT NULL)? ((DEFAULT | COLON_EQUAL | EQUAL) (vex | subquery_short_syntax))?
    | ALIAS FOR (identifier | DOLLAR_NUMBER)
    | (NO? SCROLL)? CURSOR (LEFT_PAREN arguments_list RIGHT_PAREN)? (FOR | IS) select_stmt
    ;

arguments_list
    : identifier data_type (COMMA identifier data_type)*
    ;

data_type_for_func_args_or_retval
    : data_type
    | SETOF? schema_qualified_name MODULAR TYPE
    ;

data_type_dec
    : data_type
    | schema_qualified_name MODULAR TYPE
    | schema_qualified_name_nontype MODULAR ROWTYPE
    ;

exception_statement
    : EXCEPTION (WHEN vex THEN plpgsql_function_statements)+
    ;

plpgsql_function_statements
    : (plpgsql_function_statement SEMI_COLON)*
    ;

function_statement
    : schema_statement
    | data_statement
    | lock_table
    | CALL function_call
    | cursor_statement
    | script_additional
    | additional_statement
    ;

plpgsql_function_statement
    : function_block
    | base_statement
    | control_statement
    | transaction_statement
    | cursor_statement
    | message_statement
    | schema_statement
    | plpgsql_query
    | additional_statement
    ;

base_statement
    : assign_stmt
    | PERFORM perform_stmt
    | GET (CURRENT | STACKED)? DIAGNOSTICS diagnostic_option (COMMA diagnostic_option)*
    | NULL
    ;

var
    : indirection_var
    ;

diagnostic_option
    : var (COLON_EQUAL | EQUAL) identifier
    ;

// keep this in sync with select_primary (except intended differences)
perform_stmt
    : (set_qualifier (ON LEFT_PAREN vex (COMMA vex)* RIGHT_PAREN)?)?
    select_list?
    (FROM from_item (COMMA from_item)*)?
    (WHERE vex)?
    groupby_clause?
    (HAVING vex)?
    (WINDOW identifier AS window_definition (COMMA identifier AS window_definition)*)?
    ((INTERSECT | UNION | EXCEPT) set_qualifier? select_ops)?
    after_ops*
    ;

assign_stmt
    : var (COLON_EQUAL | EQUAL) (select_stmt_no_parens | perform_stmt)
    ;

execute_stmt
    : EXECUTE vex into_var? using_vex? into_var?
    ;

control_statement
    : return_stmt
    | CALL function_call
    | if_statement
    | case_statement
    | loop_statement
    ;

cursor_statement
    : OPEN var (NO? SCROLL)? FOR plpgsql_query
    | OPEN var (LEFT_PAREN option (COMMA option)* RIGHT_PAREN)?
    | FETCH fetch_move_direction? (FROM | IN)? var into_var
    | MOVE fetch_move_direction? (FROM | IN)? var
    | CLOSE var
    ;

option
    : (identifier COLON_EQUAL)? vex
    ;

transaction_statement
    : (COMMIT | ROLLBACK) (AND NO? CHAIN)?
    | lock_table
    ;

message_statement
    : RAISE log_level? (character_string (COMMA vex)*)? raise_using?
    | RAISE log_level? identifier raise_using?
    | RAISE log_level? SQLSTATE character_string raise_using?
    | ASSERT vex (COMMA vex)?
    ;

log_level
    : DEBUG
    | LOG
    | INFO
    | NOTICE
    | WARNING
    | EXCEPTION
    ;

raise_using
    : USING raise_param EQUAL vex (COMMA raise_param EQUAL vex)*
    ;

raise_param
    : MESSAGE
    | DETAIL
    | HINT
    | ERRCODE
    | COLUMN
    | CONSTRAINT
    | DATATYPE
    | TABLE
    | SCHEMA
    ;

return_stmt
    : RETURN
    | RETURN perform_stmt
    | RETURN NEXT vex
    | RETURN QUERY plpgsql_query
    ;

loop_statement
    : start_label? loop_start? LOOP plpgsql_function_statements END LOOP identifier?
    | (EXIT | CONTINUE) col_label? (WHEN vex)?
    ;

loop_start
    : WHILE vex
    | FOR alias=identifier IN REVERSE? vex DOUBLE_DOT vex (BY vex)?
    | FOR identifier_list IN plpgsql_query
    | FOR cursor=identifier IN identifier (LEFT_PAREN option (COMMA option)* RIGHT_PAREN)? // cursor loop
    | FOREACH identifier_list (SLICE NUMBER_LITERAL)? IN ARRAY vex
    ;

using_vex
    : USING vex (COMMA vex)*
    ;

if_statement
    : IF (vex | subquery_short_syntax) THEN plpgsql_function_statements ((ELSIF | ELSEIF) vex THEN plpgsql_function_statements)* (ELSE plpgsql_function_statements)? END IF
    ;

// plpgsql case
case_statement
    : CASE vex? (WHEN vex (COMMA vex)* THEN plpgsql_function_statements)+ (ELSE plpgsql_function_statements)? END CASE
    ;

plpgsql_query
    : plpgsql_data_statement
    | execute_stmt
    | show_statement
    | explain_statement
    ;
